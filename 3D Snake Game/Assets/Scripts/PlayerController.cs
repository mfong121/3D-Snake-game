using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
     
    private PlayerControls playerControls;
    private InputAction movement;

    [SerializeField]
    private float moveSpeed = 2f;

    private float boostMultiplier = 2f;

    //Needs to be lower for larger objects, smaller for small objects
    private float rotationSpeedInDegrees = 4f;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.CameraRotation;
        movement.Enable();

        playerControls.Player.CameraRotation.performed += rotateCamera;
        playerControls.Player.CameraRotation.Enable();

        playerControls.Player.Boost.started += speedBoostStarted;
        playerControls.Player.Boost.canceled += speedBoostCancelled;
        playerControls.Player.Boost.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.CameraRotation.Disable();
        playerControls.Player.Boost.Disable();
    }

    private void rotateCamera(InputAction.CallbackContext obj)
    {
        Vector2 deltaMousePos = playerControls.Player.CameraRotation.ReadValue<Vector2>();

        transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * Quaternion.Euler(-deltaMousePos.y, deltaMousePos.x, 0),rotationSpeedInDegrees);
        /*var rotationDirection = Vector3.RotateTowards(transform.forward, new Vector3(transform.position.x + movementVector.x, transform.position.y + movementVector.y, transform.position.z), rotationSpeedInDegrees, 0);*/
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        /*Debug.DrawRay(transform.position, rotationDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(rotationDirection);*/
    }

    private void speedBoostStarted(InputAction.CallbackContext obj)
    {
        moveSpeed *= boostMultiplier;
        rotationSpeedInDegrees /= 2*boostMultiplier;
    }

    private void speedBoostCancelled(InputAction.CallbackContext obj)
    {
        moveSpeed /= boostMultiplier;
        rotationSpeedInDegrees *= 2*boostMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);

    }
}
