using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompleteLevel : MonoBehaviour,IInteractable
{
    public TMP_Text text;

    public string GetInteractText() => "Complete";

    public Transform GetTransform() => transform;

    private void Start()
    {
        text.enabled = false;
    }

    private void Update()
    {
        if(LevelManager.Instance.LevelIsCompleted() 
            || LevelManager.Instance.TimeOfDay == TimeOfDay.Afternoon)
            text.enabled = true;
    }

    public void Interact()
    {
        if (LevelManager.Instance.LevelIsCompleted() || LevelManager.Instance.Day == 0 
            || LevelManager.Instance.TimeOfDay == TimeOfDay.Afternoon)
        {
            LevelManager.Instance.ChangeTime();

            DataManager.Instance.LoadScene();
        }
           
    }
}
