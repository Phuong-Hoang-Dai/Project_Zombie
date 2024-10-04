using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
    public TMP_Text hp;
    public TMP_Text def;
    public TMP_Text baseAtk;
    public TMP_Text Coin;


    private void Update()
    {
        UpdateStats();
    }
    public void UpdateStats()
    {
        hp.text = $"Hp: {Mathf.RoundToInt(PlayerController.Instance.Stats.CurrentHp * 100) /100f}/{PlayerController.Instance.Stats.MaxHp}";
        def.text = $"Def: {PlayerController.Instance.Stats.Def}";
        baseAtk.text = $"Attack: {PlayerController.Instance.Stats.BaseAtk}";
        Coin.text = $"COIN: {InventoryManager.Instance.Coin}";
    }
}
