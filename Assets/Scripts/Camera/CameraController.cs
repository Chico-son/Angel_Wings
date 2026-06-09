using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, -10f);
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float rotationSpeed = 2.5f;

    [Range(0f, 1f)]
    [SerializeField] private float rollInfluence = 0.2f;

    private void LateUpdate()
    {
        
        if (target == null)
            return;
        
        Vector3 backOffset = -target.forward * Mathf.Abs(offset.z);
        Vector3 upOffset = Vector3.up * offset.y;
        Vector3 desiredPosition = target.position + backOffset + upOffset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
        
        Vector3 lookDir = target.position - transform.position;
        Vector3 blendedUp = Vector3.Lerp(
            Vector3.up,
            target.up,
            rollInfluence
        );
        Quaternion desiredRotation = Quaternion.LookRotation(
            lookDir,
            blendedUp
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
