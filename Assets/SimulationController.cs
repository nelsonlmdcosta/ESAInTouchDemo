using UnityEngine;
using System.Collections;

public class SimulationController : MonoBehaviour
{ 
	private static SimulationController instance;
	public static SimulationController Instance { get { return instance; } }

	private float timeModifier = 1;

	public float TimeModifier
	{
		get
		{
			return timeModifier;
		}

		set
		{
			timeModifier = value;
		}
	}

	private void Start ()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}else instance = this;
	}
}