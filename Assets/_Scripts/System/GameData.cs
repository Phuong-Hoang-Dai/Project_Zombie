using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField]
    public int id;
    [SerializeField]
    public float hpPlayer;
    [SerializeField]
    public float currentHpPlayer;
    [SerializeField]
    public float defPlayer;
    [SerializeField]
    public float baseAtkPlayer;
    [SerializeField]
    public int coin;
    [SerializeField]
    public List<InventoryItem> inventoryItems = new();
    [SerializeField]
    public InventoryItem weaponSlot;
    [SerializeField]
    public List<InventoryItem> storeItems = new();
    [SerializeField]
    public int day;
    [SerializeField]
    public TimeOfDay timeOfDay;

    public GameData()
    {
        id = -1;
        day = 0;

        timeOfDay = TimeOfDay.Morning;

        hpPlayer = 200;
        currentHpPlayer = hpPlayer;
        defPlayer = 5;
        baseAtkPlayer = 50;
        coin = 0;
    }
}
