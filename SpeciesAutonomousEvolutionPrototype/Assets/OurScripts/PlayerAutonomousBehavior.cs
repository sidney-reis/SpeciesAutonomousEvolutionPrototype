using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAutonomousBehavior : MonoBehaviour {
    private Animator anim;
    SpeciesAttributes attributes;
    int character;
    bool isWalking = false;
    bool resting = false;
    bool fastResting = true;
    bool huntingFood = false;
    bool foundFood = false;
    Vector3 destination;
    float walkSide = 0;
    float walkUp = 0;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;
    SpriteRenderer sprite;

    void Start () {
        anim = GetComponent<Animator>();
        attributes = GetComponent<SpeciesAttributes>();
        character = int.Parse(gameObject.name);
        float randomStart = Random.value * 3;
        InvokeRepeating("WanderOrStay", randomStart, 3);
        InvokeRepeating("HuntForFood", randomStart, 1);
        agent = gameObject.GetComponent<NavMeshAgent>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        Debug.Log("sprite size: " + sprite.bounds.size);

        StartCoroutine(SetAgentOffset(1));
    }

    IEnumerator SetAgentOffset(float time)
    {
        yield return new WaitForSeconds(time);
        agent.baseOffset = (sprite.bounds.size.y) / 10;
    }

    void WanderOrStay ()
    {
        if (!attributes.dying && !isWalking && character != PlayerInfo.selectedCreature && !resting && !huntingFood)
        {
            float randomValue = Random.value;
            if (randomValue < 0.7)
            {
                Wander();
            }
        }
    }

    void Wander()
    {
        Vector3 randomCircle = Random.insideUnitCircle * 20;

        if (randomCircle.x < 0 && randomCircle.x > -10.0f)
        {
            randomCircle.x = -10.0f;
        }
        else if (randomCircle.x > 0 && randomCircle.x < 10.0f)
        {
            randomCircle.x = 10.0f;
        }

        if (randomCircle.y < 0 && randomCircle.y > -10.0f)
        {
            randomCircle.y = -10.0f;
        }
        else if (randomCircle.y > 0 && randomCircle.y < 10.0f)
        {
            randomCircle.y = 10.0f;
        }

        destination = new Vector3(gameObject.transform.position.x + randomCircle.x, gameObject.transform.position.y, gameObject.transform.position.z + randomCircle.y);
        setWalk();
    }
    
    void HuntForFood ()
    {
        GameObject closestObject = null;
        if (attributes.hungry < 280 && !foundFood && !fastResting && character != PlayerInfo.selectedCreature)
        {
            huntingFood = true;
            if (!attributes.dying && !isWalking && character != PlayerInfo.selectedCreature && !resting)
            {
                List<GameObject> foodObjects = new List<GameObject>();
                foodObjects.AddRange(GameObject.FindGameObjectsWithTag("Food"));
                foodObjects.AddRange(GameObject.FindGameObjectsWithTag("RandomFood"));

                closestObject = foodObjects[0];
                foreach (GameObject obj in foodObjects)
                {
                    if (Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestObject.transform.position))
                    {
                        closestObject = obj;
                    }
                }
                if (Vector3.Distance(transform.position, closestObject.transform.position) <= attributes.perceptionRay)
                {
                    destination = closestObject.transform.position;
                    foundFood = true;

                    agent.enabled = true;
                    agent.speed = 6.0f + attributes.movementUpgrade * 3.5f;
                    obstacle.enabled = false;

                    anim.SetBool("walking", true);

                    if (character == 6)
                    {
                        Debug.Log("comida proxima: "+closestObject.transform.name+" posicao: "+closestObject.transform.position);
                    }
                    agent.SetDestination(closestObject.transform.position);
                }
            }
        }
        //else if(foundFood && attributes.hungry >= 280)
        //{
        //    foundFood = false;
        //    huntingFood = false;
        //    anim.SetBool("walking", false);
        //    agent.enabled = false;
        //    obstacle.enabled = true;
        //    Wander();
        //}
        else if(foundFood && closestObject == null)
        {
            foundFood = false;
        }
        else if(foundFood && closestObject.activeSelf == false)
        {
            foundFood = false;
        }

        if((attributes.hungry >= 280) && (huntingFood == true))
        {
            foundFood = false;
            huntingFood = false;
            anim.SetBool("walking", false);
            agent.Stop();
            agent.enabled = false;
            obstacle.enabled = true;
            Wander();
        }
    }
    
    private void setWalk()
    {
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

    void Update ()
    {
        if(character == PlayerInfo.selectedCreature)
        {
            isWalking = false;
            foundFood = false;
            huntingFood = false;
            if (agent.enabled == true)
            {
                agent.Stop();
                agent.enabled = false;
            }
            obstacle.enabled = true;
        }
        else if(!isWalking && !agent.enabled)
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

        if(attributes.movementRemaining == 0)
        {
            fastResting = true;
            agent.Stop();
            agent.enabled = false;
            obstacle.enabled = true;
        }

        if(fastResting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT / 3.0f))
        {
            fastResting = false;
            anim.SetBool("walking", false);
        }

        if (character == 6)
        {
            Debug.Log("Character: " + character + " / " + agent.enabled);
        }
        if(agent.enabled && agent.velocity.x >= 0)
        {
            sprite.flipX = false;
            if (character == 6)
            {
                Debug.Log("Character: " + character + " / entrou");
                Debug.Log("Character: " + character + " / velocidade: " + agent.velocity);
            }
            attributes.movementRemaining--;
        }
        else if (agent.enabled && agent.velocity.x < 0)
        {
            sprite.flipX = true;
            if (character == 6)
            {
                Debug.Log("Character: " + character + " / entrou");
                Debug.Log("Character: " + character + " / velocidade: " + agent.velocity);
            }
            attributes.movementRemaining--;
        }

        if (isWalking)
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
                //attributes.movementRemaining--;
            }
        }
    }

    void OnCollisionEnter(Collision target)
    {
        if(target.gameObject.tag != "Terrain")
        {
            isWalking = false;
            if(!huntingFood && !foundFood)
            {
                anim.SetBool("walking", false);
            }
        }
    }
}
