using System.Collections.Generic;
using UnityEngine;

public class StoreManager : Singleton<StoreManager>
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
        for (int i = 0; i < productSlots.Count; i++)
            productSlots[i].AddNewItem(emptyItem);

        foreach (var item in items) 
            AddItem(item.Item, item.Quantity);

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
        InventoryManager.Instance.AddItem(currentSelectedSlot.GetItem(), amountToBuy);
        currentSelectedSlot.RemoveQuantity(amountToBuy);
    }
}
