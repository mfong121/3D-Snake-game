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

    public float boostMultiplier = 2f;
    public bool speedBoostActive = false;

    
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        //subscribe input actions to functions
        playerControls.Player.Boost.started += speedBoostStarted;
        playerControls.Player.Boost.canceled += speedBoostCancelled;
        playerControls.Player.Boost.Enable();

        if (!mouseControlsActive)
        {
            playerControls.Player.KeyboardCameraRotation.performed += keyboardRotateCameraActionPerformed;
        }
    }

    private void OnDisable()
    {
        playerControls.Player.MouseCameraRotation.Disable();
        playerControls.Player.Boost.Disable();
    }

    private void keyboardRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //rotate player (not camera) according to player turning speed
        //SHOULD ONLY BE ACTIVE WHEN MOUSECONTROLS DISABLED
        Vector2 rotationInput = playerControls.Player.KeyboardCameraRotation.ReadValue<Vector2>();

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


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);
        //player should turn towards camera rotation & be affected by size, boost
    }
}
