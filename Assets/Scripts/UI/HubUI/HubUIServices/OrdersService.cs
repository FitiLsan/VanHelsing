namespace BeastHunterHubUI
{
    public class OrdersService
    {
        private HubUIContext _context;


        public OrdersService(HubUIContext context)
        {
            _context = context;
        }


        public HubUITimeStruct CalculateOrderCompletionTime(CharacterModel character, OrderModel order)
        {
            HubUITimeStruct orderCompletionTime = _context.GameTime.AddTime(order.BaseSpentHours);
            //
            //calculate logic
            //
            return orderCompletionTime;
        }
    }
}
