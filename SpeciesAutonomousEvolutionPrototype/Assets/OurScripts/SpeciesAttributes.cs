using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesAttributes : MonoBehaviour {
    public int movementRemaining = 300;
    public int hungry = 100;
    public int life = 100;

    // Use this for initialization
    //void Start()
    //{
    //}

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag.Equals("Food") == true)
        {
            hungry = 100;
            target.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (life == 0) { Destroy(gameObject); }
    }
}
