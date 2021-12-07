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
    private float keyboardRotationSpeedInDegrees = 1f;
    private float mouseRotationSpeedInDegrees = 2f;
    private float boostRotationModifier = 4;
    private float rollAngle = 0;
    private float rollDirection = 0;
    
    //Desired rotation
    private Vector3 rotationDirection;
    //private Quaternion rotationDirection;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerController = transform.GetComponent<PlayerController>();
        boostRotationModifier = 2*playerController.boostMultiplier;
    }
    private void OnEnable()
    {
        
        playerControls.Player.MouseCameraRotation.performed += mouseRotateCameraActionPerformed;
        playerControls.Player.KeyboardCameraRotation.performed += keyboardRotateCameraActionPerformed;

        playerControls.Player.MouseCameraRotation.canceled += RotateCameraActionCancelled;
        playerControls.Player.KeyboardCameraRotation.canceled += RotateCameraActionCancelled;

        if (playerController.mouseControlsActive)
        {
            playerControls.Player.MouseCameraRotation.Enable();
        }
        else
        {
            playerControls.Player.KeyboardCameraRotation.Enable();
        }
        playerControls.Player.Roll.performed += rollActionPerformed;
        playerControls.Player.Roll.canceled += rollActionCancelled;
        playerControls.Player.Roll.Enable();

    }

    // LateUpdate is called once per frame after normal Update calls
    void LateUpdate()
    {
        //move camera to follow player
        camera.transform.position = transform.position - camera.transform.forward * 4 + camera.transform.up * 1;

        //sets rotation speed to mouseRotationSpeed if mouseControlsActive, and keyboardRotationSpeed * boostModifier if boosting, or *1 if not.
        var cameraRotationSpeed = playerController.mouseControlsActive ? mouseRotationSpeedInDegrees : keyboardRotationSpeedInDegrees * (playerController.speedBoostActive ? boostRotationModifier : 1);
        //player head should follow camera slowly instead
        var newDirection = Vector3.RotateTowards(transform.position, transform.position + rotationDirection, cameraRotationSpeed, 0);

        camera.transform.rotation = Quaternion.LookRotation(newDirection);
        /*camera.transform.rotation = Quaternion.RotateTowards(transform.rotation,transform.rotation * rotationDirection, cameraRotationSpeed);*/
        
        rollAngle = rollDirection * keyboardRotationSpeedInDegrees;
        camera.transform.Rotate(camera.transform.forward, rollAngle);
        
    }

    private void keyboardRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //rotate camera according to player turning speed
        //SHOULD ONLY BE ACTIVE WHEN MOUSECONTROLS DISABLED
        Vector2 rotationInput = playerControls.Player.KeyboardCameraRotation.ReadValue<Vector2>();
        float currentRotationSpeed = keyboardRotationSpeedInDegrees;
        if (playerController.speedBoostActive)
        {
            currentRotationSpeed *= boostRotationModifier;
        }
        /*rotationDirection = Quaternion.Euler(-rotationInput.y, rotationInput.x,0);*/
        rotationDirection = new Vector3(rotationInput.x, rotationInput.y, 0);
        /*transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(rotationInput.y, rotationInput.x, 0), currentRotationSpeed);*/

    }
    private void mouseRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //boost should not affect camera rotation speed, only player turning speed
        Vector2 deltaMousePos = playerControls.Player.MouseCameraRotation.ReadValue<Vector2>();

        //Rotates the camera towards the input
        /*rotationDirection = Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0);*/
        rotationDirection = new Vector3(deltaMousePos.x, deltaMousePos.y, 0);


/*        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0), rotationSpeedInDegrees);*/

        /*var rotationDirection = Vector3.RotateTowards(transform.forward, new Vector3(transform.position.x + movementVector.x, transform.position.y + movementVector.y, transform.position.z), rotationSpeedInDegrees, 0);*/
        /*transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);*/

        /*Debug.DrawRay(transform.position, rotationDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(rotationDirection);*/
    }

    private void RotateCameraActionCancelled(InputAction.CallbackContext obj)
    {
        /*        rotationDirection = Quaternion.Euler(0, 0, 0);*/
        rotationDirection = new Vector3();
    }
    private void rollActionPerformed(InputAction.CallbackContext obj)
    {
        rollDirection = playerControls.Player.Roll.ReadValue<float>();
        /*transform.Rotate(transform.forward, rollDirection * keyboardRotationSpeedInDegrees, Space.World);*/
    }
    private void rollActionCancelled(InputAction.CallbackContext obj)
    {
        rollDirection = 0;
    }
}
