using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	private Vector3 offset;
	public GameObject target;

	// Use this for initialization
	void Start () {
		
		offset = transform.position - target.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = target.transform.position + offset;
	}
}
