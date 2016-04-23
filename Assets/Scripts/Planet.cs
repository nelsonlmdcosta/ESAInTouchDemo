using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    [SerializeField] private Transform[] bezierPoints;
    [SerializeField] private Transform[] bezierTangents;

    [SerializeField] private float bezierTransitionSpeed;

    [SerializeField] private float rotationSpeed;

    private int currentBezierIndex = 0;
    private float bezierTimer = 0;

    private Transform myTransform;

    private void Start ()
    {
        myTransform = GetComponent<Transform>();
	}
	
	private void Update ()
    {
        MovePlanet();
        RotatePlanet();
	}

    private void MovePlanet()
    {
        bezierTimer = Mathf.Clamp01( bezierTimer + SimulationController.Instance.TimeModifier * Time.deltaTime / bezierTransitionSpeed );

        if(currentBezierIndex == 0)
            myTransform.position = GetPositionFromBezier(bezierPoints[0].position, bezierPoints[1].position, bezierTangents[currentBezierIndex].position, bezierTangents[currentBezierIndex + 1].position, bezierTimer);
        else
            myTransform.position = GetPositionFromBezier(bezierPoints[1].position, bezierPoints[0].position, bezierTangents[currentBezierIndex].position, bezierTangents[currentBezierIndex + 1].position, bezierTimer);



        // Mathf.Clamp01 Guarentees That The Max Value Is 1
        if (SimulationController.Instance.TimeModifier > 0)
        {
            if (bezierTimer == 1)
            {
                if (currentBezierIndex == 0)
                {
                    currentBezierIndex += 2;
                }
                else
                {
                    currentBezierIndex -= 2;
                }
                bezierTimer = 0;
                return;
            }
        }

        if (SimulationController.Instance.TimeModifier < 0)
        {
            if (bezierTimer == 0)
            {
                if (currentBezierIndex == 0)
                {
                    currentBezierIndex += 2;
                }
                else
                {
                    currentBezierIndex -= 2;
                }
                bezierTimer = 1;
                return;
            }
        }
    }

    private void RotatePlanet()
    {
        Vector3 myEulerRotation = myTransform.rotation.eulerAngles;

        float speed = SimulationController.Instance.TimeModifier * Time.deltaTime * rotationSpeed;

        myTransform.rotation = Quaternion.Euler( myEulerRotation.x, myEulerRotation.y + speed, myEulerRotation.z);
    }

    private Vector3 GetPositionFromBezier(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent, float time)
    {
        return ((Mathf.Pow((1 - time), 3) * startPoint) + (3 * Mathf.Pow((1 - time), 2) * time * startTangent) + (3 * (1 - time) * Mathf.Pow(time, 2) * endTangent) + (Mathf.Pow(time, 3) * endPoint));
    }
}
