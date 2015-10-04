using UnityEngine;
using System.Collections;

public class GroundFireBehaviour : MonoBehaviour {

	Animator anim;
	GameObject iori;

	// Use this for initialization
	void Start () {
	
	anim = GameObject.Find ("Iori").GetComponent<Animator>();
	iori = GameObject.Find ("Iori");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			anim.SetTrigger ("GetHit");
			iori.GetComponent<Rigidbody2D>().AddForce (new Vector2 (-500,300));
			
			Destroy(gameObject);			
		}
		else
			GetComponent<Rigidbody2D>().velocity = - GetComponent<Rigidbody2D>().velocity;
		
	}
}
