using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField]
    protected GameObject inventoryUI;

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
            itemSlots[i].AddNewItem(emptyItem);

        for (int i = 0; i < equipmentSlots.Count; i++)
            equipmentSlots[i].AddNewItem(emptyItem);

        WeaponSlot.AddNewItem(emptyItem);
        HeadSlot.AddNewItem(emptyItem);
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
        if(InputManager.Instance.IsCloseUI)
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
            }
        }
       
        return false;
    }
    public void UseItem() => currentSelectedSlot.UseItem();

    public void Equip(EquipmentSlot slot)
    {
        Item item = slot.GetItem();

        if (item.cate == Item.Cate.None) return;

        EquippedSlot slotToEquip = null;

        if (item.cate == Item.Cate.Weapon) slotToEquip = WeaponSlot;
        if (item.cate == Item.Cate.Head) slotToEquip = HeadSlot;
        if (item.cate == Item.Cate.Body) slotToEquip = BodySlot;

        EquipToWith(slotToEquip, slot);
    }

    private void EquipToWith(EquippedSlot equippedSlot,EquipmentSlot slot)
    {
        //Unequip
        equippedSlot.UseItem();
        equippedSlot.Equip(slot);
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

    public int Quantity => quantity;
    public Item Item => item;

}