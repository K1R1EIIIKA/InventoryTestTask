# Тестовое задание Unity: "Система инвентаря и крафта"

### Цель

Создать функциональную систему инвентаря с drag-and-drop интерфейсом и базовым крафтингом предметов.

---

![image](https://github.com/K1R1EIIIKA/InventoryTestTask/blob/main/Images/Full.png?raw=true)

---
## Стек
- Unity 6.0
- Zenject (DI)
- Alchemy (для ускорения работы с иерархией)

---

## Архитектура
- DI (Zenject)
- MVC для UI
- Scriptable Objects для хранения данных о предметах и рецептах крафта
- Кастомный EditorWindow для удобного создания и редактирования рецептов крафта

## Функционал
- Инициализация инвентаря любого размера (настройка в конфиге)
- Drag-and-Drop интерфейс для управления предметами в инвентаре
    - Перемещение предметов между слотами
    - При зажатии Shift - разделение стака
    - Автостак при переносе одинаковых предметов
    - Удаление предметов из инвентаря
- Система крафта
    - Бесформенные рецепты: предметы определенного количества на любых слотах
    ![image](https://github.com/K1R1EIIIKA/InventoryTestTask/blob/main/Images/Shapeless.png?raw=true)
    - Форменные рецепты: предметы в определенном порядке на определенных слотах
    ![image](https://github.com/K1R1EIIIKA/InventoryTestTask/blob/main/Images/Shaped.png?raw=true)
    ![image](https://github.com/K1R1EIIIKA/InventoryTestTask/blob/main/Images/Axe.png?raw=true)