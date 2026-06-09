using UnityEngine;

public class ReticleController : MonoBehaviour
{
    [Header("Reticle Settings")]
    [SerializeField] private RectTransform reticle;
    [SerializeField] private float reticleSpeed = 800f;
    [SerializeField] private float maxRadius = 250f;

    private Vector2 reticlePosition;

    private void Start()
    {
        reticlePosition = Vector2.zero;

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
    }

    public Vector2 GetNormalizedOffset()
    {
        return reticlePosition / maxRadius;
    }
}