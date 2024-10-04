using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>, IDataPersistence
{
    [SerializeField]
    protected GameObject store;

    [SerializeField]
    protected List<InventoryItem> items = new();
    [SerializeField]
    protected List<ProductSlotInStore> productSlots = new();

    [SerializeField]
    public Item emptyItem;

    public Slot currentSelectedSlot;
    public int amountToBuy;

    protected override void Awake()
    {
        base.Awake();

        store.SetActive(true);
    }

    private void Start()
    {
        //for (int i = 0; i < productSlots.Count; i++)
        //    if (productSlots[i].GetItem() != null)
        //        productSlots[i].AddNewItem(emptyItem);

        if(LevelManager.Instance.Day == 0 && 
            LevelManager.Instance.TimeOfDay == TimeOfDay.Morning)
        {
            foreach (var item in items)
                AddItem(item.Item, item.Quantity);
        }

        store.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.IsCloseUI)
        {
            store.SetActive(false);
            InputManager.Instance.ActiveControlPlayer();
        }

        CleanInventory();
    }

    public void OpenStore()
    {
        store.SetActive(true);
        InputManager.Instance.DeactiveControlPlayer();
    }

    private void CleanInventory()
    {
        for (int i = 0; i < productSlots.Count; i++)
            if (productSlots[i].GetItemAmount() <= 0)
                productSlots[i].AddNewItem(emptyItem);
    }

    public bool AddItem(Item item, int quantity)
    {
        for (int i = 0; i < productSlots.Count; i++)
        {
            if (productSlots[i].GetItem() == emptyItem)
            {
                productSlots[i].AddNewItem(item);
                productSlots[i].AddQuantity(quantity);

                return true;
            }
        }
        return false;
    }
    public void Buy()
    {
        if (currentSelectedSlot.GetItem().price > InventoryManager.Instance.Coin)
            return;

        InventoryManager.Instance.UpdateCoin(Mathf.RoundToInt(-currentSelectedSlot.GetItem().price * amountToBuy));
        InventoryManager.Instance.AddItem(currentSelectedSlot.GetItem(), amountToBuy);
        currentSelectedSlot.RemoveQuantity(amountToBuy);
    }

    public void SaveData(GameData gameData)
    {
        gameData.storeItems = new();

        for (int i = 0; i < productSlots.Count; i++)
        {
            if (productSlots[i].GetItem() != emptyItem)
            {
                gameData.storeItems.Add(new InventoryItem(productSlots[i].GetItemAmount()
                    , productSlots[i].GetItem()));
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        for (int i = 0; i < gameData.storeItems.Count; i++)
        {
            AddItem(gameData.storeItems[i].Item, gameData.storeItems[i].Quantity);
        }
    }
}
