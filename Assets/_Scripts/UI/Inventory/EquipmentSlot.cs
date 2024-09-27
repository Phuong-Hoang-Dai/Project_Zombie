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

        NameActionToUseItem = "Equip";
        HideUI();
    }
    private void Update()
    {
        if ((itemData == InventoryManager.Instance.WeaponSlot.GetItem()
            || itemData == InventoryManager.Instance.HeadSlot.GetItem()
            || itemData == InventoryManager.Instance.BodySlot.GetItem())
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
                    SwapItemWith(InventoryManager.Instance.WeaponSlot.EquipmentSlot);
                if (oldEquippedSlot.GetItem().cate == global::Item.Cate.Head)
                    SwapItemWith(InventoryManager.Instance.HeadSlot.EquipmentSlot);
                if (oldEquippedSlot.GetItem().cate == global::Item.Cate.Body)
                    SwapItemWith(InventoryManager.Instance.BodySlot.EquipmentSlot);

                oldEquippedSlot.UseItem();
            }
        }

    }

    public override void HideUI() => equippedDisplayText.enabled = false;
    public override void ShowUI() => equippedDisplayText.enabled = true;
    public override void UseItem()
    {
        InventoryManager.Instance.Equip(this);
        UpdatePlayerStat(itemData.changeAmout);
    }
}
