using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOwner : MonoBehaviour,IInteractable
{
    public string GetInteractText() => "Talk";

    public Transform GetTransform() => transform;

    public CharacterController Controller { get; private set; }

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
    }

    public void Start()
    {
        if (LevelManager.Instance.TimeOfDay == TimeOfDay.Morning)
            Controller.enabled = false;
    }

    public void Interact()
    {
        if(LevelManager.Instance.TimeOfDay != TimeOfDay.Morning)
        {
            StoreManager.Instance.OpenStore();
        }
    }
}
