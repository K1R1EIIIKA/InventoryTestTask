using System;
using _Scripts.InventoryLogic.Inventory;
using _Scripts.InventoryLogic.Item;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.InventoryLogic.Crafting
{
    public class CraftingUIController : MonoBehaviour
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private CraftingController _craftingController;
        [Inject] private InventoryController _inventoryController;

        [SerializeField] private CraftingResultUI _craftingResultUI;
        
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private ItemSlotUI _slotPrefab;
        [SerializeField] private Button _craftButton;

        public ItemSlot[,] Slots { get; private set; }

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
            Slots = new ItemSlot[3, 3];

            _grid.constraintCount = 3;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Slots[x, y] = new ItemSlot();

                    var slotUI = _diContainer.InstantiatePrefabForComponent<ItemSlotUI>(_slotPrefab, _grid.transform);
                    slotUI.Initialize(Slots[x, y]);
                    
                    Slots[x, y].OnInventorySlotChanged += Recalculate;
                }
            }
            
            _craftingResultUI.Initialize();
            Recalculate();
        }
        
        private void Recalculate()
        {
            var recipe = _craftingController.FindMatch(Slots);

            if (recipe == null)
            {
                _craftingResultUI.ClearResult();
                return;
            }

            _craftingResultUI.SetResult(recipe.Result, recipe.ResultCount);
        }
        
        private void OnCraftButtonPressed()
        {
            var recipe = _craftingController.FindMatch(Slots);

            if (recipe == null)
            {
                Debug.Log("No matching recipe.");
                return;
            }

            // 1. Удаляем ресурсы
            if (!_inventoryController.TryAddItem(recipe.Result, recipe.ResultCount)) return;
            
            _craftingController.PayCraft(Slots, recipe);

            // 2. Добавляем рецепт в инвентарь

            // 3. Очищаем визуальный слот результата
            _craftingResultUI.ClearResult();

            // 4. Пересчёт — возможно после траты ресурсов появляется новый валидный рецепт
            Recalculate();
        }
    }
}
