using UnityEngine;
using System.Collections;

public class TrailRend : MonoBehaviour {
	public bool satelliteStatus;
	private GameObject thisObject;
	private TrailRenderer trail;
	public bool red;
	public bool blue;
	public bool green;
	private Color color;

	// Use this for initialization
	void Start () {
		thisObject = gameObject;
		if (satelliteStatus == true) {
			thisObject.AddComponent<TrailRenderer>();
			trail=thisObject.GetComponent<TrailRenderer>();
			trail.startWidth = gameObject.transform.localScale.x/3;
			trail.endWidth = 0;
			trail.time =3f;
			trail.material = Resources.Load("TrailRenderMat", typeof(Material)) as Material;;
			if (red == true) {
				color = new Vector4 (1, 0, 0, 1);
				trail.material.SetColor("_EmissionColor", color);
				trail.material.SetColor("_Color", color);
			}
			if (green == true) {
				color = new Vector4 (0, 1, 0, 1);
				trail.material.SetColor("_EmissionColor", color);
				trail.material.SetColor("_Color", color);

			}
			if (blue == true) {
				color = new Vector4 ( 0, 0, 1,1);
				trail.material.SetColor("_EmissionColor", color);
				trail.material.SetColor("_Color", color);

			}
		}
	}

}