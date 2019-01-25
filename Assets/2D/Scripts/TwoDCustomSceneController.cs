using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDCustomSceneController : MonoBehaviour {

    // Joint System to be instantiated
    public GameObject jointSystem;

    // Target for joint system to be aimed at
    public GameObject target;

    // Create new joint system at the origin
    public void CreateJointSystem()
    {
        TwoDInverseKinematicsController newJointSystem = Instantiate(jointSystem).GetComponent<TwoDInverseKinematicsController>();
        newJointSystem.target = target.transform;
    }
}
