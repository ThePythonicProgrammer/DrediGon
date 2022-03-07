using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public CharacterController controller;
	public Transform Fireball;
	public GameObject topLight;
	public GameObject bottomLight;
	
	public float speed = 4.5f;
	public float jumpHeight = 1.0f;
	public Vector3 movementVector = Vector3.zero;
	
	public bool isGrounded;
	private bool controllerIsGrounded;
	private bool hitAngleIsGrounded;
	public float gravity = 9.81f;
	public float yVel;
	public Vector3 gravityCoef = Vector3.up;
	public Vector3 hitNormal = Vector3.zero;
	private Vector3 hitHorizontal = Vector3.zero;
	private float hitHorizontalMagnitude = 0f;
	public float hitAngle = 0f;
	public float angleLimit = 40f;
	
	public float health = 100f;
	public bool isSleeping = false;
	public float sleepTimer = 0f;
	public float castingDistance = 18f;
	
	// Start is called before the first frame update
	void Start()
	{
		gravity = Physics.gravity.y;
	}
	
	// Update is called once per frame
	void Update()
	{
	  //Grounded
		controllerIsGrounded = controller.isGrounded;
		hitAngleIsGrounded = (hitAngle <= angleLimit);
		isGrounded = (controller.isGrounded && hitAngle <= angleLimit);
		
	  //Gravitational Acceleration
		yVel += gravity*Time.deltaTime;
		//was just vector3.up
		gravityCoef = gravityCoef*(1f-2f*Time.deltaTime)+Vector3.up*2f*Time.deltaTime;
		if(isGrounded)
		{
			yVel = Mathf.Max(yVel, -Mathf.Tan(Mathf.Min(hitAngle,angleLimit)*Mathf.Deg2Rad)*speed);
			gravityCoef = Vector3.up;
		}
		else
		{
			if(controller.isGrounded)
			{
				//There's GOTTA be a way to get the perpendicular normal more simply
				gravityCoef = new Vector3(hitHorizontal.x*Mathf.Cos(Mathf.Acos(hitHorizontalMagnitude)+Mathf.PI/2f),Mathf.Sin(Mathf.Asin(hitNormal.y)+Mathf.PI/2f),hitHorizontal.z*Mathf.Cos(Mathf.Acos(hitHorizontalMagnitude)+Mathf.PI/2f));
			}
		}
		
	  //sleeping
		//error logging
		if(sleepTimer < 0f)
			Debug.Log("(Player Movement) ERROR: sleepTimer was a negative value. Has been set to 0.");
		sleepTimer = Mathf.Clamp(sleepTimer - Time.deltaTime, 0f, Mathf.Infinity); 
		isSleeping = (sleepTimer > 0f);
		
	  //Movement Input
		movementVector = Vector3.ClampMagnitude(transform.right*Input.GetAxis("Horizontal") + transform.forward*Input.GetAxis("Vertical"),1f);
		if(isSleeping)
			movementVector = Vector3.zero;
		if(Input.GetButtonDown("Jump") && isGrounded)
			yVel = Mathf.Sqrt(jumpHeight * -2f * gravity);
		
	  //Knockback Test
		if(Input.GetKeyDown(KeyCode.X))
		{
			yVel = -20f;
			gravityCoef = -Vector3.Normalize(new Vector3(1f,1f,0f));
			//knockback normal must be opposite direction to intended path (made negative). yVel must also be negative for initial velocity
			//if knockback feels too fast, the gravityCoef dampening should be reduced
		}
		
	  //APPLY MOVEMENT
		controller.Move((movementVector*speed + gravityCoef*yVel) * Time.deltaTime);
		
	  //FIIIIREBAALLLL!!!
		castingDistance = Mathf.Clamp(castingDistance + Input.mouseScrollDelta.y, 1f, 35f);
		topLight.transform.localPosition = Vector3.forward * castingDistance + Vector3.up * 100f;
		bottomLight.transform.localPosition = Vector3.forward * castingDistance + Vector3.up * -100f;
		if(Input.GetMouseButtonDown(1))
		{
			//This must stay inside the curly brackets
			Transform fireball = GameObject.Instantiate(Fireball, transform.position, transform.rotation);
		}
		
	  //Debug player path
		Debug.DrawRay(transform.position, Vector3.Normalize(gravityCoef), Color.blue, 60);
		
		
	  //Independent Error Logging
		if((transform.localRotation.x != 0f) || (transform.localRotation.z != 0f))
			Debug.Log("(Player Movement) ERROR: X and/or Z Rot is not 0");
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		hitNormal = hit.normal;
		hitHorizontal = Vector3.Normalize(new Vector3(hitNormal.x,0f,hitNormal.z));
		hitHorizontalMagnitude = new Vector3(hitNormal.x, 0f, hitNormal.z).magnitude;
		hitAngle = Mathf.Atan(hitHorizontalMagnitude/hitNormal.y)*Mathf.Rad2Deg;
	}
}
