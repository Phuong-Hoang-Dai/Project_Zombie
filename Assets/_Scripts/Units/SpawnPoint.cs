using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField]
    public string NameSpawnPoint {  get; private set; }

    public Vector3 GetPositionSpawnPoint() => transform.position;
    public Quaternion GetRotationSpawnPoint() => transform.rotation;
}
