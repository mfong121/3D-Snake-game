using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
     
    private PlayerControls playerControls;
    private InputAction movement;

    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    public bool mouseControlsActive = true;
    [SerializeField]
    private Camera camera; //This camera determines the player's rotation and direction
    [SerializeField]
    private Transform head;


    //Desired rotation
    /*private Vector3 rotationDirection;*/
    private Quaternion rotationDirection;

    internal float boostMultiplier = 2f;
    internal bool speedBoostActive = false;

    private float keyboardRotationSpeedInDegrees = .5f;
    private float mouseRotationSpeedInDegrees = .01f;

    private float rollAngle = 0;
    private float rollDirection = 0;

    void Awake()
    {
        playerControls = new PlayerControls();
    }



    // Update is called once per frame
    void Update()
    {
/*        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);*/
        //player should turn towards camera rotation & be affected by size, boost


        //sets rotation speed to mouseRotationSpeed if mouseControlsActive, and keyboardRotationSpeed if not
        var cameraRotationSpeed = mouseControlsActive ? mouseRotationSpeedInDegrees : keyboardRotationSpeedInDegrees;
        Debug.Log(cameraRotationSpeed);
        //player head should follow camera slowly instead

        /*        camera.transform.rotation = Quaternion.LookRotation(newDirection);*/
/*        camera.transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * rotationDirection, cameraRotationSpeed);*/
        camera.transform.Rotate(rotationDirection.eulerAngles.x, rotationDirection.eulerAngles.y, 0); //TODO: This makes rotation too fast. Find a way to slow it

        rollAngle = rollDirection * keyboardRotationSpeedInDegrees;
        camera.transform.Rotate(transform.forward, rollAngle);


        //TODO: need to turn player transform rotation to match camera transform rotation
        /*Quaternion.RotateTowards(transform)*/
        /*head.Rotate(transform.forward, 3);*/
    }


    private void OnEnable()
    {
        //obtain reference to head node (transforms will apply to head, and body segments will follow
        

        //subscribe input actions to functions
        playerControls.Player.Boost.started += speedBoostStarted;
        playerControls.Player.Boost.canceled += speedBoostCancelled;
        playerControls.Player.Boost.Enable();

        playerControls.Player.MouseCameraRotation.performed += mouseRotateCameraActionPerformed;
        playerControls.Player.KeyboardCameraRotation.performed += keyboardRotateCameraActionPerformed;

        playerControls.Player.MouseCameraRotation.canceled += RotateCameraActionCancelled;
        playerControls.Player.KeyboardCameraRotation.canceled += RotateCameraActionCancelled;

        if (mouseControlsActive)
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

    private void OnDisable()
    {
        playerControls.Player.MouseCameraRotation.Disable();
        playerControls.Player.KeyboardCameraRotation.Disable();
        playerControls.Player.Boost.Disable();
    }

    private void speedBoostStarted(InputAction.CallbackContext obj)
    {
        moveSpeed *= boostMultiplier;
        speedBoostActive = true;
    }

    private void speedBoostCancelled(InputAction.CallbackContext obj)
    {
        moveSpeed /= boostMultiplier;
        speedBoostActive = false;
    }

    private void keyboardRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //rotate camera according to player turning speed
        //SHOULD ONLY BE ACTIVE WHEN MOUSECONTROLS DISABLED
        Vector2 rotationInput = playerControls.Player.KeyboardCameraRotation.ReadValue<Vector2>();
        float currentRotationSpeed = keyboardRotationSpeedInDegrees;
        rotationDirection = Quaternion.Euler(-rotationInput.y, rotationInput.x, 0);
        /*rotationDirection = new Vector3(rotationInput.x, rotationInput.y, 0);*/
        /*transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(rotationInput.y, rotationInput.x, 0), currentRotationSpeed);*/

    }
    private void mouseRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //boost should not affect camera rotation speed, only player turning speed
        Vector2 deltaMousePos = playerControls.Player.MouseCameraRotation.ReadValue<Vector2>();
        Debug.Log(deltaMousePos);
        //Rotates the camera towards the input
        rotationDirection = Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0);
        /*rotationDirection = new Vector3(deltaMousePos.x, deltaMousePos.y, 0);*/


        /*        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0), rotationSpeedInDegrees);*/

        /*var rotationDirection = Vector3.RotateTowards(transform.forward, new Vector3(transform.position.x + movementVector.x, transform.position.y + movementVector.y, transform.position.z), rotationSpeedInDegrees, 0);*/
        /*transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);*/

        /*Debug.DrawRay(transform.position, rotationDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(rotationDirection);*/
    }

    private void RotateCameraActionCancelled(InputAction.CallbackContext obj)
    {
        rotationDirection = Quaternion.Euler(0, 0, 0);
        /*rotationDirection = new Vector3();*/
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
