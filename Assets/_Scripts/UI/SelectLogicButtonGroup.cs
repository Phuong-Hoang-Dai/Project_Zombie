using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SelectLogicButtonGroup : MonoBehaviour
{
    protected ButtonLogic lastSelected;
    [SerializeField]
    protected ButtonLogic[] buttonsInGroup;
    protected virtual void Awake()
    {
        buttonsInGroup = GetComponentsInChildren<ButtonLogic>();
        for (int i = 0; i < buttonsInGroup.Length; i++) 
            buttonsInGroup[i].index = i;
    }
    protected virtual void Start()
    {
        lastSelected = buttonsInGroup[0];
        lastSelected.Button.onClick.Invoke();
    }

    protected virtual void Update()
    {

    }

    public bool HasAButtonSelectedByEventSystem()
    {
        for (int i = 0; i < buttonsInGroup.Length; i++)
            if (buttonsInGroup[i].Button.gameObject == EventSystem.current.currentSelectedGameObject)
                return true;
        return false;
    }


    public void SelectedButton(ButtonLogic newSelectedBtn)
    {
        if(lastSelected != null) lastSelected.Deselect();
        newSelectedBtn.Select();
        lastSelected = newSelectedBtn;

        OnSelectButtonInGroup(newSelectedBtn.gameObject);
    }
    public abstract void OnSelectButtonInGroup(GameObject gameObject);
}
