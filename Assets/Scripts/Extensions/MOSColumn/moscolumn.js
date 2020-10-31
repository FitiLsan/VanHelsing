
function CalculateValue() {
    // the explaination of this MOS implementation can be found here: https://www.pingman.com/kb/article/how-is-mos-calculated-in-pingplotter-pro-50.html
    // variables that are PascalCased come from PingPlotter, you can find a full list here:
    var percentPacketLoss = (LostPacketCount / TotalPacketCount) * 100;
    var effectiveLatency = (Average + Jitter * 2 + 10);

    var rValue;
    if (effectiveLatency < 160) {
        rValue = 93.2 - (effectiveLatency / 40);
    } else {
        rValue = 93.2 - (effectiveLatency - 120) / 10;
    }

    rValue = Math.max(rValue - (percentPacketLoss * 2.5), 0);

    var MOS = 1 + (0.035) * rValue + (0.000007) * rValue * (rValue - 60) * (100 - rValue);
    MOS = Math.round(MOS * 100) / 100;

    var displayVal = isNaN(MOS) ? "" : MOS.toFixed(2);
    var mosVal = isNaN(MOS) ? 0 : MOS;

    return mosVal;
}