using System;
using Items;

namespace Events.Args
{
    public class ItemArgs : EventArgs
    {
        public ItemArgs(Item item)
        {
            Item = item;
        }

        public Item Item { get; }
    }
}