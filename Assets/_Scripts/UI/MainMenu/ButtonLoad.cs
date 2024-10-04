using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonLoad : MonoBehaviour
{
    public Button Button { get; protected set; }
    public TMP_Text Text { get; protected set; }
    public int id = -1;

    protected void Awake()
    {
        Button = GetComponent<Button>();
        Text = GetComponentInChildren<TMP_Text>();
    }

    protected void Start()
    {
        Button.onClick.AddListener(() => Load());
    }

    public void Load()
    {
        if (id == -1) return;
        MainMenu.Instance.LoadSave(id);
    }
}
