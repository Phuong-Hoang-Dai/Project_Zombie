using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayDetailInStore : MonoBehaviour
{
    [SerializeField]
    protected Image icon_item;
    [SerializeField]
    protected TMP_Text name_item;
    [SerializeField]
    protected TMP_Text cate_item;
    [SerializeField]
    protected TMP_Text stat_item;
    [SerializeField]
    protected TMP_Text desc_item;
    [SerializeField]
    protected TMP_Text quantity_item;
    [SerializeField]
    protected TMP_Text actionText_item;
    [SerializeField]
    protected Button interactButton;
    [SerializeField]
    protected Slider amount;

    protected bool isDelay = false;
    protected int currentAmout;

    private void Awake()
    {
        interactButton = GetComponentInChildren<Button>();
        amount = GetComponentInChildren<Slider>();
    }
    private void Update()
    {
        StoreManager.instance.UpdateCurrentAmountToBuy(currentAmout);

        UpdateDetailDisplay(StoreManager.instance.GetCurrentSelectedSlot());

        if (PlayerAssetsInputs.instance.IsInteractUI() && !isDelay)
        {
            ExecuteEvents.Execute(interactButton.gameObject,
                new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            isDelay = true;
        }
        else if (!PlayerAssetsInputs.instance.IsInteractUI())
        {
            isDelay = false;
        }
    }
    public void UpdateDetailDisplay(Slot slot)
    {
        if (slot == null) return;

        Item item = slot.GetItem();

        amount.maxValue = slot.GetItemAmount();
        currentAmout = Mathf.RoundToInt(amount.value);
        if (currentAmout == 0) currentAmout = 1;

        if (slot.IsEmpty())
        {
            icon_item.sprite = item.icon;
            name_item.text = "";
            cate_item.text = "";
            stat_item.text = "";
            desc_item.text = "";
            quantity_item.text = "";
            actionText_item.text = "";

            return;
        }

        icon_item.sprite = item.icon;
        name_item.text = item.nameItem;
        cate_item.text = item.cate.ToString();
        stat_item.text = $"{item.statsToChange}: {item.changeAmout}";
        desc_item.text = item.desc;
        quantity_item.text = $"X{slot.GetItemAmount()}";
        actionText_item.text =  $"{slot.GetNameActionToUseItem()} x{currentAmout}";
    }
}
