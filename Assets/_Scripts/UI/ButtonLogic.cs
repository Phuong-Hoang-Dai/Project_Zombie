using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public int index;
    public Button Button {  get; protected set; }
    protected Transform selected;

    private void Awake()
    {
        Button = GetComponentInChildren<Button>();
        selected = transform.Find("Selected");
        Button.AddComponent<ClickButtonBySelect>();

        Deselect();
    }
    private void Start()
    {
        
    }

    public void Select() => selected.gameObject.SetActive(true);
    public void Deselect() => selected.gameObject.SetActive(false);
}
