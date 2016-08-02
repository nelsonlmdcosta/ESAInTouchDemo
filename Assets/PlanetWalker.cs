using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlanetWalker : MonoBehaviour
{
    // Generates World Orbital World Points
    // SerializeField Mostra Private Variables No Inspector
    [SerializeField] private Transform orbitParent = null;

    // Manter Cache de Components e mais eficiente porque transform. usa GetComponent<>(); por isso tentamos usar isto so uma vez
    private Transform myTransform = null;
    private BezierCurve orbit = null;
	private BezierCurve components=null;
	public GameObject Canvas;
	public Scrollbar bar;
	public Button[] button;
	public Text[] textGet;
	public Text scale;
	public Text PlanetName;
	public Text PlanetInformation;
	private string text;
	private float scaleFloat;
	public CameraControl controller;

    // Keeps Track Of Previous Local Point
    private Vector3 previousPoint = Vector3.zero;

    // Variaveis podem estar escondidas. Se quiseres ver elas a Runtime, entao tens de meter o inspector em Debug.
	private int planetNumber;
	private int previousPlanetNumber;
	private int currentMovementIndex = 0;
    private float currentSpeed = 0;
	private float cos;
	private float sin;
	private bool cameraSwitchButton=true;
	private bool cameraSwitchTouch=false;
	private bool goToSwitch=false;
	private int rotationController=0;

	Ray ray;
	RaycastHit hit;
	private GameObject hitPlanet;
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
		button = new Button[12];
		cos = Mathf.Cos (orbit.rotationAngle);
		sin = Mathf.Sin (orbit.rotationAngle);
		Canvas = GameObject.Find ("Canvas");
		bar=Canvas.GetComponentInChildren<Scrollbar>();
		button=Canvas.GetComponentsInChildren<Button>();
		textGet = Canvas.GetComponentsInChildren<Text> ();
		PlanetName = textGet [0];
		PlanetInformation = textGet [1];
		scale = textGet[2];
		for (int i=0; i<12;i = i + 1) {
			button [i].enabled = false;
			button [i].transform.position=button[12].transform.position;
		}
		button[0].onClick.AddListener(() =>{CameraSwitcher(1);});
		button[1].onClick.AddListener(() =>{CameraSwitcher(2);});
		button[2].onClick.AddListener(() =>{CameraSwitcher(3);});
		button[3].onClick.AddListener(() =>{CameraSwitcher(4);});
		button[4].onClick.AddListener(() =>{CameraSwitcher(5);});
		button[5].onClick.AddListener(() =>{CameraSwitcher(6);});
		button[6].onClick.AddListener(() =>{CameraSwitcher(7);});
		button[7].onClick.AddListener(() =>{CameraSwitcher(8);});
		button[8].onClick.AddListener(() =>{CameraSwitcher(9);});
		button[9].onClick.AddListener(() =>{CameraSwitcher(10);});
		button[10].onClick.AddListener(() =>{CameraSwitcher(11);});
		button[11].onClick.AddListener(() =>{CameraSwitcher(12);});
		button[12].onClick.AddListener(() =>{GotoButtonSwitcher();});
		planetNumber = Random.Range (1, 12);


    }

    private void Update()
    {
		scaleFloat = bar.value * 50;
		SimulationController.Instance.TimeModifier = scaleFloat;
		text = scaleFloat.ToString ();
		text = text + " Days/s";
		scale.text = text;


        MovePlanet();
		RotatePlanet ();
		if(previousPlanetNumber!=planetNumber)
		{cameraSwitchButton = true;}
		if (cameraSwitchButton==true){
			    
			PlanetButtonPressed ();	
		}

		ray = GameObject.Find ("Main Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
		{
			if (Input.GetMouseButton (1)) {
				rotationController = 0;
				cameraSwitchButton = false;
				cameraSwitchTouch = true;
				hitPlanet = GameObject.Find (hit.collider.name);

			}

		}
		if (cameraSwitchTouch==true){

			GameObject.Find ("Main Camera").transform.position=hitPlanet.transform.position+new Vector3 (hitPlanet.transform.localScale.x*2f, 0f, 0);
			PlanetName.text = hitPlanet.name;
			if(PlanetName.text=="Sun"){	
				PlanetInformation.text = "The Sun is very big and hot";
			}
			else{
				components = hitPlanet.GetComponent<BezierCurve>();
				PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
			}
			if(rotationController==0){
				GameObject.Find ("Main Camera").transform.rotation = new Quaternion (0f, -0.718f, 0, 0.718f);
				rotationController = 1;
			}
		}




    }


    private void MovePlanet()
    {
        currentSpeed = orbit.velocity[currentMovementIndex];
		previousPoint = Vector3.MoveTowards(previousPoint, orbit.points[currentMovementIndex],SimulationController.Instance.TimeModifier*currentSpeed * Time.deltaTime);
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
		myTransform.Rotate (new Vector3 (sin * Time.deltaTime*orbit.rotationSpeed*SimulationController.Instance.TimeModifier*360f, cos * Time.deltaTime*orbit.rotationSpeed*SimulationController.Instance.TimeModifier*360f, 0f), Space.World);

	}
	void CameraSwitcher(int buttonNumber)
	{
		rotationController = 0;
		if (cameraSwitchButton == false) {
			cameraSwitchButton = true;
		} else {
			cameraSwitchButton = false;
		}
		planetNumber= buttonNumber;
	}
	private void PlanetButtonPressed(){
		cameraSwitchTouch = false;
		if(planetNumber==1){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetMercury").transform.position + new Vector3 (GameObject.Find ("PlanetMercury").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetMercury").name;
			components = GameObject.Find ("PlanetMercury").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==2){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetVenus").transform.position + new Vector3 (GameObject.Find("PlanetVenus").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetVenus").name;
			components = GameObject.Find ("PlanetVenus").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==3){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetEarth").transform.position + new Vector3 (GameObject.Find ("PlanetEarth").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetEarth").name;
			components = GameObject.Find ("PlanetEarth").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==4){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetMars").transform.position + new Vector3 (GameObject.Find ("PlanetMars").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetMars").name;
			components = GameObject.Find ("PlanetMars").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==5){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetJupiter").transform.position + new Vector3 (GameObject.Find ("PlanetJupiter").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetJupiter").name;
			components = GameObject.Find ("PlanetJupiter").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==6){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetSaturn").transform.position + new Vector3 (GameObject.Find ("PlanetSaturn").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetSaturn").name;
			components = GameObject.Find ("PlanetSaturn").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==7){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetUranus").transform.position + new Vector3 (GameObject.Find ("PlanetUranus").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetUranus").name;
			components = GameObject.Find ("PlanetUranus").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==8){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("PlanetNeptune").transform.position+ new Vector3 (GameObject.Find ("PlanetNeptune").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("PlanetNeptune").name;
			components = GameObject.Find ("PlanetNeptune").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==9){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("DwarfPlanetPluto").transform.position + new Vector3 (GameObject.Find ("DwarfPlanetPluto").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("DwarfPlanetPluto").name;
			components = GameObject.Find ("DwarfPlanetPluto").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==10){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("SatelliteMoon").transform.position+ new Vector3 (GameObject.Find ("SatelliteMoon").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("SatelliteMoon").name;
			components = GameObject.Find ("SatelliteMoon").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==11){
			GameObject.Find ("Main Camera").transform.position = GameObject.Find ("CometHalley").transform.position + new Vector3 (GameObject.Find ("CometHalley").transform.localScale.x*2f, 0f, 0);
			PlanetName.text = GameObject.Find ("CometHalley").name;
			components = GameObject.Find ("CometHalley").GetComponent<BezierCurve>();
			PlanetInformation.text ="Major Semi-axis=" + components.a.ToString()+"(UA)\n"+"excentricity="+components.e+"\n"+"longitude of periapsis="+components.omega;
		}
		if(planetNumber==12){
			GameObject.Find ("Main Camera").transform.position = new Vector3(0f, 50f, 0f);
			PlanetName.text = "";
			PlanetInformation.text ="This is the solar system";

		}
		if(rotationController==0){
			if (planetNumber == 12) {
				GameObject.Find ("Main Camera").transform.rotation = new Quaternion (0.718f, 0f, 0, 0.718f);
			} else {
				GameObject.Find ("Main Camera").transform.rotation = new Quaternion (0f, -0.718f, 0, 0.718f);
			}
			rotationController = 1;
		}
		previousPlanetNumber=planetNumber;
	}
	private void GotoButtonSwitcher()
	{
		if (goToSwitch == false) {
			goToSwitch = true;
			for (int i=0; i<12;i = i + 1) {
				button [i].enabled = true;
				button [i].transform.position=button[12].transform.position+new Vector3(0f,(i+1)*30f,0f);
			}
		} else {
			goToSwitch= false;
			for (int i=0; i<12;i = i + 1) {
				button [i].enabled = false;
				button [i].transform.position=button[12].transform.position;
			}
		}
	}
}


