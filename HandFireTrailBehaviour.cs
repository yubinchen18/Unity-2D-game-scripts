using UnityEngine;
using System.Collections;

public class HandFireTrailBehaviour : MonoBehaviour {

	public float duration = 0.8f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		duration -= Time.deltaTime;
		if (duration <= 0)
			Destroy (gameObject);
	}
}
