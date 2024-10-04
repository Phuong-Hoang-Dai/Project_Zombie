using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductSlotInStore : Slot
{
    [SerializeField]
    protected TMP_Text quanlityText;

    protected override void Awake()
    {
        base.Awake();

        quanlityText = ItemInSlot.GetComponentInChildren<TMP_Text>();

        NameActionToUseItem = "Buy";

        HideUI();
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

    public override bool CanAddMoreQuantity(int addQuantity) => true;
    public override void OnDrop(PointerEventData eventData) { return; }
    public override void UseItem() { return; }
}
