namespace BeastHunterHubUI
{
    public class TimeService
    {
        private HubUIContext _context;


        public TimeService(HubUIContext context)
        {
            _context = context;
        }


        public GameTimeStruct CalculateTimeOnAddHours(int addedHours)
        {
            return _context.GameTime.AddTime(addedHours);
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

        public string GetFullPhraseAboutTravelTime(LocationModel location)
        {
            int travelTime = CountTravelTime(location);
            return $"Время похода: {travelTime} {GetHoursWord(travelTime)}";
        }

        public int CountTravelTime(LocationModel location)
        {
            //todo: count by design document
            //temporary for debug:
            return location.BaseTravelTime;
        }
    }
}
