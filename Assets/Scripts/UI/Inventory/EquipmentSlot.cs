using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : Slot
{
    protected bool isEquipped;
    [SerializeField]
    protected TMP_Text equippedDisplayText;

    protected override void Awake()
    {
        base.Awake();

        equippedDisplayText = Item.GetComponentInChildren<TMP_Text>();

        nameActionToUseItem = "Equip";
        HideUI();
    }
    private void Update()
    {
        if ((itemData == InventoryManager.instance.GetWeaponSLot().GetItem()
            || itemData == InventoryManager.instance.GetHeadSlot().GetItem()
            || itemData == InventoryManager.instance.GetbodySlot().GetItem())
            && itemData != emptyItem)
            ShowUI();
        else
            HideUI();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out EquipmentSlot oldEquipmentSlot))
            SwapItemWith(oldEquipmentSlot);
        else if(eventData.pointerDrag.TryGetComponent(out EquippedSlot oldEquippedSlot))
        {
            if(oldEquippedSlot.GetItem() != emptyItem)
            {
                if (oldEquippedSlot.GetItem().cate == global::Item.Cate.Weapon)
                    SwapItemWith(InventoryManager.instance.GetWeaponSLot().GetEquipmentSlot());
                if (oldEquippedSlot.GetItem().cate == global::Item.Cate.Head)
                    SwapItemWith(InventoryManager.instance.GetHeadSlot().GetEquipmentSlot());
                if (oldEquippedSlot.GetItem().cate == global::Item.Cate.Body)
                    SwapItemWith(InventoryManager.instance.GetbodySlot().GetEquipmentSlot());

                oldEquippedSlot.UseItem();
            }
        }

    }

    public override void HideUI() => equippedDisplayText.enabled = false;
    public override void ShowUI() => equippedDisplayText.enabled = true;
    public override void UseItem()
    {
        InventoryManager.instance.Equip(this);
        UpdatePlayerStat(itemData.changeAmout);
    }
}
