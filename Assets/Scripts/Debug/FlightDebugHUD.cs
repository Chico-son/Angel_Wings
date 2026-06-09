using UnityEngine;

public class FlightDebugHUD : MonoBehaviour
{
    [SerializeField] private Transform jet;
    [SerializeField] private JetController jetController;

    private GUIStyle style;

    private void OnGUI()
    {
        if (jet == null) return;

        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.fontSize = 18;
            style.normal.textColor = Color.green;
            style.fontStyle = FontStyle.Bold;
        }

        Vector3 pos = jet.position;
        Vector3 euler = jet.eulerAngles;

        float pitch = NormalizeAngle(euler.x);
        float yaw = euler.y;
        float roll = NormalizeAngle(euler.z);

        float altitude = pos.y;
        float speed = jetController != null ? jetController.CurrentSpeed : 0f;

        string text =
            $"SPEED:    {speed:F1}\n" +
            $"ALTITUDE: {altitude:F1}\n" +
            $"\n" +
            $"PITCH:    {pitch:F1}°\n" +
            $"YAW:      {yaw:F1}°\n" +
            $"ROLL:     {roll:F1}°\n" +
            $"\n" +
            $"POS X:    {pos.x:F1}\n" +
            $"POS Y:    {pos.y:F1}\n" +
            $"POS Z:    {pos.z:F1}";

        GUI.Label(new Rect(20, 20, 400, 400), text, style);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }
}