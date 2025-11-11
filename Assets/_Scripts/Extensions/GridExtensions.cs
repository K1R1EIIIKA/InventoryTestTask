using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Extensions
{
    public static class GridExtensions
    {
        public static void ResizeContainer(this GridLayoutGroup layoutGroup, int width, int height)
        {
            var rt = layoutGroup.GetComponent<RectTransform>();
            var cell = layoutGroup.cellSize;
            var spacing = layoutGroup.spacing;
            var padding = layoutGroup.padding;

            var totalWidth =
                padding.left +
                padding.right +
                width * cell.x +
                (width - 1) * spacing.x;

            var totalHeight =
                padding.top +
                padding.bottom +
                height * cell.y +
                (height - 1) * spacing.y;

            rt.sizeDelta = new Vector2(totalWidth, totalHeight);
        }
    }
}
