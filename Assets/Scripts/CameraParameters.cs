using UnityEngine;
using System.Collections;

public class CameraParameters : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;

    public Transform target;

    private void Start()
    {
        target = transform;
    }

    public float MinDistance
    {
        get { return minDistance; }
        set { minDistance = value; }
    }

    public float MaxDistance
    {
        get { return minDistance; }
        set { minDistance = value; }
    }
}
