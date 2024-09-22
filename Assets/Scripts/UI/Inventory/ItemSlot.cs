using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : Slot
{
    [SerializeField]
    protected TMP_Text quanlityText;

    protected override void Awake()
    {
        base.Awake();

        quanlityText = Item.GetComponentInChildren<TMP_Text>();
        HideUI();
            
        nameActionToUseItem = "Use";
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out ItemSlot oldItemSlot))
            SwapItemWith(oldItemSlot);
    }
    public override void UseItem()
    {
        if (itemData.cate == global::Item.Cate.Weapon) return;
        if (itemData.cate == global::Item.Cate.Head) return;
        if (itemData.cate == global::Item.Cate.Body) return;

        if (RemoveQuantity(1)) UpdatePlayerStat(itemData.changeAmout);
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
