using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDInverseKinematicsController : MonoBehaviour {

    [Header("Target")]
    // Target for joint system to reach
    public Transform target;

    // Error at which the solution is considered suitable
    public float minimumError;

    // Limit targets movement?
    public bool limitTargetMovement;

    [Header("Root")]
    // Root of joint system
    public Transform root;

    [Header("Joints")]
    // Array of joints in system
    public JointController[] joints;

    // Amount to change angles by while looking for direction to go in
    public float samplingDistance;

    // Speed of real change of rotations
    public float learningRate;

    [Header("Error Weighting")]
    // How valued distance to target is
    public float distanceWeight;

    // How valued less complex solutions are
    public float complexityWeight;

    // Array of angles of joints
    private float[] _angles;

    // Array of lengths of arms
    private float[] _lenghts;

	// Use this for initialization
	void Start () {

        // Initialise angles and lengths array to number of joints defined
        _angles = new float[joints.Length];
        _lenghts = new float[joints.Length];

        // Fill angles array with 0s as system always starts with no rotation on every joint
        // Fill lengths array with distance between current joint and the previous one
        for (int i = 0; i < joints.Length; i++)
        {
            _angles[i] = 0;

            // Base doesn't have an associated length so skip it
            if (i != 0)
            {
                _lenghts[i] = Vector3.Distance(joints[i - 1].transform.position, joints[i].transform.position);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        // If limited target movement is wanted do it
        if (limitTargetMovement == true)
        {
            LimitTargetMovement();
        }

        // Call function to decide new angles to get closer to target
        InverseKinematics(target.position, _angles, _lenghts);

        // Set every joints rotation to new calculate angles
        SetJointRotations(_angles);
	}

    // Limit the targets movement to within the range of the joint system (used for ragdoll)
    void LimitTargetMovement()
    {
        // Calculate direction to and distance between first joint and the target
        Vector3 directionToTarget = target.position - joints[0].transform.position;
        float distanceToTarget = Vector3.Distance(joints[0].transform.position, target.position);

        // Calculate total possible length of joint system
        float maxLength = 0;
        for (int i = 0; i < _lenghts.Length; i++)
        {
            maxLength += _lenghts[i];
        }

        // If distance to target exceeds max length of joint system bring it within reach
        if (distanceToTarget > maxLength)
        {
            Vector3 newTargetPosition = (directionToTarget.normalized) * maxLength;
            target.position = joints[0].transform.position + newTargetPosition;
        }
    }

    // Set every joints rotation to new calculated angles
    void SetJointRotations(float[] angles)
    {
        // Set joint rotation to new angle
        for (int i = 0; i < joints.Length; i++)
        {
            joints[i].transform.localEulerAngles = joints[i].rotationAxis * angles[i];
        }
    }

    // Fill angles array with new angles to take end of joint system closer to target
    void InverseKinematics(Vector3 target, float[] angles, float[] lengths)
    {
        // Check to see if end of joint system has already reached a suitable solution
        if (ErrorFunction(target, angles, lengths) < minimumError)
        {
            return;
        }

        for (int i = 0; i < joints.Length; i++)
        {
            // Gradient descent
            float gradient = PartialGradient(target, angles, lengths, i);
            angles[i] -= learningRate * gradient;

            // Check to see if end of joint system has already reached a suitable solution
            if (ErrorFunction(target, angles, lengths) < minimumError)
            {
                return;
            }
        }
    }

    // Calculate the direction a joint needs to rotate to move the end of the joint system closer to the target
    float PartialGradient(Vector3 target, float[] angles, float[] lengths, int jointIdentifier)
    {
        // Keep the angle to be restored later
        float angle = angles[jointIdentifier];

        // Gradient calculation
        // Find current error
        float currentError = ErrorFunction(target, angles, lengths);

        // Add to the rotation of the joint and calculate new error
        angles[jointIdentifier] += samplingDistance;
        float newError = ErrorFunction(target, angles, lengths);

        // Calculate direction between two points
        float gradient = (newError - currentError) / samplingDistance;

        // Restore angle
        angles[jointIdentifier] = angle;

        // Return direction the rotation would go in
        return gradient;
    }

    // Calculate distance of the end of the series of joints to the target
    float DistanceFromTarget(Vector3 target, float[] angles)
    {
        // Find end point of joint series based on angles given
        Vector3 point = ForwardKinematics(angles);

        // Return distance between calculated point and target
        return Vector3.Distance(point, target);
    }

    // Calculate normalized distance from target
    float NormalizedDistance(Vector3 target, float[] angles, float[] lengths)
    {
        // Find end point of joint series based on angles given
        Vector3 point = ForwardKinematics(angles);

        // Find distance between calculated point and target
        float distancePenalty = Vector3.Distance(point, target);

        // Find maximum distance from target as total length of all arms in system
        float maximumDistance = 0;
        for (int i = 0; i < lengths.Length; i++)
        {
            maximumDistance += lengths[i];
        }

        return distancePenalty / maximumDistance;
    }

    // Calculate normalized complexity of solution
    float NormalizedComplexity(float[] angles)
    {
        // Find average of all angles (higher average rotation means a more complex solutation) which we want to minimise
        float complexityPenalty = 0;
        for (int i = 0; i < angles.Length; i++)
        {
            complexityPenalty += Mathf.Abs(angles[i]);
        }
        complexityPenalty /= angles.Length;

        // Maximum complexity is equal to the average of every angles maximum rotation
        float maximumComplexity = 360.0f;

        return complexityPenalty / maximumComplexity;
    }

    // Calculate error of the solution for the joints
    float ErrorFunction(Vector3 target, float[] angles, float[] lengths)
    {
        // Find distance error of current solution
        float distancePenalty = NormalizedDistance(target, angles, lengths);

        // Find complexity error of current solution
        float complexityPenalty = NormalizedComplexity(angles);

        // Find total weighted error of current solution
        float error = distancePenalty * distanceWeight + complexityPenalty * complexityWeight;

        return error;
    }

    // Calculates position of the end of a system of joints rotated by the angles passed in
    Vector3 ForwardKinematics (float[] angles)
    {
        // Start with joint at base of system with no rotation
        Vector3 previousPoint = joints[0].transform.position;
        Quaternion rotation = Quaternion.identity;

        // If a root of the system has been declared the initial rotation will be inherited from it
        if (root)
        {
            rotation = root.rotation;
        }

        for (int i = 1; i < joints.Length; i++)
        {
            // Rotate current joint around new axis
            rotation *= Quaternion.AngleAxis(angles[i - 1], joints[i - 1].rotationAxis);

            // Get the position of the next joint in system including its rotation around the previous joint
            Vector3 nextPoint = previousPoint + rotation * joints[i].startPositionOffset;

            // Move onto next joint in system
            previousPoint = nextPoint;
        }

        // Return position of final point in system of joints
        return previousPoint;
    }
}
