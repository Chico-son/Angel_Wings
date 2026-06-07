using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -10f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private void LateUpdate()
    {
        
        if (target == null)
        {
            return;
        }
        
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(
            target.position - transform.position,
            target.up
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
