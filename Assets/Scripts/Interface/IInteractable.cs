using UnityEngine;

public interface IInteractable
{
    public void Interact();
    public string GetInteractText();
    public Transform GetTransform();
}
