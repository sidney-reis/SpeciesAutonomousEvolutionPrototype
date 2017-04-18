using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 zeroRotation;
        zeroRotation.x = 0;
        zeroRotation.y = 0;
        zeroRotation.z = 0;
        gameObject.transform.rotation = Quaternion.Euler(zeroRotation);
    }
}
