using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Rotation Speeds")]
    [SerializeField] private float pitchSpeed = 70f;
    [SerializeField] private float manualYawSpeed = 15f;
    [SerializeField] private float autoYawFactor = 30f;
    [SerializeField] private float rollSpeed = 100f;

    [Header("Velocity")]
    [SerializeField] private float startSpeed = 40f;
    [SerializeField] private float minSpeed = 15f;
    [SerializeField] private float maxSpeed = 120f;
    [SerializeField] private float acceleration = 25f;

    [Header("Flight smoothing")]
    [SerializeField] private float turnTightness = 6f;
    [SerializeField] private float optimalTurnSpeed = 50f;
    [SerializeField] private float minTurnMultiplier = 0.4f;
    [SerializeField] private float inputSmoothing = 8f;
    private Rigidbody rb;
    private float currentSpeed;
    private float smoothedPitch;
    private float smoothedRoll;
    private float smoothedYaw;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("JetController requires a Rigidbody.");
        }
        currentSpeed = startSpeed;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void FixedUpdate()
    {
        HandleThrottle();
        HandleRotation();
        HandleMovement();

    }
    private void HandleThrottle()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed += acceleration * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed -= acceleration * Time.fixedDeltaTime;

        currentSpeed = Mathf.Clamp(
            currentSpeed,
            minSpeed,
            maxSpeed
        );
    }

    private void HandleRotation()
    {
       float rawPitch = Input.GetAxis("Vertical");
       float rawRoll = Input.GetAxis("Horizontal");

       float rawYaw = 0f;

        if (Input.GetKey(KeyCode.Q)) rawYaw = -1f;
        if (Input.GetKey(KeyCode.E)) rawYaw = 1f;

        smoothedPitch = Mathf.Lerp(smoothedPitch, rawPitch, inputSmoothing * Time.fixedDeltaTime);
        smoothedPitch = Mathf.Lerp(smoothedRoll, rawRoll, inputSmoothing * Time.fixedDeltaTime);
        smoothedPitch = Mathf.Lerp(smoothedYaw, rawYaw, inputSmoothing * Time.fixedDeltaTime);

        float bankAngle = Vector3.Dot(transform.right, Vector3.down);
        float autoYaw = bankAngle * autoYawFactor;
        float totalYaw = autoYaw + (smoothedYaw * manualYawSpeed);

        float turnMult = GetTurnMultiplier();

        Vector3 rotationAmount = new Vector3(
            smoothedPitch * pitchSpeed *turnMult,
            totalYaw,
            -smoothedRoll * rollSpeed * turnMult
        ) *Time.fixedDeltaTime;

        Quaternion deltaRotation = Quaternion.Euler(rotationAmount);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void HandleMovement()
    {
        Vector3 desiredVelocity = transform.forward * currentSpeed;
        rb.linearVelocity = Vector3.Lerp(
            rb.linearVelocity,
            desiredVelocity,
            turnTightness * Time.fixedDeltaTime
        );
    }

    private float GetTurnMultiplier()
    {
        float speedDifference = Mathf.Abs(currentSpeed - optimalTurnSpeed);
        float normalizedDiff = Mathf.InverseLerp(0f, maxSpeed, speedDifference);
        return Mathf.Lerp(1.0f, minTurnMultiplier, normalizedDiff);
    }

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;
    public float MinSpeed => minSpeed;
}