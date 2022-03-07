using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
	public GameObject playerBody;
	public PlayerMovement playerMovement;
	
	public float mouseSens = 720f;
	public bool aerialView = false;
	public float aerialHeight = 100f;
	public float groundHeight = 0.85f;
	public float swapTime = 2f;
	
	private float xRot = 0f;
	private float xRotCache;
	private float startTime;
	public float startPos;
	public float endPos;
	
	//MatrixBlender
	private Matrix4x4   ortho,
						perspective;
	public float		fov	 = 90f,
						near	= .01f,
						far	 = 1000f,
						orthographicSize = 100f;
	private float aspect;
	public MatrixBlender blender;
	
	
	// Start is called before the first frame update
	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		
		//MatrixBlender
		aspect = (float) Screen.width / (float) Screen.height;
		ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
		perspective = Matrix4x4.Perspective(fov, aspect, near, far);
		GetComponent<Camera>().projectionMatrix = perspective;
		blender = (MatrixBlender) GetComponent(typeof(MatrixBlender));
		startPos = groundHeight;
		endPos = groundHeight;
	}
	
	// Update is called once per frame
	void Update()
	{
		
		if(!aerialView && !playerMovement.isSleeping)
		{
			float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
			xRot -= mouseY;
			xRot = Mathf.Clamp(xRot, -90f, 90f);
			playerBody.transform.Rotate(Vector3.up * mouseX);
		}
		transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
		
		if(Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab))
		{
			startTime = Time.time;
			startPos = transform.localPosition.y;
			aerialView = !aerialView;
			if(aerialView)
			{
				xRotCache = xRot;
				xRot = 90f;
				Cursor.lockState = CursorLockMode.Confined;
				endPos = aerialHeight;
				blender.BlendToMatrix(ortho, swapTime);
			}
			else
			{
				xRot = xRotCache;
				Cursor.lockState = CursorLockMode.Locked;
				endPos = groundHeight;
				blender.BlendToMatrix(perspective, swapTime);
			}
		}
		float t = (Time.time - startTime) / swapTime;
		transform.localPosition = Vector3.up * Mathf.Lerp(startPos, endPos, t);
	}
}
