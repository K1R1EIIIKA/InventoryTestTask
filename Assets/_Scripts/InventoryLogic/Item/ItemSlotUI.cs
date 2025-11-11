using _Scripts.InventoryLogic.Crafting;
using _Scripts.InventoryLogic.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.InventoryLogic.Item
{
    public class ItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Inject] private InventoryUIController _inventoryUIController;
        [Inject] private DragUIController _dragUIController;
        [Inject] private CraftingUIController _craftingUIController;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;

        public ItemSlot Slot => _itemSlot;
        
        private ItemSlot _itemSlot;

        public void Initialize(ItemSlot itemSlot)
        {
            _itemSlot = itemSlot;
            UpdateUI();
            _itemSlot.OnItemSlotChanged += UpdateUI;
        }
        
        private void UpdateUI()
        {
            if (_itemSlot.IsEmpty)
            {
                _icon.enabled = false;
                _countText.text = string.Empty;
            }
            else
            {
                _icon.enabled = true;
                _icon.sprite = _itemSlot.Item.Icon;
                _countText.text = _itemSlot.Item.Stackable ? _itemSlot.Count.ToString() : string.Empty;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_itemSlot.IsEmpty || _dragUIController.IsDragging) return;
            _inventoryUIController.ShowTooltip(_itemSlot);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_itemSlot.IsEmpty) return;
            _inventoryUIController.HideTooltip();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_itemSlot.IsEmpty || _dragUIController.IsDragging) return;
            
            if (!_inventoryUIController.IsTooltipVisible) _inventoryUIController.ShowTooltip(_itemSlot);
            _inventoryUIController.MoveTooltip();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Slot.Interactable) return;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _dragUIController.SplitStack(this);
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Slot.Interactable) return;
            
            if (Input.GetKey(KeyCode.LeftShift)) return;
            _dragUIController.BeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Slot.Interactable) return;
            
            _dragUIController.Drag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!Slot.Interactable) return;
            
            var target = eventData.pointerCurrentRaycast.gameObject
                ? eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlotUI>()
                : null;

            _dragUIController.EndDrag(target);
        }
    }
}
