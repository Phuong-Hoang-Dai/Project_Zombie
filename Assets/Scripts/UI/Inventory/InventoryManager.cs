using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    public List<EquipmentSlot> equipmentSlots = new();
    public List<ItemSlot> itemSlots = new();
    public Item emptyItem;

    [SerializeField]
    protected List<InventoryItem> inventoryItems = new();
    [SerializeField]
    protected EquippedSlot weaponSlot;
    [SerializeField]
    protected EquippedSlot headSlot;
    [SerializeField]
    protected EquippedSlot bodySlot;

    public EquippedSlot GetWeaponSLot() => weaponSlot;
    public EquippedSlot GetHeadSlot() => headSlot;
    public EquippedSlot GetbodySlot() => bodySlot;

    [SerializeField]
    protected GameObject inventory;

    protected Slot currentSelectedSlot;
    protected InventoryCate currentInventoryCate;

    public Slot GetCurrentSelectedSlot() => currentSelectedSlot;
    public void UpdateCurrentSelectedSLot(Slot newSelectedSlot) => currentSelectedSlot = newSelectedSlot;

    public InventoryCate GetCurrentInventoryCate() => currentInventoryCate;
    public void UpdateCurrentInventoryCate(InventoryCate newInventoryCate) => currentInventoryCate = newInventoryCate;

    private void Awake()
    {
        inventory.SetActive(true);

        if (instance != null) Debug.LogError("Have more 1 Inventory Manager exists!!");
        instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < itemSlots.Count; i++)
            itemSlots[i].AddNewItem(emptyItem);

        for (int i = 0; i < equipmentSlots.Count; i++)
            equipmentSlots[i].AddNewItem(emptyItem);

        weaponSlot.AddNewItem(emptyItem);
        headSlot.AddNewItem(emptyItem);
        bodySlot.AddNewItem(emptyItem);

        foreach (var item in inventoryItems) AddItem(item.Item, item.Quantity);

        inventory.SetActive(false);
    }

    private void Update()
    {
        if(PlayerAssetsInputs.instance.IsOpenInventory())
        {
            inventory.SetActive(true);
            PlayerAssetsInputs.instance.DeactiveControlPlayer();
        }
        if(PlayerAssetsInputs.instance.IsCloseUI())
        {
            inventory.SetActive(false);
            PlayerAssetsInputs.instance.ActiveControlPlayer();
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
        if (item.isEquipable)
        {
            for (int i = 0; i < equipmentSlots.Count; i++)
            {
                if (equipmentSlots[i].GetItem() == emptyItem)
                {
                    equipmentSlots[i].AddNewItem(item);
                    equipmentSlots[i].AddQuantity(1);

                    return true;
                }
            }
        }
        else
        {
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].GetItem() == emptyItem)
                {
                    itemSlots[i].AddNewItem(item);
                    itemSlots[i].AddQuantity(quantity);

                    return true;
                }
            }
        }
        return false;
    }
    public void UseItem() => currentSelectedSlot.UseItem();

    public void Equip(EquipmentSlot slot)
    {
        Item item = slot.GetItem();

        if (item.cate == Item.Cate.Weapon)
        {
            weaponSlot.UseItem();
            weaponSlot.AddNewItem(item);
            weaponSlot.SetEquipmentSlot(slot);
            weaponSlot.SetQuantity(1);
        }
        if (item.cate == Item.Cate.Head)
        {
            headSlot.UseItem();
            headSlot.AddNewItem(item);
            headSlot.SetEquipmentSlot(slot);
            headSlot.SetQuantity(1);
        }
        if (item.cate == Item.Cate.Body)
        {
            bodySlot.UseItem();
            bodySlot.AddNewItem(item);
            bodySlot.SetEquipmentSlot(slot);
            bodySlot.SetQuantity(1);
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

    public int Quantity => quantity;
    public Item Item => item;

}