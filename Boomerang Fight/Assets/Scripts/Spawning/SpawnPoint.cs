using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int ID;
    public Vector3 SpawnPosition => transform.position;
    public bool Available = true;
}
