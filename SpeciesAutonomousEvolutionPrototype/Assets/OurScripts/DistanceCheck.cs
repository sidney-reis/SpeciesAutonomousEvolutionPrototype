using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCheck : MonoBehaviour {
    public GameObject go1;
    public GameObject go2;
    
    void Start () {
        Debug.Log(Vector3.Distance(go1.transform.position, go2.transform.position));
	}

    void Update () {
		
	}
}
