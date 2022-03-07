using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public GameObject playerBody;
	
	public float castingDistance;
	public float speed = 22f;
	private Vector3 startingPos;
	public float travel = 0f;
	
	// Start is called before the first frame update
	void Start()
	{
		playerBody = GameObject.Find("Player");
		PlayerMovement PlayerMovement = playerBody.GetComponent<PlayerMovement>();
		castingDistance = PlayerMovement.castingDistance;
		transform.position = playerBody.transform.position;
		startingPos = playerBody.transform.position;
		travel = 0f;
	}

	// Update is called once per frame
	void Update()
	{
		transform.Translate(0f, 0f, speed * Time.deltaTime);
		float translate = Vector3.Distance(transform.position, startingPos);
		//float yPos = (-Mathf.Pow(translate,2f)+(castingDistance*translate))/(0.75f*castingDistance);
		//transform.position = new Vector3(transform.position.x, yPos+startingPos.y, translate+startingPos.z);
		travel += speed * Time.deltaTime;
		transform.position = new Vector3(transform.position.x, ((-Mathf.Pow(travel,2f)+(castingDistance*travel))/(0.75f*castingDistance) + startingPos.y), transform.position.z);
		
		if(transform.position.y < 0)
		{
			Debug.Log("Fireball destroyed below Y=0");
			Destroy(transform.gameObject);
		}
	}
}
