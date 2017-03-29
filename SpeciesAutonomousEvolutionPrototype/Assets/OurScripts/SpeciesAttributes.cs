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

    // Update is called once per frame
    void Update()
    {
        if (life == 0) { Destroy(gameObject); }
    }
}
