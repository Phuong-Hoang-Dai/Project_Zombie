using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>, IDamageable, IAttackable, IDataPersistence
{
    [field: SerializeField]
    public float Hp { get; private set; }
    [field: SerializeField]
    public float Def { get; private set; }
    [field: SerializeField]
    public float BaseAtk {  get; private set; }
    [field: SerializeField]
    public float MoveSpeed { get; private set; }
    [field: SerializeField]
    public float SprintSpeed { get; private set; }
    [field: SerializeField]
    public float SpeedChangeRate { get; private set; }
    [field: SerializeField]
    public float SpeedOffset { get; private set; }
    [field: SerializeField]
    public float RotationSmoothTime { get; private set; }

    public void UpdateStats(Item.StateToChange stat, float amout)
    {
        if(stat == Item.StateToChange.Hp) Hp += amout;
        if(stat == Item.StateToChange.Def) Def += amout;
        if(stat == Item.StateToChange.Atk) BaseAtk += amout;
    }

    public void Attack(IDamageable enemy) { return; }

    public void TakeDamage(float damage)
    {
        Hp -= damage * (1 - (Def / (Def + 40)));
        if (Hp <= 0) Hp = 0;
    }

    public void SaveData(GameData gameData)
    {
        gameData.hpPlayer = Hp;
        gameData.defPlayer = Def;
        gameData.baseAtkPlayer = BaseAtk;
    }

    public void LoadData(GameData gameData)
    {
        Hp = gameData.hpPlayer;
        Def = gameData.defPlayer;
        BaseAtk = gameData.baseAtkPlayer;
    }
}
