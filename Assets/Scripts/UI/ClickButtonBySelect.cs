using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButtonBySelect : MonoBehaviour, ISelectHandler
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(button != null)
            button.onClick.Invoke();
    }

    
}
