using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField]
    public Transform target;
    
    // LateUpdate is called once per frame after normal Update calls
    void LateUpdate()
    {
        //move camera to follow player
        transform.position = target.transform.position - transform.forward * 4 + transform.up * 1;       
    }    
}
