using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private float distance = 25f;
    [SerializeField] private float height = 6f;
    [SerializeField] private float followSpeed = 4f;
    [SerializeField] private float rotationSpeed = 3f;

    [Range(0f, 1f)]
    [SerializeField] private float rollInfluence = 0.2f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetForward = target.forward;
        targetForward.y = 0f;
        if (targetForward.sqrMagnitude < 0.01f) targetForward = Vector3.forward;
        targetForward.Normalize();

        Vector3 desiredPos = target.position
                             - targetForward * distance
                             + Vector3.up * height;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            followSpeed * Time.deltaTime
        );

        Vector3 lookDir = (target.position + target.forward * 10f) - transform.position;
        Vector3 blendedUp = Vector3.Lerp(Vector3.up, target.up, rollInfluence);
        Quaternion desiredRot = Quaternion.LookRotation(lookDir, blendedUp);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            rotationSpeed * Time.deltaTime
        );
    }
}
