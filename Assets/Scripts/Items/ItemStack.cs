using System;
using UnityEngine;

namespace Items
{
    [Serializable]
    public struct ItemStack
    {
        [SerializeField] private Item item;
        
        public Item Item => item;
        public int amount;
        
        public ItemStack(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }
}