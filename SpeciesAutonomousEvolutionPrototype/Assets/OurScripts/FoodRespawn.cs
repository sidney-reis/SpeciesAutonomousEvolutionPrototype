using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningFood
{
    public GameObject food;
    public int respawningTime;
}

public class FoodRespawn : MonoBehaviour {
    public static List<RespawningFood> foodRespawning = new List<RespawningFood>();

    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
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
        }
	}
}
