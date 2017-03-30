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
    public static List<RespawningFood> foodRespawning = new List<RespawningFood>();

    void Start()
    {
        //Debug.Log(foodRespawning);
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("foodresp count"+foodRespawning.Count);
        //Debug.Log(""foodRespawning)
        //Debug.Log("active: " + gameObject.activeSelf);
		if(foodRespawning.Count > 0)
        {
            for(int i = foodRespawning.Count-1; i >= 0; i--)
            {
                foodRespawning[i].respawningTime--;
                if(foodRespawning[i].respawningTime == 0)
                {
                    foodRespawning[i].food.SetActive(true);
                    foodRespawning.Remove(foodRespawning[i]);
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
