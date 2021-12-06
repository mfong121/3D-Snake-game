using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class PlayerCamera : MonoBehaviour
{

    [SerializeField]
    public Camera camera;
    PlayerController playerController;
    private PlayerControls playerControls;

    //Needs to be lower for larger objects, smaller for small objects
    private float rotationSpeedInDegrees = 1f;
    private float boostRotationModifier = 4;
    
    //Desired rotation
    private Quaternion rotationDirection;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerController = transform.GetComponent<PlayerController>();
        boostRotationModifier = 2*playerController.boostMultiplier;
    }
    private void OnEnable()
    {
        
        playerControls.Player.MouseCameraRotation.performed += mouseRotateCameraActionPerformed;
        
        if (playerController.mouseControlsActive)
        {
            playerControls.Player.MouseCameraRotation.Enable();
        }
        
        playerControls.Player.Roll.performed += rollActionPerformed;
        playerControls.Player.Roll.Enable();
    }

    // LateUpdate is called once per frame after normal Update calls
    void LateUpdate()
    {
        //move camera to follow player
        camera.transform.position = transform.position - camera.transform.forward * 4 + camera.transform.up * 1;
        
        //player head should follow camera slowly instead
        transform.rotation = camera.transform.rotation;
    }


    private void mouseRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //boost should not affect camera rotation speed, only player turning speed
        Vector2 deltaMousePos = playerControls.Player.MouseCameraRotation.ReadValue<Vector2>();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0), rotationSpeedInDegrees);

        /*var rotationDirection = Vector3.RotateTowards(transform.forward, new Vector3(transform.position.x + movementVector.x, transform.position.y + movementVector.y, transform.position.z), rotationSpeedInDegrees, 0);*/
        /*transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);*/

        /*Debug.DrawRay(transform.position, rotationDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(rotationDirection);*/
    }
    private void rollActionPerformed(InputAction.CallbackContext obj)
    {
        float rotationDirection = playerControls.Player.Roll.ReadValue<float>();
        transform.Rotate(transform.forward, rotationDirection * rotationSpeedInDegrees, Space.World);
    }
}
