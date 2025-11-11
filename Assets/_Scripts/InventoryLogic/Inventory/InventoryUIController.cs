using _Scripts.Extensions;
using _Scripts.InventoryLogic.Item;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.InventoryLogic.Inventory
{
    public class InventoryUIController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        
        [SerializeField] private GridLayoutGroup _container;
        [SerializeField] private ItemSlotUI _itemSlotUIPrefab;
        [SerializeField] private ItemTooltipUI _tooltip;
            
        public ItemSlot[,] Slots;
        public bool IsTooltipVisible => _tooltip.gameObject.activeSelf;
        
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
                    
                    var itemSlotUI = _diContainer.InstantiatePrefabForComponent<ItemSlotUI>(_itemSlotUIPrefab, _container.transform);
                    itemSlotUI.Initialize(Slots[x, y]);
                }
            }
        }
        
        public void ShowTooltip(ItemSlot slot)
        {
            _tooltip.Show(slot);
        }

        public void HideTooltip()
        {
            _tooltip.Hide();
        }

        public void MoveTooltip()
        {
            var offset = _tooltip.GetComponent<RectTransform>().sizeDelta / 2;
            _tooltip.transform.position = Input.mousePosition + (Vector3)offset;
        }
    }
}
