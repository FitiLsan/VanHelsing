namespace BeastHunterHubUI
{
    public struct HubUITimeStruct
    {
        public int Day;
        public int Hour;


        public HubUITimeStruct(int day, int hour)
        {
            Day = day >= 0 ? day : 0;
            Hour = hour >= 0 ? hour : 0;
        }

        public override string ToString()
        {
            return $"day {Day}, hour {Hour}";
        }
    }
}
