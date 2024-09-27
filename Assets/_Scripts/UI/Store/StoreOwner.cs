using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreOwner : MonoBehaviour,IInteractable
{
    public string GetInteractText() => "Talk";

    public Transform GetTransform() => transform;

    public void Interact() => StoreManager.Instance.OpenStore();
}
