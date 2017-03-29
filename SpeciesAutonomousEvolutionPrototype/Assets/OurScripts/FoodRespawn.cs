using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningFood
{
    public GameObject food;
    public int respawningTime;
}

public class FoodRespawn : MonoBehaviour {
    //int respawnTime = 100;
    // Use this for initialization
    public ArrayList foodRespawning = new ArrayList();

	//void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("active: " + gameObject.activeSelf);
		if(foodRespawning.Count > 1)
        {
            foreach(RespawningFood food in foodRespawning)
            {
                food.respawningTime--;
                if(food.respawningTime == 0)
                {
                    food.food.SetActive(true);
                    foodRespawning.Remove(food);
                }
            }
            //Debug.Log("sumiu "+respawnTime);
            //respawnTime--;
            //if(respawnTime == 0)
            //{
            //    Debug.Log("respawnou");
            //    gameObject.SetActive(true);
            //    respawnTime = 100;
            //}
        }
	}
}
