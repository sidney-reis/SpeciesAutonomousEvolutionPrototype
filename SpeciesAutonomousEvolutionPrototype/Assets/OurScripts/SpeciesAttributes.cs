using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesAttributes : MonoBehaviour {
    public static int MAX_MOVEMENT = 3000;

    [HideInInspector]
    public int movementRemaining;
    public int movementUpgrade = 0;
    public int perceptionUpgrade = 0;
    public int perceptionRay = 9999900;
    public int hungry = 300;
    public int life = 100;
    public bool dying = false;
    public int libido = 0;
    
    void Start ()
    {
        movementRemaining = MAX_MOVEMENT;
    }

    void Update()
    {
        if (life == 0 && !dying)
        {
            StartCoroutine(deathRoutine());
        }
    }

    private IEnumerator deathRoutine()
    {
        dying = true;
        if(gameObject.GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
        }

        string playerSpecies = PlayerInfo.selectedSpecies.ToString();
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("species_"+playerSpecies+ "_death");
        Destroy(gameObject.GetComponent<Animator>());
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
        PlayerInfo.playerCreaturesCount--;
        dying = false;
    }
}
