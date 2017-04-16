using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesAttributes : MonoBehaviour {
    public int movementRemaining = 300;
    public int hungry = 100;
    public int life = 100;
    public bool dying = false;
    public int libido = 0;

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
