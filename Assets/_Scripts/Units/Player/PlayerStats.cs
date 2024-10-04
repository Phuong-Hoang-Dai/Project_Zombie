using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour,IDamageable, IAttackable, IDataPersistence
{
    [field: SerializeField]
    public float MaxHp { get; set; }
    [field: SerializeField]
    public float CurrentHp { get; set; }
    [field: SerializeField]
    public float Def { get; set; }
    [field: SerializeField]
    public float BaseAtk { get; set; }

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

    private void Update()
    {
        if(CurrentHp > MaxHp) 
            CurrentHp = MaxHp;
    }

    public void Attack(IDamageable enemy) { return; }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage * (1 - (Def / (Def + 40)));
        if (CurrentHp <= 0) CurrentHp = 0;
    }

    public bool CanAddMoreHp() => CurrentHp < MaxHp;

    public void SaveData(GameData gameData)
    {
        gameData.hpPlayer = MaxHp;
        gameData.defPlayer = Def;
        gameData.baseAtkPlayer = BaseAtk;
        gameData.currentHpPlayer = CurrentHp;
    }

    public void LoadData(GameData gameData)
    {
        CurrentHp = gameData.currentHpPlayer;
        MaxHp = gameData.hpPlayer;
        Def = gameData.defPlayer;
        BaseAtk = gameData.baseAtkPlayer;
    }
}
