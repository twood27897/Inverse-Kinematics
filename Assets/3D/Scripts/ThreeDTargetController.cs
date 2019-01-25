using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDTargetController : MonoBehaviour {

    // Default object to move
    public Transform defaultObjectToMove;

    // Scalar for movement speed of target
    public float movementSpeed;

    // Object last clicked on
    Transform _objectToMove;

	// Use this for initialization
	void Start () {

        // Set default object to move
        _objectToMove = defaultObjectToMove;

        // Darken colour of default controlled object
        _objectToMove.gameObject.GetComponent<MeshRenderer>().material.color += new Color(0.25f, 0.25f, 0.25f, 0.0f);

    }
	
	// Update is called once per frame
	void Update () {

        // Check for player clicking to change controlled object
        CheckForClickOnNewObject();

        // Move target based on player input
        MoveTarget();

	}

    // On mouse click check for a new object to move
    void CheckForClickOnNewObject()
    {
        // If player hits left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the mouse position into the scene
            Ray rayFromMouse = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Check to see if the ray hit anything
            RaycastHit hitObjectFromRay;
            Physics.Raycast(rayFromMouse, out hitObjectFromRay);

            // If it did, check the tag 
            if (hitObjectFromRay.transform)
            {
                if (hitObjectFromRay.transform.tag == "Moveable")
                {
                    // Darken colour of old controlled object
                    _objectToMove.gameObject.GetComponent<MeshRenderer>().material.color -= new Color(0.25f, 0.25f, 0.25f, 0.0f);

                    // Set as new transform to be moved
                    _objectToMove = hitObjectFromRay.transform;

                    // Lighten colour of new controlled object
                    _objectToMove.gameObject.GetComponent<MeshRenderer>().material.color += new Color(0.25f, 0.25f, 0.25f, 0.0f);
                }
            }
        }
    }

    // Move target based on inputs
    void MoveTarget()
    {
        // Move in x axis
        XAxisControl();

        // Move in y axis
        YAxisControl();

        // Move in z axis
        ZAxisControl();
    }

    // Controls of the targets movements in the given axis
    // X Axis
    void XAxisControl()
    {
        // D Key moves target positiviley
        if (Input.GetKey(KeyCode.D))
        {
            _objectToMove.position += new Vector3(1.0f * movementSpeed * Time.deltaTime, 0, 0);
        }

        // A Key moves target negatively
        if (Input.GetKey(KeyCode.A))
        {
            _objectToMove.position -= new Vector3(1.0f * movementSpeed * Time.deltaTime, 0, 0);
        }
    }

    // Y Axis
    void YAxisControl()
    {
        // W Key moves target positiviley
        if (Input.GetKey(KeyCode.W))
        {
            _objectToMove.position += new Vector3(0, 1.0f * movementSpeed * Time.deltaTime, 0);
        }                                     
                                              
        // S Key moves target negatively      
        if (Input.GetKey(KeyCode.S))          
        {
            _objectToMove.position -= new Vector3(0, 1.0f * movementSpeed * Time.deltaTime, 0);
        }
    }

    // Z Axis
    void ZAxisControl()
    {
        // Q Key moves target positiviley
        if (Input.GetKey(KeyCode.Q))
        {
            _objectToMove.position += new Vector3(0, 0, 1.0f * movementSpeed * Time.deltaTime);
        }

        // E Key moves target negatively
        if (Input.GetKey(KeyCode.E))
        {
            _objectToMove.position -= new Vector3(0, 0, 1.0f * movementSpeed * Time.deltaTime);
        }
    }
}
