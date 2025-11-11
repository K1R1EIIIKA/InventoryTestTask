using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Item
{
    public class DragItemUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show(Sprite sprite)
        {
            _icon.sprite = sprite;
            _canvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
        }

        private void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}
