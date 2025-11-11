using _Scripts.Configs;
using _Scripts.Item;
using UnityEngine;
using Zenject;

namespace _Scripts.Inventory
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
        

    }
}
