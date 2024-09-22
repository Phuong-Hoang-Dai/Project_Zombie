using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    protected float interactRange = 2f;
    private IInteractable interactableObj;

    void Update()
    {
        UpdateInteractalbeObject();

        if (PlayerAssetsInputs.instance.IsInteract() && interactableObj != null) interactableObj.Interact();
    }


    private void UpdateInteractalbeObject()
    {
        interactableObj = null;

        //Get interactable objects in range
        List<IInteractable> interactableObjects = new List<IInteractable>();
        Collider[] objectInRange = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in objectInRange)
            if (collider.TryGetComponent(out IInteractable interactableObject))
                interactableObjects.Add(interactableObject);

        //Get the interactable object closest
        foreach (IInteractable obj in interactableObjects)
        {
            if (interactableObj == null) interactableObj = obj;
            else if (Vector3.Distance(transform.position, interactableObj.GetTransform().position)
            > Vector3.Distance(transform.position, obj.GetTransform().position))
            {
                interactableObj = obj;
            }
        }
    }

    public IInteractable GetInteractableObject() => interactableObj;
}
