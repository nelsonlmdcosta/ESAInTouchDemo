using UnityEngine;
using System.Collections;

public class SateliteWalker : MonoBehaviour {
	public BezierCurve orbit;
	private int k;
	float currentSpeed;
	// Use this for initialization
	void Start () {
		orbit.GenerateCurve();
		transform.position = orbit.points [0];
		currentSpeed = orbit.velocity [0];
	}

	// Update is called once per frame
	void Update () {
		currentSpeed= orbit.velocity [k];
		transform.position = Vector3.MoveTowards(transform.position,orbit.points[k]+GameObject.Find("PlanetEarth").transform.position,currentSpeed*Time.deltaTime);
		if(transform.position==GameObject.Find("PlanetEarth").transform.position+orbit.points[k])
		{
			if(k==99)
			{k=0;}
			else
			{k++;}	
		}
	}
}

