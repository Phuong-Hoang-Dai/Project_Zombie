using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance {  get; private set; }

    [SerializeField]
    public List<InventoryItem> items = new();
    public List<ProductSlotInStore> productSlots = new();

    [SerializeField]
    public Item emptyItem;

    protected Slot currentSelectedSlot;

    [SerializeField]
    protected GameObject store;

    protected int amountToBuy;

    public Slot GetCurrentSelectedSlot() => currentSelectedSlot;
    public void UpdateCurrentSelectedSLot(Slot newSelectedSlot) => currentSelectedSlot = newSelectedSlot;

    public int GetCurrentAmounToBuy() => amountToBuy;
    public void UpdateCurrentAmountToBuy(int newAmount) => amountToBuy = newAmount;

    private void Awake()
    {
        store.SetActive(true);

        if (instance != null) Debug.LogError("Have more 1 Store Manager exists!!");
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < productSlots.Count; i++)
            productSlots[i].AddNewItem(emptyItem);

        foreach (var item in items) AddItem(item.Item, item.Quantity);

        store.SetActive(false);
    }

    private void Update()
    {
        if (PlayerAssetsInputs.instance.IsCloseUI())
        {
            store.SetActive(false);
            PlayerAssetsInputs.instance.ActiveControlPlayer();
        }

        CleanInventory();
    }

    public void OpenStore()
    {
        store.SetActive(true);
        PlayerAssetsInputs.instance.DeactiveControlPlayer();
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
        InventoryManager.instance.AddItem(currentSelectedSlot.GetItem(), amountToBuy);
        currentSelectedSlot.RemoveQuantity(amountToBuy);
    }
}
