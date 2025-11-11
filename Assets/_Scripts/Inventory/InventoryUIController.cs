using _Scripts.Extensions;
using _Scripts.Item;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _container;
        [SerializeField] private ItemSlotUI _itemSlotUIPrefab;
            
        public ItemSlot[,] Slots;
        
        public void Initialize(int width, int height)
        {
            Slots = new ItemSlot[width, height];
            _container.constraintCount = width;
            _container.ResizeContainer(width, height);
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Slots[x, y] = new ItemSlot();
                    
                    var itemSlotUI = Instantiate(_itemSlotUIPrefab, _container.transform);
                    itemSlotUI.Initialize(Slots[x, y]);
                }
            }
        }
    }
}
