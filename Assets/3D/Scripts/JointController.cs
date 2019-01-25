using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointController : MonoBehaviour {

    // Axis of rotation of joint
    public Vector3 rotationAxis;

    // Offset of joint
    [HideInInspector]
    public Vector3 startPositionOffset;

	// Use this for initialization
	void Awake () {
        startPositionOffset = transform.localPosition;
	}
}
