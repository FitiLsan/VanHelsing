namespace Settings
{
    public class DayTimeSettings
    {
        public DayTimeSettings(int morningLength, int dayLength, int eveningLength, int nightLength)
        {
            MorningLength = morningLength;
            DayLength = dayLength;
            EveningLength = eveningLength;
            NightLength = nightLength;
        }

        public DayTimeSettings()
        {
            MorningLength = 600;
            DayLength = 600;
            EveningLength = 600;
            NightLength = 600;
        }

        public int MorningLength { get; }
        public int DayLength { get; }
        public int EveningLength { get; }
        public int NightLength { get; }
        public int FullLength => MorningLength + DayLength + EveningLength + NightLength;
    }
}