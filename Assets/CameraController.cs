using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;

    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    public void UpdateRotation(Vector2 inputDir)
    {
        x += inputDir.x * xSpeed * 0.02f;
        y = Mathf.Clamp(y - inputDir.y * ySpeed * 0.02f, -90, 90);
    }

    public void UpdateZoomDistance(float offset)
    {
        distance = Mathf.Clamp(distance + offset, distanceMin, distanceMax);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}