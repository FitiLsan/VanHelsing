

var _traceStates = {};

var textEncoding = importNamespace('System.Text').Encoding;
var invariantCulture = importNamespace('System.Globalization').CultureInfo.InvariantCulture
var httpGetMethod = importNamespace('System.Net').Http.HttpMethod.Get;

// By default, only 2 connections at a time, which isn't good for remote agent.
importNamespace('System.Net').ServicePointManager.DefaultConnectionLimit = 250; // Set to 250 concurrent connections.

// regex to pull relevant data out of a traceroute output line
var traceLineRegex = new RegExp(/\s*(\d+)\s+(\*|((\d+\.\d+\.\d+\.\d+)|(([A-Fa-f0-9]{1,4}::?){1,7}[A-Fa-f0-9]{1,4}))\s+(\([^\)]*\)\s+)?(\*|\d+\.?\d*))/i);

var httpClient;

function init() {

    /*var requestHandler = new System.Net.Http.WebRequestHandler();

    // set read/write timeout to 60 seconds, this needs to be enough time for the remote side to be busy
    requestHandler.ReadWriteTimeout = 60000;

    httpClient = new System.Net.Http.HttpClient(requestHandler);*/

    httpClient = CreateHttpClient();

    // set connect timeout to 15 seconds
    httpClient.Timeout = System.TimeSpan.FromSeconds(15);
}

function matchResponseBodyToPacketRequests(traceState) {

    traceState.packets.forEach(function (packet) {

        // check only packets which do not yet have a reply or timeout
        if (!packet.IsFinished) {

            // check if any lines in our response can finish this packet
            traceState.parsedResponses.forEach(function (parsedLine) {

                // match probe latencies (remote agent replies with hop 128 for probe, the probe request hop is 255)
                if ((parsedLine.hop === 128 && packet.Hop === 255)

                    // if the hop numbers match exactly
                    || (parsedLine.hop === packet.Hop)

                    // if the parsed line is both a final destination response and its a lower hop number
                    || (parsedLine.hop < packet.Hop && packet.TargetAddress.Equals(parsedLine.address))
                ) {

                    if (parsedLine.didTimeout) {
                        packet.SetDidTimeout();
                    }
                    else {
                        packet.SetReplyAddress(parsedLine.address);
                        packet.SetLatency(parsedLine.latency);
                    }

                    OnPacketResponse(packet);
                }
            });

            // if we won't get anymore data from the remote agent and this packet has still not been finished
            if (traceState.getRequestCompleted === true && !packet.IsFinished) {
                // error with this http request, set packet as refused
                if (traceState.getRequestError) {
                    packet.SetNotParticipating(traceState.getRequestError);
                }
                    // trace request was throttled by the server
                else if (traceState.serviceUnavailable === true) {
                    packet.SetIgnored();
                }
                    // request completed normally
                else {
                    packet.SetDidTimeout();
                }
                OnPacketResponse(packet);
            }
        }
    });
}

// reads a stream line by line (ASAP)
function readStreamByLine(stream, callback) {
    var streamReader = new System.IO.StreamReader(stream, textEncoding.UTF8);
    var wholeResponse = "";
    var readNextLine = function readNextLine() {
        Await(streamReader.ReadLineAsync(), function (err, line) {
            if (line != null) {
                wholeResponse += line + System.Environment.NewLine;
                callback(wholeResponse, line);
                readNextLine();
            }
            else {
                // finished reading entire response, notify callback by passing null for line
                callback(wholeResponse, null);
            }
        });
    };
    readNextLine();
}

function parseResponseLine(traceState, line) {

    var reponsePreIndex = traceState.response.indexOf("<PRE>");
    // response has no sample data lines yet so return
    if (reponsePreIndex === -1) {
        return false;
    }

    var parts = traceLineRegex.exec(line);
    if (parts === null) {
        return false;
    }

    var hopNum = parseInt(parts[1]);
    var isTimeout = (parts[2] === "*") || (parts[8] === "*");
    var ip = isTimeout ? null : System.Net.IPAddress.Parse(parts[3]);
    var latency = isTimeout ? 0 : System.Single.Parse(parts[8].replace(',', '.'), invariantCulture);

    traceState.parsedResponses.push({
        hop: hopNum,
        address: ip,
        didTimeout: isTimeout,
        latency: latency
    });

    return true;
}

function setTraceStateError(traceState, requestError) {
    traceState.getRequestError = requestError;
    traceState.getRequestCompleted = true;
    traceState.getRequestCompletedTimestamp = currentTimestamp();
    matchResponseBodyToPacketRequests(traceState);
}

function setTraceStateIgnored(traceState) {
    traceState.serviceUnavailable = true;
    traceState.getRequestCompleted = true;
    traceState.getRequestCompletedTimestamp = currentTimestamp();
    matchResponseBodyToPacketRequests(traceState);
}

function startTrace(traceState, packet) {

    var remoteAgentAddress = packet.GeneratorSettings.Settings.RemoteAgentAddress.trim();

    // if no protocol/scheme specified, assume http
    if (remoteAgentAddress.indexOf("://") === -1) {
        remoteAgentAddress = "http://" + remoteAgentAddress;
    }

    var remoteUriBase;

    // try parse remote endpoint input in the full URI format (ex: "http://192.168.32.70:7465/trace.php")
    if (System.Uri.TryCreate(remoteAgentAddress, System.UriKind.Absolute, remoteUriBase)) {
        remoteUriBase = new System.Uri(remoteAgentAddress, System.UriKind.Absolute);
    }
    else {
        var invalidUrlMsg = "Invalid remote agent address: " + remoteAgentAddress;
        setTraceStateError(traceState, invalidUrlMsg);
        return;
    }

    var remoteEndpoint = remoteUriBase.Scheme + "://" + remoteUriBase.Authority + remoteUriBase.AbsolutePath + (remoteUriBase.Query || "?")
        + "&IP=" + packet.TargetAddress.ToString()
        + "&TimeoutTime=" + packet.Timeout.TotalMilliseconds
        + "&PacketSize=" + packet.PacketSize
        + "&UniqueID=" + packet.CollectorID
        + "&GuaranteeReply=1";

    var httpRequest = new System.Net.Http.HttpRequestMessage(httpGetMethod, new System.Uri(remoteEndpoint));

    if (!System.String.IsNullOrWhiteSpace(packet.GeneratorSettings.Settings.AuthUsername) ||
        !System.String.IsNullOrWhiteSpace(packet.GeneratorSettings.Settings.AuthPassword)) {
        var authString = packet.GeneratorSettings.Settings.AuthUsername + ":" + packet.GeneratorSettings.Settings.AuthPassword;
        var authByteArray = textEncoding.ASCII.GetBytes(authString);
        var b64Auth = System.Convert.ToBase64String(authByteArray);
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", b64Auth);
    }

    // create an http stream request task to the given remote agent for this target
    var httpGet = httpClient.SendAsync(httpRequest, System.Net.Http.HttpCompletionOption.ResponseHeadersRead);

    Await(httpGet, function (err, responseMessage) {

        if (err) {
            var exc = (err.InnerException || err);
            var excType = (err.GetType().Name);
            if (excType == "TaskCanceledException") {
               setTraceStateError(traceState, "Timed out connecting to remote agent.");
            } else {
               var msg = exc.Message || err;
               setTraceStateError(traceState, "HTTP request error (connect): " + msg);
            }
            return;
        }

        var statusCode = responseMessage.StatusCode;

        if (statusCode == 503) {
            responseMessage.Dispose();
            setTraceStateIgnored(traceState);
            return;
        }

        if (statusCode != 200) {
            responseMessage.Dispose();
            setTraceStateError(traceState, "HTTP request error, status code " + statusCode);
            return;
        }

        var httpStream = responseMessage.Content.ReadAsStreamAsync();

        Await(httpStream, function (err, stream) {

            if (err) {
                responseMessage.Dispose();
                setTraceStateError(traceState, "HTTP request getting response: " + err);
                return;
            }

            // once we have the stream established, read from it line by line
            readStreamByLine(stream, function (result, line) {

                // we finished reading the response
                if (line === null) {
                    responseMessage.Dispose();
                    httpStream.Dispose();
                    traceState.getRequestCompleted = true;
                    traceState.getRequestCompletedTimestamp = currentTimestamp();
                    matchResponseBodyToPacketRequests(traceState);
                }
                else {
                    traceState.response = result;

                    // try to parse a sample from this line of data
                    if (parseResponseLine(traceState, line)) {

                        // we parsed one, try to match it up with a packet request
                        matchResponseBodyToPacketRequests(traceState);
                    }
                }
            });

        });
    });
}

function currentTimestamp() {
    return new Date().getTime() / 1000;
}

// removes cached trace states that had their http get request complete longer than 10 seconds ago
function clearOldTraceStates() {

    var now = currentTimestamp();

    Object.keys(_traceStates).forEach(function (key) {
        var traceState = _traceStates[key];
        if (traceState.getRequestCompleted === true && (now - traceState.getRequestCompletedTimestamp) > 10) {
            delete _traceStates[key];
        }
    });
}

function sendPacket(packet) {

    var traceBucketID = packet.CollectorID + "_" + Number(packet.TraceDateTime);


    var traceState = _traceStates[traceBucketID];

    if (traceState !== undefined) {
        // we found an existing trace state for this collector's trace number
        traceState.packets.push(packet);
        // check if any data from the trace request can fulfill this packet request
        matchResponseBodyToPacketRequests(traceState);
    }
    else if (packet.IsTargetProbe) {
        // this is the first packet request of a new trace
        traceState = {
            packets: [packet],
            response: "",
            parsedResponses: [],
            getRequestCompleted: false
        };

        // before starting a new trace, clear out any old ones
        clearOldTraceStates();

        _traceStates[traceBucketID] = traceState;

        startTrace(traceState, packet);
    }
    else {
        // this packet request came too late, we serviced the previous ones and cleared the trace state.
        // we no longer have data about this trace so refuse to service it
        packet.SetNotParticipating("Requested packet after trace had be garbage collected");
        OnPacketResponse(packet);
    }
}