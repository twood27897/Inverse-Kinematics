using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Speed of rotation
    public float cameraRotateSpeed;

    // Speed of zoom
    public float cameraZoomSpeed;

    private float x;
    private float y;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        CameraRotation();
        CameraZoom();
	}

    // Zoom camera in and out with scroll
    void CameraZoom()
    {
        // Scroll data
        float scrollInput = Input.mouseScrollDelta.y;

        // Check for muse scroll
        if (scrollInput != 0)
        {
            // Find vector from camera to centre
            Transform cameraRotationCentre = Camera.main.transform.parent;
            Vector3 vectorToCentre = cameraRotationCentre.position - Camera.main.transform.position;

            // Check to make sure zoom is allowed 
            if ((vectorToCentre.magnitude > 1 && scrollInput > 0) || (vectorToCentre.magnitude < 10 && scrollInput < 0))
            {
                // Normalize the value
                Vector3 directionToCentre = vectorToCentre.normalized;

                // Mouse scroll is returned as 1 or -1 depending on direction
                // Multiply the direction to centre by the scroll direction
                // And then by the speed
                Vector3 positionChange = directionToCentre * scrollInput * cameraZoomSpeed;

                // Find the sum of the camera position and change to be made
                Vector3 newPosition = gameObject.transform.position + positionChange;
                Vector3 newPositionToCentre = cameraRotationCentre.position - newPosition;

                if (newPositionToCentre.magnitude > 1)
                {
                    gameObject.transform.position = newPosition;
                }
            }
        }
    }

    // Rotate camera around rotation centre with right mouse button
    void CameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            // Get rotation centre
            Transform cameraRotationCentre = Camera.main.transform.parent;

            // Take input from mouse
            // x input is used for y rotation (and vice versa) as horizontal rotation is for x 
            float xRotation = Input.GetAxis("Mouse Y") * cameraRotateSpeed;
            float yRotation = -Input.GetAxis("Mouse X") * cameraRotateSpeed;

            // Check is made to see if rotation is at a pole to stop jittering
            float currentX = cameraRotationCentre.eulerAngles.x;
            if ((currentX < 88.0f || currentX > 272.0f) || (currentX > 260.0f && xRotation > 0) || (currentX < 100.0f && xRotation < 0))
            {
                cameraRotationCentre.Rotate(new Vector3(xRotation, yRotation, 0));
                x = cameraRotationCentre.eulerAngles.x;
                y = cameraRotationCentre.eulerAngles.y;
                cameraRotationCentre.rotation = Quaternion.Euler(x, y, 0);
            }
        }
    }
}
