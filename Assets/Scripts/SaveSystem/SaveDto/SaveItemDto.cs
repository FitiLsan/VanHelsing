namespace SaveSystem.SaveDto
{
    public class SaveItemDto
    {
        public int Entry { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; }
        public int TimeLeft { get; set; }
        public bool ScriptUsed { get; set; }
        public int SpellCharges1 { get; set; }
        public int SpellCharges2 { get; set; }
        public int Durability { get; set; }
        public int Slot { get; set; }
    }
}