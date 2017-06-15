using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCombatHandler : MonoBehaviour {
    public GameObject approachingEnemy;
    public List<GameObject> ignoredEnemy;
    private GameObject counterBackground;

    void Start () {
        counterBackground = GameObject.Find("CounterBackground");
    }
	
	void Update () {
		if(approachingEnemy)
        {
            if (!ignoredEnemy.Contains(approachingEnemy))
            {
                counterBackground.SetActive(true);
            }
        }
        else
        {
            counterBackground.SetActive(false);
        }
    }
}
