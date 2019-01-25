using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDTargetController : MonoBehaviour {

    // When this object is clicked on and dragged
    void OnMouseDrag ()
    {
        // Find position of mouse on screen
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        float z = 10;

        // Translate point to world space
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(new Vector3(x, y, z));

        // Set new position
        gameObject.transform.position = mousePositionInWorld;
    }
}
