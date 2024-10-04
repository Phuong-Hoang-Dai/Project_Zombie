using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField]
    protected Item itemData;

    protected int quantity;

    [SerializeField]
    protected Transform ItemInSlot;

    [SerializeField]
    protected Image image;

    [SerializeField]
    protected Button button;

    [SerializeField]
    protected Item emptyItem;

    public string NameActionToUseItem { get; protected set; }

    protected virtual void Awake()
    {
        ItemInSlot = transform.Find("Item");
        if(ItemInSlot != null )
        {
            image = ItemInSlot.GetComponent<Image>();
            button = ItemInSlot.GetComponent<Button>();
        }
        
        if(itemData == null) itemData = emptyItem;

        NameActionToUseItem = "Please set name action for kind of slot";
    }

    public virtual void AddNewItem(Item item)
    {
        itemData = item;
        if(image != null) image.sprite = item.icon;

        if(item == emptyItem ) ResetQuantity();
    }

    public bool AddQuantity(int addQuantity)
    {
        if (CanAddMoreQuantity(addQuantity))
        {
            UpdateQuanlity(quantity + addQuantity);
            return true;
        }
        return false;
    }

    public bool RemoveQuantity(int removeQuantity)
    {
        if (quantity <= 0) return false;

        UpdateQuanlity(quantity - removeQuantity);
        return true;
    }

    protected virtual void UpdateQuanlity(int newQuality) => quantity = newQuality;

    protected virtual void UpdatePlayerStat(int amoutToChange)
        => PlayerController.Instance.UpdateStats(itemData.statsToChange, amoutToChange);


    public abstract void OnDrop(PointerEventData eventData);

    public void SwapItemWith(Slot oldItemSlot)
    {
        //int quanlityOfNewItemSlot = oldItemSlot.quantity;

        //Item oldItem = (Item)AssetDatabase.LoadAssetAtPath(
        //    AssetDatabase.GetAssetPath(oldItemSlot.GetItem()), typeof(Item));

        //Item newItem = (Item)AssetDatabase.LoadAssetAtPath(
        //    AssetDatabase.GetAssetPath(itemData), typeof(Item));

        //oldItemSlot.AddNewItem(newItem);
        //oldItemSlot.UpdateQuanlity(quantity);

        //AddNewItem(oldItem);
        //UpdateQuanlity(quanlityOfNewItemSlot);

        //button.onClick.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData) => button.onClick.Invoke();
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData) { }

    public virtual bool CanAddMoreQuantity(int addQuantity)
       => quantity + addQuantity <= itemData.maxQuantity;
    public void ResetQuantity() => quantity = 0;
    public bool CanShowUI() => quantity > 0;
    public bool IsFull() => quantity == itemData.maxQuantity;
    public bool IsEmpty() => quantity <= 0;
    public int GetItemAmount() => quantity;
    public Button GetButton() => button;
    public Item GetItem() => itemData;

    public abstract void UseItem();
    public abstract void ShowUI();
    public abstract void HideUI();
}
