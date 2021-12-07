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


    internal float boostMultiplier = 2f;
    internal bool speedBoostActive = false;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);
        //player should turn towards camera rotation & be affected by size, boost

        //TODO: need to turn player transform rotation to match camera transform rotation
    }
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


}
