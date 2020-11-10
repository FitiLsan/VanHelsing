using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static partial class ParseUtils
    {
        private static NumberFormatInfo _CommaFormatInfo;

        private static NumberFormatInfo CommaFormatInfo
        {
            get
            {
                if (_CommaFormatInfo == null)
                {
                    _CommaFormatInfo = new NumberFormatInfo();
                    _CommaFormatInfo.NumberDecimalSeparator = ",";
                }

                return _CommaFormatInfo;
            }
        }

        private static NumberFormatInfo _DotFormatInfo;

        private static NumberFormatInfo DotFormatInfo
        {
            get
            {
                if (_DotFormatInfo == null)
                {
                    _DotFormatInfo = new NumberFormatInfo();
                    _DotFormatInfo.NumberDecimalSeparator = ".";
                }

                return _DotFormatInfo;
            }
        }
        
        public static float ParseStringToFloat(string s)
        {
            NumberFormatInfo curFormatInfo = null;
            if (s.Contains(","))
            {
                curFormatInfo = CommaFormatInfo;
            }
            else
            {
                curFormatInfo = DotFormatInfo;
            }

            return Single.Parse(s, curFormatInfo);
        }

        public static float SafeParseFloat(string s, float defaultValue = 0)
        {
            NumberFormatInfo curFormatInfo = null;
            if (s.Contains(","))
            {
                curFormatInfo = CommaFormatInfo;
            }
            else
            {
                curFormatInfo = DotFormatInfo;
            }

            float num;
            if (Single.TryParse(s, NumberStyles.Float, curFormatInfo, out num))
            {
                return num;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime ParseDateTimeFromString(string date_string, string formatDateString)
        {
            DateTime date = DateTime.MinValue;
            bool can_parse_date = DateTime.TryParseExact(date_string, formatDateString, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date);
            if (!can_parse_date)
            {
                UnityEngine.Debug.LogError("Cant parse date: " + date_string);
            }

            return date;
        }

        public static DateTime GetTimeOfDayFromString(string time_str)
        {
            DateTime time = DateTime.Today;
            string[] time_arr = time_str.Split(':');
            float hours = 0;
            float minutes = 0;
            float seconds = 0;
            if (time_arr.Length > 0)
            {
                hours = Single.Parse(time_arr[0]);
                if (time_arr.Length > 1)
                {
                    minutes = Single.Parse(time_arr[1]);
                    if (time_arr.Length > 2)
                    {
                        seconds = Single.Parse(time_arr[2]);
                    }
                }
            }

            time = time.AddHours(hours);
            time = time.AddMinutes(minutes);
            time = time.AddSeconds(seconds);
            return time;
        }

        public static string StringToUtf8String(string string_to_encode)
        {
            byte[] str_bytes = Encoding.Default.GetBytes(string_to_encode);
            string string_utf8 = Encoding.UTF8.GetString(str_bytes);
            return string_utf8;
        }

        public static int[] IntListFromString(string str)
        {
            string[] str_int = str.Replace(" ", "").Split(',');
            int[] ints = new int[str_int.Length];
            for (int i = 0; i < str_int.Length; i++)
            {
                Int32.TryParse(str_int[i], out ints[i]);
            }

            return ints;
        }

        public static string IntListToString(List<int> int_list)
        {
            return ListToString(int_list, ",");
        }

        public static string IntArrayToString(int[] int_list)
        {
            string list_string = "";
            for (int i = 0; i < int_list.Length; i++)
            {
                string separator = (i != int_list.Length - 1) ? ", " : "";
                list_string += int_list[i] + separator;
            }

            return list_string;
        }

        public static string StringListToString(List<string> string_list)
        {
            return StringListToString(string_list, ",");
        }

        public static string StringListToString(List<string> string_list, string separator)
        {
            return ListToString(string_list, separator);
        }

        public static string ListToString<T>(List<T> obj_list, string separator)
        {
            string combined_string = "";
            for (int i = 0; i < obj_list.Count; i++)
            {
                string curSeparator = (i != obj_list.Count - 1) ? separator : "";
                combined_string += obj_list[i] + curSeparator;
            }

            return combined_string;
        }

        public static List<int> StringToIntList(string str, char divider = ',')
        {
            str = str.Replace(" ", "");
            if (str == "") return new List<int>();

            List<string> elements = new List<string>(str.Split(divider));
            elements = elements.FindAll(x => x != "");
            List<int> res = new List<int>();

            int temp = -1;
            for (int i = 0; i < elements.Count; i++)
            {
                if (Int32.TryParse(elements[i], out temp))
                {
                    res.Add(temp);
                }
                else
                {
                    //UnityEngine.Debug.Log("Wrong '" + elements[i] + "' in " + str);
                }
            }

            return res;
        }

        public static Dictionary<int, int> StringToIntIntDictionary(string str, char pairs_divider = ',', char key_value_devider = ':')
        {
            Dictionary<int, int> toReturn = new Dictionary<int, int>();
            var streakPairs = new List<string>(str.Split(pairs_divider));
            foreach (string strinckPair in streakPairs)
            {
                if (!String.IsNullOrEmpty(strinckPair))
                {
                    var values = new List<string>(strinckPair.Split(key_value_devider));
                    int key = SafeParseInt(values[0]);
                    int val = SafeParseInt(values[1]);
                    if (key != 0 || val != 0)
                        toReturn.Add(key, val);
                    else
                    {
                        throw new Exception("Can't parse to int int key val  " + strinckPair);
                    }
                }
            }

            return toReturn;
        }

        public static Vector2 StringToVector2(string str)
        {
            str = str.Replace(" ", "");
            if (str == "") return new Vector2();

            List<string> elements = new List<string>(str.Split(','));
            elements = elements.FindAll(val => val != "");
            Vector2 res = new Vector2();

            int x = -1;
            int y = -1;
            if (Int32.TryParse(elements[0], out x) && Int32.TryParse(elements[1], out y))
            {
                res.x = x;
                res.y = y;
            }
            else
            {
                //Debug.Log("Wrong '" + elements[i] + "' in " + str);
            }

            return res;
        }

        public static List<float> StringToFloatList(string str, char divider = ';')
        {
            str = str.Replace(" ", "");
            if (str == "") return new List<float>();

            List<string> elements = new List<string>(str.Split(divider));
            elements = elements.FindAll(x => x != "");
            List<float> res = new List<float>();
            for (int i = 0; i < elements.Count; i++)
            {
                float temp = -1;
                temp = float.Parse(elements[i], CultureInfo.InvariantCulture);
                res.Add(temp);
            }

            return res;
        }

        public static List<string> StringToStringList(string str, char separator = ',')
        {
            str = str.Replace(separator + " ", separator.ToString());
            str = str.Replace(" " + separator, separator.ToString());
            if (str == "") return new List<string>();
            List<string> elements = new List<string>(str.Split(separator));
            return elements;
        }

        public static float[] FloatArayFromString(string str, int expecterd_length = 0)
        {
            string orig_str = str;
            str = str.Replace("[", "");
            str = str.Replace("[", "");
            str = str.Replace(" ", "");
            string[] string_arr = str.Split(',');
            int array_length = (expecterd_length > 0) ? expecterd_length : string_arr.Length;
            float[] float_arr = new float[array_length];

            if (expecterd_length != string_arr.Length)
            {
                UnityEngine.Debug.LogError("Cant parse string: " + str + ", expecterd_length: " + expecterd_length);
            }
            else
            {
                for (int i = 0; i < array_length; i++)
                {
                    float cur_float = -1.0f;
                    bool right_val = Single.TryParse(string_arr[i], out cur_float);
                    if (!right_val)
                    {
                        UnityEngine.Debug.LogError("Cant parse string: " + orig_str + " to float[]");
                    }

                    float_arr[i] = cur_float;
                }
            }

            return float_arr;
        }

        public static Color ColorByString(string color_str, float? alpha = null)
        {
            Color color = Color.white;

            color = BaseColorByString(color_str, alpha);

            /*
        if (Constants.CONSTANTS_COLORS.ContainsKey(color_str)){
            color = Constants.CONSTANTS_COLORS[color_str];
        }
        else{
            color = BaseColorByString(color_str, alpha);
        }
        */
            color.a = alpha.HasValue ? alpha.Value : 1.0f;
            return color;
        }

        public static Color BaseColorByString(string color_str, float? alpha = null)
        {
            Color color = Color.white;

            switch (color_str)
            {
                case "black":
                    color = Color.black;
                    break;
                case "blue":
                    color = Color.blue;
                    break;
                case "cyan":
                    color = Color.cyan;
                    break;
                case "gray":
                    color = Color.gray;
                    break;

                case "green":
                    color = Color.green;
                    break;
                case "grey":
                    color = Color.grey;
                    break;
                case "magenta":
                    color = Color.magenta;
                    break;

                case "red":
                    color = Color.red;
                    break;
                case "white":
                    color = Color.white;
                    break;
                case "yellow":
                    color = Color.yellow;
                    break;
            }

            if (color_str.Contains("("))
            {
                string tempString = color_str.Substring(color_str.IndexOf("("),
                    color_str.IndexOf(",") - color_str.IndexOf("("));
                string r = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                tempString = color_str.Substring(color_str.IndexOf(",") + 1).Substring(0,
                    color_str.Substring(color_str.IndexOf(",") + 1).IndexOf(",") + 1);
                string g = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                tempString = color_str.Substring(color_str.IndexOf(",") + 1)
                    .Substring(color_str.Substring(color_str.IndexOf(",") + 1).IndexOf(",") + 1);
                string b = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                tempString = color_str.Substring(color_str.IndexOf(",") + 1)
                    .Substring(color_str.Substring(color_str.IndexOf(",") + 1).IndexOf(",") + 1);
                string a = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                color = new Color(Convert.ToInt32(r) / 255.0f, Convert.ToInt32(g) / 255.0f, Convert.ToInt32(b) / 255.0f,
                    Convert.ToInt32(a) / 255.0f);
            }

            if (alpha.HasValue)
            {
                color.a = alpha.Value;
            }

            return color;
        }

        public static Color ParseColor(string color_str)
        {
            color_str = color_str.Replace("(", "").Replace(")", "").Replace(" ", "");
            string[] compArr = color_str.Split(',');
            float r = SafeParseInt(compArr[0], 1) / 255.0f;
            float g = SafeParseInt(compArr[1], 1) / 255.0f;
            float b = SafeParseInt(compArr[2], 1) / 255.0f;
            float a = 1.0f;
            if (compArr.Length > 3)
            {
                a = SafeParseInt(compArr[3], 1) / 255.0f;
            }

            Color color = new Color(r, g, b, a);
            return color;
        }

        public static DayOfWeek ParseDayOfWeekFormString(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "mon": return DayOfWeek.Monday;
                case "tue": return DayOfWeek.Tuesday;
                case "wed": return DayOfWeek.Wednesday;
                case "thu": return DayOfWeek.Thursday;
                case "fri": return DayOfWeek.Friday;
                case "sat": return DayOfWeek.Saturday;
                case "sun": return DayOfWeek.Sunday;
            }

            return DayOfWeek.Monday;
        }

        public static List<DayOfWeek> ParseDaysOfWeekToArray(string days)
        {
            List<DayOfWeek> result = new List<DayOfWeek>();
            days = days.Replace(" ", "");
            string parseStr = days;
            string tempStr;
            while (parseStr != "")
            {
                if (parseStr.Substring(0, 1) == ",") parseStr = parseStr.Substring(1);

                if (parseStr.IndexOf(",") != -1)
                    tempStr = parseStr.Substring(0, parseStr.IndexOf(","));
                else
                    tempStr = parseStr;

                if (tempStr != "")
                {
                    result.Add(ParseDayOfWeekFormString(tempStr));
                    parseStr = parseStr.Replace(tempStr, "").Replace(",,", ",");
                }
            }

            return result;
        }
        
        public static int SafeParseInt(string row, int defaultValue = 0)
        {
            int value;
            return Int32.TryParse(row, out value) ? value : defaultValue;
        }

        public static Color ParseHEXColor(string colorString)
        {
            if (!colorString.StartsWith("#"))
                colorString = "#" + colorString;
            Color tapeColor = Color.white;
            ColorUtility.TryParseHtmlString(colorString, out tapeColor);
            return tapeColor;
        }
        public static List<float> FloatListFromString(string str)
        {
            List<string> strList = StringToStringList(str);
            List<float> floatList = strList.ConvertAll(s => ParseStringToFloat(s));
            return floatList;
        }
        
        public static Vector3 Vector3ByString(string vector_str)
        {
            vector_str = vector_str.Replace(" ", "");
            Vector3 vector = Vector3.zero;
            if (vector_str.Contains("("))
            {
                string tempString = vector_str.Substring(vector_str.IndexOf("("),
                    vector_str.IndexOf(",") - vector_str.IndexOf("("));
                string x = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                tempString = vector_str.Substring(vector_str.IndexOf(",") + 1).Substring(0,
                    vector_str.Substring(vector_str.IndexOf(",") + 1).IndexOf(",") + 1);
                string y = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                tempString = vector_str.Substring(vector_str.IndexOf(",") + 1)
                    .Substring(vector_str.Substring(vector_str.IndexOf(",") + 1).IndexOf(",") + 1);
                string z = tempString.Replace(",", "").Replace(")", "").Replace(" ", "").Replace("(", "");

                vector = new Vector3(ParseStringToFloat(x), ParseStringToFloat(y), ParseStringToFloat(z));
            }

            return vector;
        }
    }
}