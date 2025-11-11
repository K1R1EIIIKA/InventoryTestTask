using System;
using _Scripts.Configs;
using UnityEngine;

namespace _Scripts.InventoryLogic.Item
{
    [Serializable]
    public class ItemSlot
    {
        public ItemData Item;
        public int Count;

        public bool IsEmpty => Item == null;
        public bool Interactable => _interactable;
        public Action OnItemSlotChanged;
        public Action OnInventorySlotChanged;
        
        private bool _interactable = true;

        public bool CanStack(ItemData item) =>
            !IsEmpty && Item == item && Item.Stackable;

        public void SetItem(ItemData item, int count)
        {
            Item = item;
            Count = count;
            OnItemSlotChanged?.Invoke();
            OnInventorySlotChanged?.Invoke();
        }

        public int Add(int amount)
        {
            if (!Item.Stackable) return amount;

            int capacity = Item.MaxStack - Count;
            int moved = Mathf.Min(capacity, amount);

            Count += moved;

            OnItemSlotChanged?.Invoke();
            OnInventorySlotChanged?.Invoke();

            return amount - moved;
        }

        public int Remove(int amount)
        {
            if (IsEmpty) return amount;

            int removed = Mathf.Min(Count, amount);
            Count -= removed;
            
            if (Count == 0)
                Item = null;

            OnItemSlotChanged?.Invoke();
            OnInventorySlotChanged?.Invoke();

            return amount - removed;
        }
        
        public void ClearItem()
        {
            Item = null;
            Count = 0;
            OnItemSlotChanged?.Invoke();
            OnInventorySlotChanged?.Invoke();   
        }
        
        public void SetCraftingItem(ItemData item, int count)
        {
            Item = item;
            Count = count;
            OnItemSlotChanged?.Invoke();
            _interactable = false;
        }
    }
}
