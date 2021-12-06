using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
     
    private PlayerControls playerControls;
    private InputAction movement;

    [SerializeField]
    private float moveSpeed = 2f;

    public float boostMultiplier = 2f;
    public bool speedBoostActive = false;

    
    void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        movement = playerControls.Player.CameraRotation;
        movement.Enable();

        playerControls.Player.Boost.started += speedBoostStarted;
        playerControls.Player.Boost.canceled += speedBoostCancelled;
        playerControls.Player.Boost.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.CameraRotation.Disable();
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


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);

    }
}
