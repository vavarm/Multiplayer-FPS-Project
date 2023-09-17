using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Animator))]
public class PlayerController : NetworkBehaviour
{
    [Header("Player Options")]

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float crouchSpeed = 3f;
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
    private Animator animator;

    [Header("Player Input")]
    private Vector2 moveDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private bool isThrusterHeld = false;
    private bool isCrouching = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();
    }

    public void OnThruster(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isThrusterHeld = true;
        }
        else if (context.canceled)
        {
            isThrusterHeld = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started && !isThrusterHeld)
        {
            isCrouching = true;
        }
        else if (context.canceled)
        {
            isCrouching = false;
        }
    }


    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);
    }

    private void Update()
    {

        ////// Ground check //////

        RaycastHit _hit;
        if (Physics.SphereCast(transform.position, 0.5f, Vector3.down, out _hit, 10f, groundMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        ////// Movement calculation //////

        // calculate movement velocity as a 3D vector
        Vector3 _movementHorizontal = transform.right * moveDirection.x;
        Vector3 _movementVertical = transform.forward * moveDirection.y;

        // calculate movement speed: if thruster is held and fuel is available, use thrusterSpeed, else use crouchSpeed if crouching, else use speed
        float movementSpeed = isThrusterHeld && thrusterFuelAmount >= 0.01f ? thrusterSpeed : (isCrouching ? crouchSpeed : speed);

        // final movement vector
        Vector3 _velocity = (_movementHorizontal + _movementVertical) * movementSpeed;

        ////// Rotation calculation //////

        float _yRot = lookDirection.x;
        Vector3 rotation = new Vector3(0f, _yRot, 0f) * lookSensitivityY;

        ////// Camera rotation calculation //////

        float _xRot = lookDirection.y;
        float _cameraRotationX = _xRot * lookSensitivityX;

        ////// Thruster calculation //////

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

        if (!base.IsOwner)
            return;

        // update the thruster animation
        animator.SetFloat("ForwardVelocity", moveDirection.y, 0.1f, Time.deltaTime);

        // apply movement
        motor.Move(_velocity);

        // apply rotation
        motor.Rotate(rotation);

        // apply rotation
        motor.RotateCamera(_cameraRotationX);

        motor.ApplyThruster(thrusterVelocity);
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
