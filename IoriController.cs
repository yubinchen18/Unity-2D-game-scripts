using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IoriController : MonoBehaviour {

	public float maxSpeed;
	bool facingRight = true;
	public float currentSpeed;
	public float jumpForce = 300f;
	public Rigidbody2D character;
	public float runSpeed = 12.0f;
	public float walkSpeed = 6.0f;
	public GameObject projectile;
	public GameObject fireTrail;
	
	Animator anim;
	BoxCollider2D colliderRef;
	
	bool grounded = false;
	bool ducking = false;
	bool firing = false;
	bool gettingHit = false;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	
	// Key Register System Variables
	public string keyHistory;
	public float keyHistoryTimer = 0.5f;
	public float currentTime;
	public float lastKeyTime;
	public string currentDirection = "";

	// Use this for initialization
	void Start () 
	{
		maxSpeed = runSpeed;
		character = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		colliderRef = GetComponent<BoxCollider2D>();
		keyHistory = "";
	}
		
		// Update is called once per frame
	void FixedUpdate () 
	{
		grounded = Physics2D.OverlapCircle(new Vector2(groundCheck.transform.position.x, groundCheck.transform.position.y), 0.1f);
		//grounded = Physics2D.OverlapArea(new Vector2(groundCheck.position.x - GetComponent<SpriteRenderer>().bounds.extents.x, groundCheck.position.y) , new Vector2 (groundCheck.position.x + GetComponent<SpriteRenderer>().bounds.extents.x, groundCheck.position.y + 0.1f) ,whatIsGround);
		anim.SetBool ("Ground", grounded);
	
		anim.SetFloat ("vSpeed",GetComponent<Rigidbody2D>().velocity.y);
	
		float move = Input.GetAxis("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (GetComponent<Rigidbody2D>().velocity.x));
		
		if (!ducking && !firing && !gettingHit)
		GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		
		if(move >0 && ! facingRight && !gettingHit)
			Flip ();
		else if(move < 0 && facingRight && !gettingHit)
			Flip ();
		
		currentSpeed = character.velocity.x;
		
		
		//key actions
		if ((keyHistory.Contains ("RnR") && grounded && !firing) || (keyHistory.Contains ("LnL") && grounded && !firing))
		{
			anim.SetTrigger ("DashForward");
			maxSpeed = runSpeed;
			keyHistory = "";
		}
			
		if ((keyHistory.Contains ("D2RF") && grounded) || (keyHistory.Contains ("D2FR") && grounded))
		{
			anim.SetTrigger ("GroundFire");
			keyHistory = "";
			
		}	
			
		if (move == 0 || ducking)
			maxSpeed = walkSpeed;	
		
		if(Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow))
		{
			anim.SetBool ("ForwardIdle", true);
		}
		
		if(Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.LeftArrow))
			anim.SetBool ("ForwardIdle", false);
		
		
		if(grounded && !ducking && !firing && !gettingHit && Input.GetButtonDown("Jump"))
		{
			anim.SetTrigger ("Jumping");
			anim.SetBool ("Ground", false);
			GetComponent<Rigidbody2D>().AddForce (new Vector2(0, jumpForce));
		}
		
		if(Input.GetKeyDown (KeyCode.T))
			anim.SetTrigger ("Taunt");
		
		if(!firing && !gettingHit && Input.GetKeyDown (KeyCode.DownArrow))
		{
			anim.SetTrigger ("Duck");
			ducking = true;
		}
		
		if(Input.GetKey (KeyCode.DownArrow))
			anim.SetBool ("DuckIdle", true);
		
		if (Input.GetKeyUp (KeyCode.DownArrow))
		{
			anim.SetBool ("DuckIdle", false);
			anim.SetTrigger ("GetUp");
			ducking = false;
		}	
		
	}
	
	void Update()
	{
		currentTime = Time.time;
		
		colliderRef.size = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x,GetComponent<SpriteRenderer>().bounds.size.y);
		colliderRef.offset = new Vector2(0,GetComponent<SpriteRenderer>().bounds.size.y / 2);
	
	
	//DIRECTION REGISTER CODE	
		if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.DownArrow))
		{
			currentDirection = "L";
			lastKeyTime = Time.time;
		}	
		if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.LeftArrow))
		{	
			currentDirection = "1";
			lastKeyTime = Time.time;
		}
		if (Input.GetKey (KeyCode.DownArrow) && !Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow))
		{	
			currentDirection = "D";
			lastKeyTime = Time.time;
		}
		if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.RightArrow))
		{	
			currentDirection = "2";
			lastKeyTime = Time.time;
		}
		if (Input.GetKey (KeyCode.RightArrow) && !Input.GetKey (KeyCode.DownArrow))
		{	
			currentDirection = "R";
			lastKeyTime = Time.time;
		}
		if (Input.GetKey (KeyCode.UpArrow))
		{	
			currentDirection = "U";
			lastKeyTime = Time.time;
		}
		
		if (Input.GetKeyDown (KeyCode.F))
		{
			currentDirection = "F";
			lastKeyTime = Time.time;
		}
		
		//needs modi later on
		if (!Input.anyKey)
		{	
			currentDirection = "n";
			anim.SetTrigger ("NeutralPosition");
		}
		
		if (keyHistory == "" || currentDirection != keyHistory.Substring(keyHistory.Length-1))
			keyHistory = keyHistory + currentDirection;
		
		if ((currentTime - lastKeyTime) >= keyHistoryTimer && !Input.anyKey)
			keyHistory = "";
		
		
		//Debug.Log ("key is: " + keyHistory);
		
		
	// key actions
		
		
		
		//test key
		if (Input.GetKeyDown (KeyCode.Q))
			anim.SetTrigger ("GroundFire");
		
	}
	
	void Flip()
	{
		//float theExtents = GetComponent<SpriteRenderer>().bounds.extents.x;
		//Vector3 thePosition = new Vector3 (theExtents * 2,0,0);
		
		//if (facingRight)
			//transform.position += thePosition;
		//else
			//transform.position -= thePosition;
			
		
		facingRight = ! facingRight;
		Vector3 theScale = transform.localScale;
		

		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void FireGroundFire()
	{
		GameObject clone;
		clone = Instantiate(projectile, transform.position + new Vector3 (GetComponent<SpriteRenderer>().bounds.extents.x + projectile.GetComponent<SpriteRenderer>().bounds.extents.x + 0.5f,0,0),transform.rotation) as GameObject;
		clone.GetComponent<Rigidbody2D>().velocity = new Vector2(20,0);
	}
	
	public void HandFireTrail()
	{
		GameObject clone0;
		clone0 = Instantiate(fireTrail, transform.position + new Vector3 (-0.5f, 0.7f, 0), transform.rotation) as GameObject;
	}
	
	public void SetFiringStatus()
	{
		if (!firing)
		{
			firing = true;
			Debug.Log (firing);
			return;
		}
		if (firing)
		{	
			firing = false;
			Debug.Log (firing);
		}
	}
	
	public void SetGettingHitStatus(int state0)
	{
		if (state0 == 2)
		{
			ResetEverything ();
		}
		else
		{	
			gettingHit = true;
			anim.SetBool ("GettingHit", true);
			Debug.Log ("get hit");
		}
	}	
	
	public void ResetEverything()
	{
		grounded = false;
		ducking = false;
		firing = false;
		gettingHit = false;
		anim.SetBool ("GettingHit", false);
	}
}
