using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButtonBySelect : MonoBehaviour, ISelectHandler
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if(_button != null)
            _button.onClick.Invoke();
    }

    
}
