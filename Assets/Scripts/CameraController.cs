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

    public float lerpTime = 1;
    private bool lerping = false;
    private float startTime = 0;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    public void UpdateRotation(Vector2 inputDir)
    {
        x += inputDir.x * xSpeed * 0.005f;
        y = Mathf.Clamp(y - inputDir.y * ySpeed * 0.005f, -90, 90);
    }
    public void UpdateZoomDistance(float offset)
    {
        offset *= 0.05f;
        distance = Mathf.Clamp(distance + offset, distanceMin, distanceMax);
    }

    private void LateUpdate()
    {
        if (!lerping)
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            //RaycastHit hit;
            //if (Physics.Linecast(target.position, transform.position, out hit))
            //{
            //    distance -= hit.distance;
            //}
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
        else
        {
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            //RaycastHit hit;
            //if (Physics.Linecast(target.position, transform.position, out hit))
            //{
            //    distance -= hit.distance;
            //}
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            float normalizedTime = Mathf.Clamp01(((startTime + lerpTime) - Time.time) / lerpTime);

            LerpTowards(position, normalizedTime);

            if (Time.time > startTime + lerpTime)
            {
                lerping = false;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void SetParameters(CameraParameters param)
    {
        if (target != param.target)
        {
            startTime = Time.time;

            target = param.target;

            distanceMin = param.MinDistance;
            distanceMax = param.MaxDistance;

            distance = (distanceMin + distanceMax) / 2;

            return;
        }
    }

    public void LerpTowards(Vector3 targetPosition, float time)
    {
        target.position = Vector3.Lerp(transform.position, targetPosition, time);
    }
}