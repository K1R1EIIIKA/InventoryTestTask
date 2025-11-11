using _Scripts.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Item
{
    public class ItemSlotUI : MonoBehaviour
    {
        [Inject] private InventoryUIController _inventoryUIController;

        private ItemSlot _itemSlot;

        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _countText;

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
    }
}
