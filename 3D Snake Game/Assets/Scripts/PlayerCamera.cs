using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField]
    public Transform target;

    // LateUpdate is called once per frame after normal Update calls
    void LateUpdate()
    {
        //move camera to follow player
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
