using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float startSpeed = 50f;
    [SerializeField] private float minSpeed = 20f;
    [SerializeField] private float maxSpeed = 120f;
    [SerializeField] private float acceleration = 20f;

    [Header("Turning")]
    [SerializeField] private float turnSpeed = 60f;
    [SerializeField] private float maxBankAngle = 60f;

    [Header("References")]
    [SerializeField] private ReticleController reticleController;

    private Rigidbody rb;
    private float currentSpeed;
    private float currentYaw;
    private float currentPitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        currentSpeed = startSpeed;

        Vector3 e = transform.eulerAngles;
        currentYaw = e.y;
        currentPitch = e.x;
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

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
    }

    private void HandleRotation()
    {
        Vector2 reticle = reticleController.GetNormalizedOffset();

        float horizontalInput = Mathf.Clamp(reticle.x, -1f, 1f);
        float verticalInput = Mathf.Clamp(-reticle.y, -1f, 1f);

        currentYaw += horizontalInput * turnSpeed * Time.fixedDeltaTime;

        currentPitch += verticalInput * turnSpeed * Time.fixedDeltaTime;
        currentPitch = Mathf.Clamp(currentPitch, -80f, 80f);

        float bankAngle = -horizontalInput * maxBankAngle;

        Quaternion targetRotation = Quaternion.Euler(currentPitch, currentYaw, bankAngle);

        rb.MoveRotation(Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            180f * Time.fixedDeltaTime
        ));
    }

    private void HandleMovement()
    {
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    public float CurrentSpeed => currentSpeed;
    public float MaxSpeed => maxSpeed;
    public float MinSpeed => minSpeed;
}