using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour
{
    [Header("Player Options")]

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float crouchSpeed = 3f;
    private bool isCrouching = false;
    [SerializeField]
    private float thrusterSpeed = 8f;

    [SerializeField]
    private float lookSensitivityX = 10f;

    [SerializeField]
    private float lookSensitivityY = 80f;

    [Header("Thruster Options")]
    [SerializeField]
    private float thrusterForce = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    public float thrusterFuelAmount { get; private set; } = 1f;

    [Header("Joint Options")]
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 50f;

    [Header("Player Components")]
    private ConfigurableJoint joint;
    private PlayerMotor motor;

    [Header("Player Input")]
    private PlayerInputActions playerControls;
    private InputAction move;
    private InputAction look;
    private InputAction crouch;
    private InputAction thruster;

    private Vector2 moveDirection = Vector2.zero;

    private void OnEnable()
    {
        playerControls = new PlayerInputActions();
        move = playerControls.Player.Move;
        move.Enable();
        look = playerControls.Player.Look;
        look.Enable();
        crouch = playerControls.Player.Crouch;
        crouch.Enable();
        thruster = playerControls.Player.Thruster;
        thruster.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        crouch.Disable();
        thruster.Disable();
    }

    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        motor = GetComponent<PlayerMotor>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            // calculate player state
            bool isThrusterHeld = thruster.ReadValue<float>() > 0f;
            if (crouch.triggered && !isThrusterHeld)
            {
                isCrouching = true;
            }
            else if (crouch.WasReleasedThisFrame())
            {
                isCrouching = false;
            }
            /* Set target position for spring */
            RaycastHit _hit;
            if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out _hit, 10f, groundMask))
            {
                joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
            }
            else
            {
                joint.targetPosition = new Vector3(0f, 0f, 0f);
            }

            /* Movement input */
            moveDirection = move.ReadValue<Vector2>();

            // calculate movement velocity as a 3D vector
            Vector3 _movementHorizontal = transform.right * moveDirection.x;
            Vector3 _movementVertical = transform.forward * moveDirection.y;

            // calculate movement speed: if thruster is held and fuel is available, use thrusterSpeed, else use crouchSpeed if crouching, else use speed
            float movementSpeed = isThrusterHeld && thrusterFuelAmount >= 0.01f ? thrusterSpeed : (isCrouching ? crouchSpeed : speed);
            Debug.Log(movementSpeed);
            Debug.Log("isThrusterHeld: " + isThrusterHeld);
            Debug.Log("isCrouching: " + isCrouching);

            // final movement vector
            Vector3 _velocity = (_movementHorizontal + _movementVertical) * movementSpeed;

            // apply movement
            motor.Move(_velocity);

            /* Look input y rotation */
            Vector2 _lookDirection = look.ReadValue<Vector2>();
            float _yRot = _lookDirection.x;
            Vector3 rotation = new Vector3(0f, _yRot, 0f) * lookSensitivityY;

            // apply rotation
            motor.Rotate(rotation);

            /* Look input x rotation */
            float _xRot = _lookDirection.y;
            float _cameraRotationX = _xRot * lookSensitivityX;

            // apply rotation
            motor.RotateCamera(_cameraRotationX);

            /* Thruster */

            // calculate thrusterVelocity
            Vector3 thrusterVelocity = Vector3.zero;
            if (isThrusterHeld && thrusterFuelAmount > 0f)
            {
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
                if (thrusterFuelAmount >= 0.01f)
                {
                    thrusterVelocity = Vector3.up * thrusterForce;
                    SetJointSettings(0f);
                }
            }
            else
            {
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
                SetJointSettings(jointSpring);
            }

            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

            motor.ApplyThruster(thrusterVelocity);

        }
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
