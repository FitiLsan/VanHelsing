namespace BeastHunterHubUI
{
    public class TravelTimeService
    {
        #region Methods

        public string GetFullPhraseAboutTravelTime(LocationModel location)
        {
            int travelTime = CountTravelTime(location);
            return $"Время похода: {travelTime} {GetHoursWord(travelTime)}";
        }

        public int CountTravelTime(LocationModel location)
        {
            return location.TravelTime;
        }

        public string GetHoursWord(int number)
        {
            if (number > 9)
            {
                number = (int)(((float)number / 10 - number / 10) * 10);
            }

            switch (number)
            {
                case 1:
                    return "час";
                case 2:
                case 3:
                case 4:
                    return "часа";
                default:
                    return "часов";
            }
        }

        #endregion
    }
}
