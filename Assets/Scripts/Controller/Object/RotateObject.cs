using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    [SerializeField] private float angler_Velocity;


	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, angler_Velocity);
	}
}
