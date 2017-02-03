using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    public float speed = 1;

    private Camera cameraMain;

	void Start ()
	{
	    cameraMain = transform.FindChild("WebVRCameraSet/CameraMain").GetComponent<Camera>();
	}
	
	void Update ()
	{
	    float movementX = Input.GetAxis("Horizontal");
	    float movementY = Input.GetAxis("Vertical");

	    Vector3 movement = cameraMain.transform.forward * movementY * speed * Time.deltaTime;
	    movement += cameraMain.transform.right * movementX * speed * Time.deltaTime;

        transform.Translate(movement, Space.World);
	}
}
