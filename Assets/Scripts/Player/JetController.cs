using UnityEngine;

public class JetController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 25f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 forwardMovement = transform.forward * forwardSpeed;
        rb.linearVelocity = forwardMovement;

    }
}   