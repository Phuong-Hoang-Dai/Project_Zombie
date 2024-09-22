using UnityEngine;
using UnityEngine.EventSystems;

public class StoreUI : SelectLogicButtonGroup
{
    protected override void Update()
    {
        if(lastSelected != null && !HasAButtonSelectedByEventSystem())
            EventSystem.current.SetSelectedGameObject(lastSelected.GetButton().gameObject);
    }

    public override void OnSelectButtonInGroup(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out Slot newSelectedSlot))
            StoreManager.instance.UpdateCurrentSelectedSLot(newSelectedSlot);
    }
}
