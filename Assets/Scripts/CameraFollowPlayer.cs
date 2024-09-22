using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    
    private void Update()
    {
        transform.position = new Vector3(PlayerController.Instance.transform.position.x,transform.position.y ,PlayerController.Instance.transform.position.z);
    }
}
