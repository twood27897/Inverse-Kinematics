using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBasedMovementController : MonoBehaviour {

    // Transform to be moved by this object
    public Transform objectToMove;

    // Direction to move the object
    public Vector3 directionToMove;

    // Is the mouse currently clicking on this object
    private bool clickedOn;

	// Use this for initialization
	void Start () {

        // To start mouse is not clicking on object
        clickedOn = false;

    }
	
	// Update is called once per frame
	void Update () {

        // If currently being clicked on, move the object
        if (clickedOn == true)
        {
            Move();
        }

	}

    // Move object 
    void Move()
    {
        objectToMove.position += Vector3.zero;
    }

    // When the attached object is clicked on
    void OnMouseButtonDown()
    {
        clickedOn = true;
    }
}
