﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutonomousBehavior : MonoBehaviour {
    private Animator anim;
    SpeciesAttributes attributes;
    int character;
    bool isWalking = false;
    bool resting = false;
    Vector3 destination;
    float walkSide = 0;
    float walkUp = 0;

    void Start () {
        anim = GetComponent<Animator>();
        attributes = GetComponent<SpeciesAttributes>();
        character = int.Parse(gameObject.name);
        float randomStart = Random.value * 3;
        InvokeRepeating("WanderOrStay", randomStart, 3);
    }

    void WanderOrStay ()
    {
        if (!attributes.dying && !isWalking && character != PlayerInfo.selectedCreature && !resting)
        {
            float randomValue = Random.value;
            Debug.Log(randomValue);
            if (randomValue < 0.7)
            {
                Vector3 randomCircle = Random.insideUnitCircle * 20;
                Debug.Log(randomCircle);

                if (randomCircle.x < 0 && randomCircle.x > -10.0f)
                {
                    randomCircle.x = -10.0f;
                    Debug.Log("x virou -10");
                }
                else if (randomCircle.x > 0 && randomCircle.x < 10.0f)
                {
                    randomCircle.x = 10.0f;
                    Debug.Log("x virou 10");
                }

                if (randomCircle.y < 0 && randomCircle.y > -10.0f)
                {
                    randomCircle.y = -10.0f;
                    Debug.Log("y virou -10");
                }
                else if (randomCircle.y > 0 && randomCircle.y < 10.0f)
                {
                    randomCircle.y = 10.0f;
                    Debug.Log("y virou 10");
                }

                destination = new Vector3(gameObject.transform.position.x + randomCircle.x, gameObject.transform.position.y, gameObject.transform.position.z + randomCircle.y);

                if (destination.x > gameObject.transform.position.x)
                {
                    walkSide = 0.1f;
                    SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                    sprite.flipX = false;
                }
                else if (destination.x < gameObject.transform.position.x)
                {
                    walkSide = -0.1f;
                    SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
                    sprite.flipX = true;
                }

                if (destination.z > gameObject.transform.position.z)
                {
                    walkUp = 0.1f;
                }
                else if (destination.z < gameObject.transform.position.z)
                {
                    walkUp = -0.1f;
                }

                isWalking = true;
                anim.SetBool("walking", true);
            }
        }
    }

    void Update ()
    {
        if(character == PlayerInfo.selectedCreature)
        {
            isWalking = false;
        }
        else if(!isWalking)
        {
            if (attributes.movementRemaining < SpeciesAttributes.MAX_MOVEMENT)
            {
                attributes.movementRemaining++;
            }
        }

        if(isWalking && attributes.movementRemaining < SpeciesAttributes.MAX_MOVEMENT/2)
        {
            isWalking = false;
            resting = true;
            anim.SetBool("walking", false);
        }

        if(resting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT/1.07f))
        {
            resting = false;
        }

        if(isWalking)
        {
            if ((walkSide == -0.1f) && (gameObject.transform.position.x <= destination.x))
            {
                walkSide = 0.0f;
            }
            else if ((walkSide == 0.1f) && (gameObject.transform.position.x >= destination.x))
            {
                walkSide = 0.0f;
            }

            if ((walkUp == -0.1f) && (gameObject.transform.position.z <= destination.z))
            {
                walkUp = 0.0f;
            }
            else if ((walkUp == 0.1f) && (gameObject.transform.position.z >= destination.z))
            {
                walkUp = 0.0f;
            }

            if ((walkUp == 0.0f) && (walkSide == 0.0f))
            {
                isWalking = false;
                anim.SetBool("walking", false);
            }
            else
            {
                Vector3 newPosition = gameObject.transform.position;
                newPosition.x = newPosition.x + walkSide;
                newPosition.z = newPosition.z + walkUp;
                gameObject.transform.position = newPosition;
                attributes.movementRemaining--;
            }
            Debug.Log("walkup: " + walkUp + " walkside: " + walkSide);
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.tag != "Terrain")
        {
            isWalking = false;
            anim.SetBool("walking", false);
            Debug.Log("colidiu com " + target.gameObject.tag);
        }
    }
}
