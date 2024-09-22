using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedSlot : Slot
{
    protected EquipmentSlot equipmentSlot;

    public EquipmentSlot GetEquipmentSlot() => equipmentSlot;
    public void SetEquipmentSlot(EquipmentSlot equipmentSlot) => this.equipmentSlot = equipmentSlot;

    protected override void Awake()
    {
        base.Awake();

        nameActionToUseItem = "Unequip";
    }

    public override void HideUI() { }

    public override void ShowUI() { }

    public override void UseItem()
    {
        UpdatePlayerStat(-itemData.changeAmout);
        AddNewItem(emptyItem);
        SetQuantity(0);
        equipmentSlot = null;
    }

    public override void OnDrop(PointerEventData eventData) 
    {
        if(eventData.pointerDrag.TryGetComponent(out EquipmentSlot oldItemSlot))
        {
            oldItemSlot.UseItem();
            button.onClick.Invoke();
        }
    }

    public void SetQuantity(int newQuantity) => quantity = newQuantity;
}
