using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{
     
    private PlayerControls playerControls;
    private InputAction movement;

    //configurable settings
    [SerializeField]
    public static float moveSpeed = 2f;
    [SerializeField]
    public static float bodySegmentLength = .75f;
    [SerializeField]
    public bool mouseControlsActive = true;
    [SerializeField]
    private Camera camera; //This camera determines the player's rotation and direction
    /*[SerializeField]*/
    private Transform head;
    private Transform body;
    [SerializeField]
    public Transform[] headVariants;
    [SerializeField]
    public Transform[] bodyVariants;

    [SerializeField]
    [Range(0, 1)]
    public int skinOption;

    private int wormLength = 0;
    private Transform previousSegment;

    //Desired rotation
    /*private Vector3 rotationDirection;*/
    private Quaternion rotationDirection;

    internal float boostMultiplier = 5f;

    internal bool speedBoostActive = false;

    private float keyboardRotationSpeed = .25f;
    private float mouseRotationSpeed = .01f;
    private float headRotationSpeed = .25f;
    private float rollAngle = 0;
    private float rollDirection = 0;

    private Transform selectedHeadVariant;
    private Transform selectedBodyVariant;
    private bool gameOver = false;



    void Awake()
    {
        playerControls = new PlayerControls();
        selectedHeadVariant = headVariants[skinOption];
        selectedBodyVariant = bodyVariants[skinOption];

    }
    private void Start()
    {
        head = transform.GetChild(0);
        body = transform.GetChild(1);
        InstantiateWorm();
    }
    // Update is called once per frame
    void Update()
    {
        /*        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);*/
        //player should turn towards camera rotation & be affected by size, boost

        //sets rotation speed to mouseRotationSpeed if mouseControlsActive, and keyboardRotationSpeed if not
        var cameraRotationSpeed = mouseControlsActive ? mouseRotationSpeed : keyboardRotationSpeed;
        /*Debug.Log(cameraRotationSpeed);*/
        //player head should follow camera slowly instead

        /*        camera.transform.rotation = Quaternion.LookRotation(newDirection);*/
/*        camera.transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * rotationDirection, cameraRotationSpeed);*/
        camera.transform.Rotate(rotationDirection.eulerAngles.x, rotationDirection.eulerAngles.y, 0); 

        rollAngle = rollDirection * keyboardRotationSpeed;
        camera.transform.Rotate(head.forward, rollAngle,Space.World);


        //TODO: need to turn player transform rotation to match camera transform rotation
        /*Quaternion.RotateTowards(transform)*/
        head.rotation = Quaternion.RotateTowards(head.rotation, camera.transform.rotation, headRotationSpeed);
        head.transform.position += head.transform.forward * moveSpeed * Time.deltaTime; //move forward
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
        Vector2 rotationInput = playerControls.Player.KeyboardCameraRotation.ReadValue<Vector2>();
        Vector3 rotationValues = new Vector3(-rotationInput.y, rotationInput.x, 0) * keyboardRotationSpeed;
        rotationDirection = Quaternion.Euler(rotationValues);
    }

    private void mouseRotateCameraActionPerformed(InputAction.CallbackContext obj)
    {
        //boost should not affect camera rotation speed, only player turning speed
        Vector2 deltaMousePos = playerControls.Player.MouseCameraRotation.ReadValue<Vector2>();
        /*Debug.Log(deltaMousePos);*/
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

    void InstantiateWorm()
    {
        Transform headSegment = Instantiate(selectedHeadVariant,head.position,Quaternion.identity,head);
        previousSegment = head;
        for (int i = 0; i < wormLength; i++)
        {
            Transform bodySegment = Instantiate(selectedBodyVariant, previousSegment.position -head.forward * bodySegmentLength, Quaternion.identity, body);
            bodySegment.GetComponent<BodyBehavior>().target = previousSegment;
            previousSegment = bodySegment;
        }
    }

    public void AddSegment()
    {
        Transform bodySegment = Instantiate(selectedBodyVariant, previousSegment.position - head.forward * bodySegmentLength, Quaternion.identity, body);
        bodySegment.GetComponent<BodyBehavior>().target = previousSegment;
        previousSegment = bodySegment;
        wormLength++;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(30,30, 200, 200), "Length: " + wormLength + "");

        if (gameOver)
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 200), "Game Over:\nYou lost!\nWith a final size of " + wormLength + ".");
        }
    }
    public void gameEnd()
    {
        moveSpeed = 0;
        this.OnDisable();
        gameOver = true;
    }

}
