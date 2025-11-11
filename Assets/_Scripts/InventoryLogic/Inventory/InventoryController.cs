using System.Collections.Generic;
using System.Linq;
using _Scripts.Configs;
using UnityEngine;
using Zenject;

namespace _Scripts.InventoryLogic.Inventory
{
    public class InventoryController
    {
        [Inject] private InventoryUIController _inventoryUIController;
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
            // 1. Сначала пытаемся добавить в существующие стаки
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

            // 2. Если остались предметы — ищем пустой слот
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

            // 3. Если нет места
            UnityEngine.Debug.LogWarning("Inventory is full! Item was not added.");
            return false;
        }
        
        public void SortInventory()
        {
            // 1. Собрать все предметы в список (только занятые)
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

            // 2. Объединить одинаковые предметы
            var merged = all
                .GroupBy(i => i.item)
                .Select(g => (item: g.Key, count: g.Sum(x => x.count)))
                .ToList();

            // 3. Сортировать по ID или порядку в ItemsPool
            merged = merged
                .OrderBy(m => m.item.ID)     // ✅ сортировка по ID
                .ToList();

            // 4. Полностью очистить инвентарь
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _inventoryUIController.Slots[x, y].ClearItem();

            // 5. Записать заново СЛЕВА НАПРАВО БЕЗ ДЫР
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
