using System.Collections.Generic;
using System.Linq;
using _Scripts.Configs;
using _Scripts.InventoryLogic.Interfaces;
using UnityEngine;
using Zenject;

namespace _Scripts.InventoryLogic.Inventory
{
    public class InventoryController : IInventoryController
    {
        [Inject] private IInventoryUIController _inventoryUIController;
        [Inject] private InventoryConfig _inventoryConfig;
        
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ItemData[] ItemsPool => _inventoryConfig.ItemsPool;
        
        public void Initialize()
        {
            Width = _inventoryConfig.Width;
            Height = _inventoryConfig.Height;
            
            _inventoryUIController.Initialize(Width, Height);
        }
        
        public bool TryAddItem(ItemData item, int count)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var slot = _inventoryUIController.Slots[x, y];

                    if (slot.Item == item && item.Stackable)
                    {
                        count = slot.Add(count);
                        if (count <= 0) return true;
                    }
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var slot = _inventoryUIController.Slots[x, y];
            
                    if (slot.IsEmpty)
                    {
                        slot.SetItem(item, count);
                        return true;
                    }
                }
            }

            return false;
        }
        
        public void SortInventory()
        {
            List<(ItemData item, int count)> all = new();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var slot = _inventoryUIController.Slots[x, y];
                    if (!slot.IsEmpty)
                        all.Add((slot.Item, slot.Count));
                }
            }

            var merged = all
                .GroupBy(i => i.item)
                .Select(g => (item: g.Key, count: g.Sum(x => x.count)))
                .ToList();

            merged = merged
                .OrderBy(m => m.item.ID)     
                .ToList();

            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _inventoryUIController.Slots[x, y].ClearItem();

            int index = 0;

            foreach (var entry in merged)
            {
                int remaining = entry.count;

                while (remaining > 0)
                {
                    int add = Mathf.Min(remaining, entry.item.MaxStack);

                    int x = index % Width;
                    int y = index / Width;

                    _inventoryUIController.Slots[x, y].SetItem(entry.item, add);

                    remaining -= add;
                    index++;
                }
            }
        }

    }
}
