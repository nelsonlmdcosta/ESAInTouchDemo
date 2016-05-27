using UnityEngine;
using System.Collections;

public class SimulationController : MonoBehaviour
{ 
    private static SimulationController instance;
    public static SimulationController Instance { get { return instance; } }

    [SerializeField] [Range(-2, 2)]private float timeModifier = 1;

    [SerializeField] private float minRange = 0;
    [SerializeField] private float maxRange = 0;

    public float TimeModifier
    {
        get
        {
            return timeModifier;
        }

        set
        {
            float val = Mathf.Abs(minRange) + Mathf.Abs(maxRange);
            float mid = val / 2;
            val = (val * value) - mid;

            timeModifier = val;
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
