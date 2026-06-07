using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float pitchSpeed = 60f;
    [SerializeField] private float yawSpeed = 45f;
    [SerializeField] private float rollSpeed = 90f;
    [SerializeField] private float currentSpeed = 25f;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float acceleration = 20f;




    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("JetController requires a Rigidbody.");
        }
    }

    private void FixedUpdate()
    {
        MoveForward();
        RotateJet();
        HandleThrottle();

    }

    private void MoveForward()
    {
        Vector3 forwardMovement = transform.forward * currentSpeed;
        rb.linearVelocity = forwardMovement;
    }

    private void HandleThrottle()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed -= acceleration * Time.fixedDeltaTime;
        }

        currentSpeed = Mathf.Clamp(
            currentSpeed,
            minSpeed,
            maxSpeed
        );
    }

    private void RotateJet()
    {
       float pitchInput = Input.GetAxis("Vertical");
       float yawInput = Input.GetAxis("Horizontal");

       float rollInput = 0f;

       if (Input.GetKey(KeyCode.Q))
        {
            rollInput = 1f;
        }

        else if (Input.GetKey(KeyCode.E))
        {
            rollInput = -1f;

        }

        Quaternion pitchRotation = Quaternion.AngleAxis(
            pitchInput * pitchSpeed * Time.fixedDeltaTime,
            transform.right
        );

         Quaternion yawRotation = Quaternion.AngleAxis(
            yawInput * yawSpeed * Time.fixedDeltaTime,
            transform.up
        );

         Quaternion rollRotation = Quaternion.AngleAxis(
            rollInput * rollSpeed * Time.fixedDeltaTime,
            transform.forward
        );

        rb.MoveRotation(rb.rotation * pitchRotation * yawRotation * rollRotation);
    }   
}