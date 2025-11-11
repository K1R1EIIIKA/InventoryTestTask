using _Scripts.Configs;
using _Scripts.InventoryLogic.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace _Scripts.InventoryLogic.Inventory
{
    public class DebugExtensions : MonoBehaviour
    {
        [Inject] private IInventoryUIController _inventoryUIController;
        [Inject] private IInventoryController _inventoryController;
        
        [SerializeField] private Button _fillButton;
        [SerializeField] private Button _clearButton;
        [SerializeField] private Button _sortButton;

        private void OnEnable()
        {
            _fillButton.onClick.AddListener(FillRandomItems);
            _clearButton.onClick.AddListener(ClearItems);
            _sortButton.onClick.AddListener(SortItems);
            
            FillRandomItems();
        }
        
        private void OnDisable()
        {
            _fillButton.onClick.RemoveListener(FillRandomItems);
            _clearButton.onClick.RemoveListener(ClearItems);
            _sortButton.onClick.RemoveListener(SortItems);
        }
        private void SortItems()
        {
            _inventoryController.SortInventory();
        }

        private void FillRandomItems()
        {
            for (int y = 0; y < _inventoryController.Height; y++)
            {
                for (int x = 0; x < _inventoryController.Width; x++)
                {
                    var slot = _inventoryUIController.Slots[x, y];

                    var item = GetRandomItem();
                    slot.SetItem(item, Random.Range(1, item.MaxStack + 1));
                }
            }
        }
        
        private void ClearItems()
        {
            for (int y = 0; y < _inventoryController.Height; y++)
            {
                for (int x = 0; x < _inventoryController.Width; x++)
                {
                    var slot = _inventoryUIController.Slots[x, y];
                    slot.ClearItem();
                }
            }
        }
        
        private ItemData GetRandomItem()
        {
            var list = _inventoryController.ItemsPool;
            return list[Random.Range(0, list.Length)];
        }
    }
}
