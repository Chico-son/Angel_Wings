using UnityEngine;

public class JetController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 25f;
    [SerializeField] private float pitchSpeed = 60f;
    [SerializeField] private float yawSpeed = 45f;
    [SerializeField] private float rollSpeed = 90f;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveForward();
        RotateJet();

    }

    private void MoveForward()
    {
        Vector3 forwardMovement = transform.forward * forwardSpeed;
        rb.linearVelocity = forwardMovement;
    }

    private void RotateJet()
    {
       float pitchInput = Input.GetAxis("Vertical");
       float yawInput = Input.GetAxis("Horizontal");

       float rollInput = 0f;

       if (yawInput.GetKey(KeyCode,Q))
        {
            rollInput = 1f;
        }

        else if (yawInput.GetKey(KeyCode,E))
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