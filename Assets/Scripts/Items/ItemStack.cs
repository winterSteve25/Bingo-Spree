using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public struct ItemStack
    {
        [SerializeField] private Item item;
        [SerializeField] private int amount;
        
        public Item Item => item;
        public int Amount => amount;
        
        public ItemStack(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}