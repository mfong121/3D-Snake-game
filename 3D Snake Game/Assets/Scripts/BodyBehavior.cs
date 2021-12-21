using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBehavior : MonoBehaviour
{

    public Transform target; //Target is the previous body segment

    // Update is called once per frame
    void LateUpdate()
    {
        if (Vector3.Distance(transform.position,target.position) > PlayerController.bodySegmentLength)
        {
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            transform.position += transform.forward * PlayerController.moveSpeed * Time.deltaTime; //move forward

        }
    }
}
