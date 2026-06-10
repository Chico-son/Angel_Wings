using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Throttle")]
    [SerializeField] private float throttleChangeRate = 30f;    // % per second
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 120f;
    [SerializeField] private float thrustResponsiveness = 1f;   // Lower = heavier feel

    [Header("Turning")]
    [SerializeField] private float pitchSpeed = 40f;            // Degrees per second
    [SerializeField] private float yawSpeed = 20f;              // Degrees per second
    [SerializeField] private float rollSpeed = 90f;             // Degrees per second (Q/E)
    [SerializeField] private float visualBankAngle = 30f;       // Cosmetic roll during yaw turns
    [SerializeField] private float inputSmoothing = 3f;         // Lower = heavier input feel

    [Header("Auto-Level")]
    [SerializeField] private float autoLevelSpeed = 1.5f;       // How fast wings return to level

    [Header("Gravity & Energy")]
    [SerializeField] private float gravityInfluence = 20f;      // Speed loss/gain from climb/dive

    [Header("References")]
    [SerializeField] private ReticleController reticleController;

    private Rigidbody rb;
    private float throttle = 50f;
    private float currentSpeed;

    // Smoothed input values for heavier feel
    private float smoothedPitch;
    private float smoothedYaw;
    private float smoothedRoll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, throttle / 100f);
    }

    private void FixedUpdate()
    {
        HandleThrottleInput();
        HandleSpeed();
        HandleRotation();
        HandleMovement();
    }

    private void HandleThrottleInput()
    {
        if (Input.GetKey(KeyCode.W))
            throttle += throttleChangeRate * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.S))
            throttle -= throttleChangeRate * Time.fixedDeltaTime;

        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void HandleSpeed()
    {
        float targetSpeed = Mathf.Lerp(minSpeed, maxSpeed, throttle / 100f);

        float pitchFactor = transform.forward.y;
        float gravityEffect = -pitchFactor * gravityInfluence;
        targetSpeed += gravityEffect;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, thrustResponsiveness * Time.fixedDeltaTime);
        currentSpeed = Mathf.Max(currentSpeed, minSpeed * 0.5f);
    }

    private void HandleRotation()
    {

        Vector2 reticle = reticleController.GetNormalizedOffset();
        float rawPitch = Mathf.Clamp(-reticle.y, -1f, 1f);
        float rawYaw = Mathf.Clamp(reticle.x, -1f, 1f);

        float rawRoll = 0f;
        if (Input.GetKey(KeyCode.Q)) rawRoll = 1f;
        if (Input.GetKey(KeyCode.E)) rawRoll = -1f;

        smoothedPitch = Mathf.Lerp(smoothedPitch, rawPitch, inputSmoothing * Time.fixedDeltaTime);
        smoothedYaw = Mathf.Lerp(smoothedYaw, rawYaw, inputSmoothing * Time.fixedDeltaTime);
        smoothedRoll = Mathf.Lerp(smoothedRoll, rawRoll, inputSmoothing * Time.fixedDeltaTime);

        float pitchAmount = smoothedPitch * pitchSpeed * Time.fixedDeltaTime;
        float yawAmount = smoothedYaw * yawSpeed * Time.fixedDeltaTime;
        float rollAmount = smoothedRoll * rollSpeed * Time.fixedDeltaTime;

        Quaternion pitchRotation = Quaternion.AngleAxis(pitchAmount, transform.right);
        Quaternion yawRotation = Quaternion.AngleAxis(yawAmount, Vector3.up);

        Quaternion newRotation = yawRotation * pitchRotation * rb.rotation;

        Quaternion rollRotation = Quaternion.AngleAxis(rollAmount, transform.forward);
        newRotation = rollRotation * newRotation;

        bool notRolling = Mathf.Abs(rawRoll) < 0.01f;
        bool mostlyUpright = IsMostlyUpright(newRotation);

        if (notRolling && mostlyUpright)
        {
            newRotation = ApplyAutoLevel(newRotation);
        }

        if (mostlyUpright)
        {
            Quaternion bankRotation = Quaternion.AngleAxis(-smoothedYaw * visualBankAngle, transform.forward);
            newRotation = Quaternion.Slerp(newRotation, bankRotation * newRotation, 0.05f);
        }

        rb.MoveRotation(newRotation);
    }

    private bool IsMostlyUpright(Quaternion rotation)
    {

        Vector3 planeUp = rotation * Vector3.up;
        return Vector3.Dot(planeUp, Vector3.up) > 0.3f;
    }

    private Quaternion ApplyAutoLevel(Quaternion currentRotation)
    {
        Vector3 forward = currentRotation * Vector3.forward;
        Quaternion levelRotation = Quaternion.LookRotation(forward, Vector3.up);
        return Quaternion.Slerp(currentRotation, levelRotation, autoLevelSpeed * Time.fixedDeltaTime);
    }

    private void HandleMovement()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;
    public float MinSpeed => minSpeed;
    public float Throttle => throttle;
}