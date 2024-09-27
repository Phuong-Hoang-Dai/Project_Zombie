using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayDetail : MonoBehaviour
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

    protected bool isDelay = false;

    private void Awake()
    {
        interactButton = GetComponentInChildren<Button>();
    }
    private void Update()
    {

        UpdateDetailDisplay(InventoryManager.Instance.currentSelectedSlot);

        if (InputManager.Instance.IsInteractUI && !isDelay)
        {
            ExecuteEvents.Execute(interactButton.gameObject,
                new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            isDelay = true;
        }else if(!InputManager.Instance.IsInteractUI)
        {
            isDelay = false;
        }
    }
  
    public void UpdateDetailDisplay(Slot slot)
    {
        if(slot == null) return;

        Item item = slot.GetItem();

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
        quantity_item.text = item.isEquipable? "" : $"X{slot.GetItemAmount()}";
        actionText_item.text = slot.NameActionToUseItem;
    }
}
