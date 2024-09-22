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
    protected Item itemData;
    protected int quantity;

    [SerializeField]
    protected Transform Item;

    [SerializeField]
    protected Image image;

    [SerializeField]
    protected Button button;

    [SerializeField]
    protected Item emptyItem;

    protected string nameActionToUseItem;
    public string GetNameActionToUseItem() => nameActionToUseItem;

    protected virtual void Awake()
    {
        Item = transform.Find("Item");
        if(Item != null )
        {
            image = Item.GetComponent<Image>();
            button = Item.GetComponent<Button>();
        }
        
        itemData = emptyItem;

        nameActionToUseItem = "Please set name action for kind of slot";
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
    {
        if (itemData.statsToChange == global::Item.StateToChange.Hp)
            Debug.Log($"Add {amoutToChange} hp");
        if (itemData.statsToChange == global::Item.StateToChange.Def)
            Debug.Log($"Add {amoutToChange} def");
        if (itemData.statsToChange == global::Item.StateToChange.Atk)
            Debug.Log($"Add {amoutToChange} atk");
    }


    public abstract void OnDrop(PointerEventData eventData);

    public void SwapItemWith(Slot oldItemSlot)
    {
        int quanlityOfNewItemSlot = oldItemSlot.quantity;

        Item oldItem = (Item)AssetDatabase.LoadAssetAtPath(
            AssetDatabase.GetAssetPath(oldItemSlot.GetItem()), typeof(Item));

        Item newItem = (Item)AssetDatabase.LoadAssetAtPath(
            AssetDatabase.GetAssetPath(itemData), typeof(Item));

        oldItemSlot.AddNewItem(newItem);
        oldItemSlot.UpdateQuanlity(quantity);

        AddNewItem(oldItem);
        UpdateQuanlity(quanlityOfNewItemSlot);

        button.onClick.Invoke();
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
