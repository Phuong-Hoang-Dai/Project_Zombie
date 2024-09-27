using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : SelectLogicButtonGroup
{
    protected Button[] buttons;
    protected override void Awake()
    {
        base.Awake();

        buttons = GetComponentsInChildren<Button>();
    }
    protected override void Update()
    {
        base.Update();
        int moveDirection = InputManager.Instance.ControlMenu;
        if (moveDirection != 0 && buttonsInGroup.Length > 1)
        {
            if(lastSelected.index + moveDirection >= 0 
                && lastSelected.index + moveDirection < buttonsInGroup.Length)
            {
                buttons[lastSelected.index + moveDirection].onClick.Invoke();
            }
        }
    }

    public override void OnSelectButtonInGroup(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out ButtonLogic buttonLogic))
        {
            if (buttonLogic.index == 0)
                InventoryManager.Instance.currentInventoryCate = InventoryManager.InventoryCate.Equipment;
            if (buttonLogic.index == 1)
                InventoryManager.Instance.currentInventoryCate = InventoryManager.InventoryCate.Item;
        }
    }
}
