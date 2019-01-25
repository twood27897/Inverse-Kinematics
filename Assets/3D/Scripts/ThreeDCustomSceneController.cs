using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDCustomSceneController : MonoBehaviour {

    // Joint System to be instantiated
    public GameObject jointSystem;

    // Target for joint system to be aimed at
    public GameObject target;

    // Create new joint system at the origin
    public void CreateJointSystem()
    {
        ThreeDInverseKinematicsController newJointSystem = Instantiate(jointSystem).GetComponent<ThreeDInverseKinematicsController>();
        newJointSystem.target = target.transform;
    }
}
