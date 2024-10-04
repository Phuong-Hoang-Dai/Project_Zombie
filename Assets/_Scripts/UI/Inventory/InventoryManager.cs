using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : Singleton<InventoryManager>, IDataPersistence
{
    [SerializeField]
    protected GameObject inventoryUI;

    public int Coin {  get; private set; }

    [SerializeField]
    protected List<InventoryItem> inventoryItems = new();
    [SerializeField]
    protected List<ItemSlot> itemSlots = new();
    [SerializeField]
    protected List<EquipmentSlot> equipmentSlots = new();
    [field: SerializeField]
    public EquippedSlot WeaponSlot { get; private set; }
    [field: SerializeField]
    public EquippedSlot HeadSlot { get; private set; }
    [field: SerializeField]
    public EquippedSlot BodySlot { get; private set; }

    public Item emptyItem;

    public Slot currentSelectedSlot;
    public InventoryCate currentInventoryCate;

    protected override void Awake()
    {
        base.Awake();

        inventoryUI.SetActive(true);
    }

    private void Start()
    {
        for(int i = 0; i < itemSlots.Count; i++)
            if(itemSlots[i].GetItem() == null)
                itemSlots[i].AddNewItem(emptyItem);

        for (int i = 0; i < equipmentSlots.Count; i++)
            if(equipmentSlots[i].GetItem() == null)
                equipmentSlots[i].AddNewItem(emptyItem);

        if (WeaponSlot.GetItem() == null)
            WeaponSlot.AddNewItem(emptyItem);
        if (HeadSlot.GetItem() == null)
            HeadSlot.AddNewItem(emptyItem);
        if (BodySlot.GetItem() == null)
            BodySlot.AddNewItem(emptyItem);

        foreach (var item in inventoryItems) AddItem(item.Item, item.Quantity);

        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if(InputManager.Instance.IsInventoryOpen)
        {
            inventoryUI.SetActive(true);
            InputManager.Instance.DeactiveControlPlayer();
        }
        else if(InputManager.Instance.IsCloseUI)
        {
            inventoryUI.SetActive(false);
            InputManager.Instance.ActiveControlPlayer();
        }

        CleanInventory();
    }

    private void CleanInventory()
    {
        for (int i = 0; i < itemSlots.Count; i++)
            if (itemSlots[i].GetItemAmount() <= 0)
                itemSlots[i].AddNewItem(emptyItem);

        for (int i = 0; i < equipmentSlots.Count; i++)
            if (equipmentSlots[i].GetItemAmount() <= 0)
                equipmentSlots[i].AddNewItem(emptyItem);
    }

    public bool AddItem(Item item, int quantity)
    {
        if (item == emptyItem) return false;

        List<Slot> slotToAdd = item.isEquipable ? new(equipmentSlots) : new(itemSlots);
        int quantityToAdd = item.isEquipable ? 1 : quantity;

        for (int i = 0; i < slotToAdd.Count; i++)
        {
            if (slotToAdd[i].GetItem() == emptyItem)
            {
                slotToAdd[i].AddNewItem(item);
                slotToAdd[i].AddQuantity(quantityToAdd);

                return true;
            }else if (slotToAdd[i].GetItem() == item && !item.isEquipable)
            {
                slotToAdd[i].AddQuantity(quantityToAdd);

                return true;
            }
        }
       
        return false;
    }
    public void UpdateCoin(int amout) => Coin += amout;

    public void UseItem() => currentSelectedSlot.UseItem();

    public void Equip(EquipmentSlot slot)
    {
        Item item = slot.GetItem();
        if (item == emptyItem) return;

        EquippedSlot slotToEquip = null;

        if (item.cate == Item.Cate.Weapon) slotToEquip = WeaponSlot;
        if (item.cate == Item.Cate.Head) slotToEquip = HeadSlot;
        if (item.cate == Item.Cate.Body) slotToEquip = BodySlot;

        EquipToWith(slotToEquip, slot);
    }

    private void EquipToWith(EquippedSlot equippedSlot,EquipmentSlot slot)
    {
        equippedSlot.UseItem();

        equippedSlot.Equip(slot);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveData(GameData gameData)
    {
        gameData.coin = Coin;

        gameData.inventoryItems = new();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].GetItem() != emptyItem)
            {
                gameData.inventoryItems.
                    Add(new InventoryItem(itemSlots[i].GetItemAmount(), itemSlots[i].GetItem()));
            }
        }

        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            if (equipmentSlots[i].GetItem() != emptyItem)
            {
                gameData.inventoryItems.
                Add(new InventoryItem(equipmentSlots[i].GetItemAmount(), equipmentSlots[i].GetItem()));
            }
        }

        gameData.weaponSlot = new InventoryItem(WeaponSlot.GetItemAmount(), WeaponSlot.GetItem());
    }

    public void LoadData(GameData gameData)
    {
        Coin = gameData.coin;

        for (int i = 0; i < gameData.inventoryItems.Count; i++)
            AddItem(gameData.inventoryItems[i].Item, gameData.inventoryItems[i].Quantity);

        for (int i = 0; i < equipmentSlots.Count; i++)
            if(gameData.weaponSlot != null)
                if (equipmentSlots[i].GetItem() == gameData.weaponSlot.Item)
                {
                    equipmentSlots[i].UseItem();
                    PlayerController.Instance.UpdateStats(gameData.weaponSlot.Item.statsToChange,
                        -gameData.weaponSlot.Item.changeAmout);
                }
    }

    public enum InventoryCate
    {
        None,
        Item,
        Equipment
    }
}

[Serializable]
public class InventoryItem
{
    [SerializeField]
    private int quantity;
    [SerializeField]
    private Item item;

    public InventoryItem(int quantity, Item item)
    {
        this.quantity = quantity;
        this.item = item;
    }

    public int Quantity => quantity;
    public Item Item => item;

}