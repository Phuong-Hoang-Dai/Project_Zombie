using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New inventory item", menuName = "Asset/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public bool isEquipable;
    public string nameItem;
    public Cate cate = new();
    public StatToChange statsToChange = new();
    public int changeAmout;
    public int maxQuantity;
    public string desc;
    public float price;
    public GameObject prefab;

    public enum Cate
    {
        None,
        Drink,
        Food,
        Weapon,
        Head,
        Body
    }
    public enum StatToChange
    {
        None,
        Hp,
        Def,
        Atk,
        MaxHp
    }
}
