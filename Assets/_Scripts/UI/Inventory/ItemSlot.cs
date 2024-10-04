using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : Slot
{
    [SerializeField]
    protected TMP_Text quanlityText;

    public override void AddNewItem(Item item)
    {
        itemData = item;
        if (image != null) image.sprite = item.icon;

        if (item == emptyItem) ResetQuantity();
    }

    protected override void Awake()
    {
        base.Awake();

        quanlityText = ItemInSlot.GetComponentInChildren<TMP_Text>();
        HideUI();
            
        NameActionToUseItem = "Use";
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out ItemSlot oldItemSlot))
            SwapItemWith(oldItemSlot);
    }
    public override void UseItem()
    {
        if (itemData.cate == Item.Cate.Weapon) return;
        if (itemData.cate == Item.Cate.Head) return;
        if (itemData.cate == Item.Cate.Body) return;

        if(itemData.statsToChange == Item.StatToChange.Hp)
            if(PlayerController.Instance.Stats.CanAddMoreHp())
                if (RemoveQuantity(1)) 
                    UpdatePlayerStat(itemData.changeAmout);
    }

    protected override void UpdateQuanlity(int newQuality)
    {
        base.UpdateQuanlity(newQuality);

        quanlityText.text = quantity.ToString();

        if (CanShowUI()) ShowUI();
        else HideUI();
    }

    public override void ShowUI() => quanlityText.enabled = true;
    public override void HideUI() => quanlityText.enabled = false;
}
