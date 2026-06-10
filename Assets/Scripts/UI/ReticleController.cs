using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [Header("Aim Reticle (Mouse-Controlled)")]
    [SerializeField] private RectTransform reticle;
    [SerializeField] private float reticleSpeed = 800f;
    [SerializeField] private float maxRadius = 250f;

    [Header("Heading Reticle (Plane's Actual Direction)")]
    [SerializeField] private RectTransform headingReticle;
    [SerializeField] private float headingChaseSpeed = 2f;

    private Vector2 reticlePosition;
    private Vector2 headingPosition;

    private void Start()
    {
        reticlePosition = Vector2.zero;
        headingPosition = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        reticlePosition += new Vector2(mouseX, mouseY) * reticleSpeed * Time.deltaTime;

        if (reticlePosition.magnitude > maxRadius)
        {
            reticlePosition = reticlePosition.normalized * maxRadius;
        }

        if (reticle != null)
        {
            reticle.anchoredPosition = reticlePosition;
        }

        headingPosition = Vector2.Lerp(headingPosition, reticlePosition, headingChaseSpeed * Time.deltaTime);

        if (headingReticle != null)
        {
            headingReticle.anchoredPosition = headingPosition;
        }
    }
    public Vector2 GetNormalizedOffset()
    {
        return reticlePosition / maxRadius;
    }

    public Vector2 GetHeadingNormalizedOffset()
    {
        return headingPosition / maxRadius;
    }
    public Vector2 GetAimToHeadingDelta()
    {
        return (reticlePosition - headingPosition) / maxRadius;
    }
}