using _Scripts.Configs;
using _Scripts.InventoryLogic.Interfaces;
using _Scripts.InventoryLogic.Inventory;
using _Scripts.InventoryLogic.Item;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.InventoryLogic.Crafting
{
    public class CraftingUIController : MonoBehaviour, ICraftingUIController
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private ICraftingController _craftingController;
        [Inject] private IInventoryController _inventoryController;

        [SerializeField] private CraftingResultUI _craftingResultUI;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private ItemSlotUI _slotPrefab;
        [SerializeField] private Button _craftButton;

        private ItemSlot[,] _slots;

        private void OnEnable()
        {
            _craftButton.onClick.AddListener(OnCraftButtonPressed);
        }

        private void OnDisable()
        {
            _craftButton.onClick.RemoveListener(OnCraftButtonPressed);
        }

        public void Initialize()
        {
            _slots = new ItemSlot[3, 3];
            _grid.constraintCount = 3;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    _slots[x, y] = new ItemSlot();

                    var ui = _diContainer.InstantiatePrefabForComponent<ItemSlotUI>(_slotPrefab, _grid.transform);
                    ui.Initialize(_slots[x, y]);

                    _slots[x, y].OnInventorySlotChanged += Recalculate;
                }
            }

            _craftingResultUI.Initialize();
            Recalculate();
        }

        private void Recalculate()
        {
            if (_craftingController.FindMatch(_slots, out RecipeData recipe, out var used))
            {
                _craftingResultUI.SetResult(recipe.Result, recipe.ResultCount);
            }
            else
            {
                _craftingResultUI.ClearResult();
            }
        }

        private void OnCraftButtonPressed()
        {
            if (_craftingController.FindMatch(_slots, out RecipeData recipe, out var used))
            {
                if (_inventoryController.TryAddItem(recipe.Result, recipe.ResultCount))
                {
                    _craftingController.PayCraft(used);

                    _craftingResultUI.ClearResult();

                    Recalculate();
                }
                else
                {
                    Debug.LogWarning("Inventory is full!");
                }
            }
        }
    }
}
