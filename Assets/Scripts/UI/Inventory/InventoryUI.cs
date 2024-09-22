using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : SelectLogicButtonGroup
{
    [SerializeField] protected Transform equipmentInventory;
    [SerializeField] protected Transform itemInventory;

    protected Slot lastSelectedSlot;
    protected Button firstButtonInIten;
    protected Button firstButtonInEquipment;
    protected GameObject firstSlotInIten;
    protected GameObject firstSlotInEquipment;
    protected InventoryManager.InventoryCate currentDisplayCate;

    protected override void  Start()
    {
        currentDisplayCate = InventoryManager.InventoryCate.None;
        List<Button> buttons;
        buttons = itemInventory.GetComponentsInChildren<Button>().ToList();
        if(buttons.Count > 0)
        {
            firstButtonInIten = buttons[0];
            firstSlotInIten = firstButtonInIten.gameObject;
        }
        buttons = equipmentInventory.GetComponentsInChildren<Button>().ToList();
        if (buttons.Count > 0)
        {
            firstButtonInEquipment = buttons[0];
            firstSlotInEquipment = firstButtonInEquipment.gameObject;
        }
    }

    protected override void Update()
    {

        if (currentDisplayCate != InventoryManager.instance.GetCurrentInventoryCate())
        {
            currentDisplayCate = InventoryManager.instance.GetCurrentInventoryCate();
            UpdateInventoryUI();
        }
        if(lastSelected != null && !HasAButtonSelectedByEventSystem())
        {
            if(InventoryManager.instance.GetCurrentSelectedSlot() != null)
                EventSystem.current.SetSelectedGameObject(
                    InventoryManager.instance.GetCurrentSelectedSlot().GetButton().gameObject);
            else 
                EventSystem.current.SetSelectedGameObject(lastSelected.GetButton().gameObject);
        }

        if (lastSelectedSlot != InventoryManager.instance.GetCurrentSelectedSlot())
            lastSelected.Deselect();
    }
    
    

    private void UpdateInventoryUI()
    {
        if (currentDisplayCate == InventoryManager.InventoryCate.Item)
        {
            equipmentInventory.gameObject.SetActive(false);
            itemInventory.gameObject.SetActive(true);

            if (firstButtonInIten != null)
            {
                firstButtonInIten.onClick.Invoke();
                EventSystem.current.SetSelectedGameObject(firstButtonInIten.gameObject);
            }
        }
        if(currentDisplayCate == InventoryManager.InventoryCate.Equipment)
        {
            equipmentInventory.gameObject.SetActive(true);
            itemInventory.gameObject.SetActive(false);

            if (firstButtonInEquipment != null)
            {
                Debug.Log("aaaa");
                firstButtonInEquipment.onClick.Invoke();
                EventSystem.current.SetSelectedGameObject(firstButtonInEquipment.gameObject);
            }
        }
    }

    public override void OnSelectButtonInGroup(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out Slot newSelectedSlot))
        {
            InventoryManager.instance.UpdateCurrentSelectedSLot(newSelectedSlot);
            lastSelectedSlot = newSelectedSlot;
        }
    }
}
