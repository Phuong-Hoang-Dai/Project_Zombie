using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedSlot : Slot
{
    public EquipmentSlot EquipmentSlot {  get; private set; }

    protected override void Awake()
    {
        base.Awake();

        NameActionToUseItem = "Unequip";
    }

    public override void HideUI() { }

    public override void ShowUI() { }

    public override void UseItem()
    {
        if(itemData != null ) UpdatePlayerStat(-itemData.changeAmout);

        AddNewItem(emptyItem);
        PlayerController.Instance.Equip(itemData);

        EquipmentSlot = null;
        quantity = 0;
    }

    public void Equip(EquipmentSlot slot)
    {
        AddNewItem(slot.GetItem());

        UpdatePlayerStat(itemData.changeAmout);

        PlayerController.Instance.Equip(slot.GetItem());

        EquipmentSlot = slot;
        quantity = 1;
    }

    public override void OnDrop(PointerEventData eventData) 
    {
        if(eventData.pointerDrag.TryGetComponent(out EquipmentSlot oldItemSlot))
        {
            oldItemSlot.UseItem();
            button.onClick.Invoke();
        }
    }
}
