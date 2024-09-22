using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public int index;
    protected Button button;
    protected Transform selected;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
        selected = transform.Find("Selected");
        button.AddComponent<ClickButtonBySelect>();

        Deselect();
    }
    private void Start()
    {
        
    }

    public Button GetButton() => button;
    public void Select() => selected.gameObject.SetActive(true);
    public void Deselect() => selected.gameObject.SetActive(false);
}
