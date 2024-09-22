using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedUI : SelectLogicButtonGroup
{
    protected Slot lastSelectedSlot;

    protected override void Start()
    {
        if (buttonsInGroup.Length > 0)
            lastSelected = buttonsInGroup[0];
    }

    protected override void Update()
    {
        if (lastSelectedSlot != InventoryManager.instance.GetCurrentSelectedSlot())
            lastSelected.Deselect();
    }

    public override void OnSelectButtonInGroup(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out EquippedSlot slot))
        {
            InventoryManager.instance.UpdateCurrentSelectedSLot(slot);
            lastSelectedSlot = slot;
        }
    }
}
