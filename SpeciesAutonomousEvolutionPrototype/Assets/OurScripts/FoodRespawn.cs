using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRespawn : MonoBehaviour {
    int respawnTime = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("active: " + gameObject.activeSelf);
		if(gameObject.activeSelf == false)
        {
            Debug.Log("sumiu "+respawnTime);
            respawnTime--;
            if(respawnTime == 0)
            {
                Debug.Log("respawnou");
                gameObject.SetActive(true);
                respawnTime = 100;
            }
        }
	}
}
