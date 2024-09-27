using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    
    private void Update()
    {
        transform.position = new Vector3(PlayerController.Instance.Player.transform.position.x
            ,transform.position.y 
            ,PlayerController.Instance.Player.transform.position.z);
    }
}
