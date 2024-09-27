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
    public float defPlayer;
    [SerializeField]
    public float baseAtkPlayer;
    [SerializeField]
    public List<InventoryItem> inventoryItems;
    [SerializeField]
    public List<InventoryItem> storeItems;
    [SerializeField]
    public int day;
    [SerializeField]
    public TimeOfDay timeOfDay;

    public GameData()
    {
        id = -1;
        day = 0;
        timeOfDay = TimeOfDay.Afternoon;

        hpPlayer = 200;
        defPlayer = 5;
        baseAtkPlayer = 50;
    }
}
