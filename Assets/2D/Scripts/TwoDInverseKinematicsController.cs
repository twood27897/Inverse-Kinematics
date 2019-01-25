using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDInverseKinematicsController : MonoBehaviour {

    [Header("Joints")]
    public Transform joint0;
    public Transform joint1;
    public Transform hand;

    [Header("Target")]
    public Transform target;

    private float length0;
    private float length1;

	// Use this for initialization
	void Start () {

        // Set length of bones
        length0 = length1 = 3;

	}
	
	// Update is called once per frame
	void Update () {

        SolveInverseKinematics();
        RenderArm();

	}

    // Find solution for system of joints so the hand is as close to the target as possible
    void SolveInverseKinematics()
    {
        // Angle to store joint rotations
        float jointAngle0;
        float jointAngle1;

        // Calculate distance between first joint and target (hypotinuse of triangle formed by joints)
        float length2 = Vector2.Distance(joint0.position, target.position);

        // Angle from first joint and target
        Vector2 difference = target.position - joint0.position;
        float atan = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Is the target in range
        if (length0 + length1 < length2)
        {
            jointAngle0 = atan;
            jointAngle1 = 0;
        }
        else
        {
            // Find required internal angles of triangle
            float cosAngle0 = ((length2 * length2) + (length0 * length0) - (length1 * length1)) / (2 * length2 * length0);
            float angle0 = Mathf.Acos(cosAngle0) * Mathf.Rad2Deg;

            float cosAngle1 = ((length1 * length1) + (length0 * length0) - (length2 * length2)) / (2 * length1 * length0);
            float angle1 = Mathf.Acos(cosAngle1) * Mathf.Rad2Deg;

            // Adjust to work with Unity angles
            jointAngle0 = atan - angle0;
            jointAngle1 = 180.0f - angle1;
        }

        // Set angle for joint 1
        Vector3 Euler0 = joint0.localEulerAngles;
        Euler0.z = jointAngle0;
        joint0.transform.localEulerAngles = Euler0;

        // Set angle for joint 2
        Vector3 Euler1 = joint1.localEulerAngles;
        Euler1.z = jointAngle1;
        joint1.transform.localEulerAngles = Euler1;
    }

    // Render lines between points
    void RenderArm()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, joint0.position);
            lineRenderer.SetPosition(1, joint1.position);
            lineRenderer.SetPosition(2, hand.position);
        }
    }
}
