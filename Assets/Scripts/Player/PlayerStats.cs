using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable, IAttackable, IDataPersistence
{
    public static PlayerStats instance { get; private set; }

    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float def;
    [SerializeField]
    protected float baseAtk;

    public float Hp => hp;
    public float Def => def;
    public float DamageDealt => baseAtk;

    public void UpdateStats(Item.StateToChange stat, float amout)
    {
        if(stat == Item.StateToChange.Hp) hp += amout;
        if(stat == Item.StateToChange.Def) def += amout;
        if(stat == Item.StateToChange.Atk) baseAtk += amout;
    }

    private void Awake()
    {
        if (instance != null) Debug.LogError("Should only have 1 PlayerStats at a time");
        instance = this;
    }

    public void Attack(IDamageable enemy)
    {
        return;
    }

    public void TakeDamage(float damage)
    {
        hp -= damage * (1 - (def / (def + 40)));
        if (hp <= 0) hp = 0;
    }

    public void SaveData(GameData gameData)
    {
        gameData.hpPlayer = hp;
        gameData.defPlayer = def;
        gameData.baseAtkPlayer = baseAtk;
    }

    public void LoadData(GameData gameData)
    {
        hp = gameData.hpPlayer;
        def = gameData.defPlayer;
        baseAtk = gameData.baseAtkPlayer;
    }
}
