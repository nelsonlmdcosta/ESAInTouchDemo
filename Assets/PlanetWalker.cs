using UnityEngine;
using System.Collections;

public class PlanetWalker : MonoBehaviour
{
    // Generates World Orbital World Points
    // SerializeField Mostra Private Variables No Inspector
    [SerializeField] private Transform orbitParent = null;

    // Manter Cache de Components e mais eficiente porque transform. usa GetComponent<>(); por isso tentamos usar isto so uma vez
    private Transform myTransform = null;
    private BezierCurve orbit = null;

    // Keeps Track Of Previous Local Point
    private Vector3 previousPoint = Vector3.zero;

    // Variaveis podem estar escondidas. Se quiseres ver elas a Runtime, entao tens de meter o inspector em Debug.
    private int currentMovementIndex = 0;
    private float currentSpeed = 0;
	private float cos;
	private float sin;
	private float simulationSpeed=1;
    private void Start()
    {
        // Cache Components
        myTransform = GetComponent<Transform>();
        orbit = GetComponent<BezierCurve>();

        // Generate World Points To Transition Around
        orbit.GenerateCurve();

        // Set Position And Velocity At Array Index
        previousPoint = orbit.points[0];
        currentSpeed = orbit.velocity[0];

		cos = Mathf.Cos (orbit.rotationAngle);
		sin = Mathf.Sin (orbit.rotationAngle);
    }

    private void Update()
    {
        MovePlanet();
		RotatePlanet ();
    }

    private void MovePlanet()
    {
        currentSpeed = orbit.velocity[currentMovementIndex];
		previousPoint = Vector3.MoveTowards(previousPoint, orbit.points[currentMovementIndex],simulationSpeed*currentSpeed * Time.deltaTime);
        Vector3 parentPosition = orbitParent != null ? orbitParent.position : Vector3.zero;

        if (previousPoint == orbit.points[currentMovementIndex])
        {
            currentMovementIndex = WrapInt(++currentMovementIndex, 0, 99);
        }

        myTransform.position = previousPoint + parentPosition;
    }

    // Wraps A Value Around Min Max 
    private int WrapInt(int value, int wrapMin, int wrapMax)
    {
        // Ternary Operator To Quickly Get A Wrapped Value
        return (value > wrapMax) ? wrapMin : (value < wrapMin) ? wrapMax : value;
    }
	private void RotatePlanet (){
		myTransform.Rotate (new Vector3 (sin * Time.deltaTime*orbit.rotationSpeed*simulationSpeed*360f, cos * Time.deltaTime*orbit.rotationSpeed*simulationSpeed*360f, 0f), Space.World);

	}
}

