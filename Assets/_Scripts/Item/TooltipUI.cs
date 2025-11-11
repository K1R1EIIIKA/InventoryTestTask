using TMPro;
using UnityEngine;

namespace _Scripts.Item
{
    public class ItemTooltipUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _countText;

        public void Show(ItemSlot slot)
        {
            _nameText.text = slot.Item.ItemName;
            _descriptionText.text = slot.Item.Description;
        
            if (slot.Item.Stackable)
            {
                _countText.gameObject.SetActive(true);
                _countText.text = $"x{slot.Count}";
            }
            else
            {
                _countText.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);

            transform.position = Input.mousePosition;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
