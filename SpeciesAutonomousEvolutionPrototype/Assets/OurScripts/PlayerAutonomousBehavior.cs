using Pathfinding;
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
    bool huntingFood = false;
    bool foundFood = false;
    Vector3 destination;
    float walkSide = 0;
    float walkUp = 0;
    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    void Start () {
        anim = GetComponent<Animator>();
        attributes = GetComponent<SpeciesAttributes>();
        character = int.Parse(gameObject.name);
        float randomStart = Random.value * 3;
        InvokeRepeating("WanderOrStay", randomStart, 3);
        InvokeRepeating("HuntForFood", randomStart, 1);
        agent = gameObject.GetComponent<NavMeshAgent>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
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
        if (attributes.hungry < 280 && !foundFood)
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
                    var seeker = GetComponent<Seeker>();

                    agent.enabled = true;
                    obstacle.enabled = false;

                    anim.SetBool("walking", true);
                    agent.SetDestination(closestObject.transform.position);
                }
            }
        }
        else if(foundFood && attributes.hungry > 280)
        {
            foundFood = false;
            huntingFood = false;
            anim.SetBool("walking", false);
            agent.enabled = false;
            obstacle.enabled = true;
            Wander();
        }
        else if(foundFood && closestObject == null)
        {
            foundFood = false;
        }
        else if(foundFood && closestObject.activeSelf == false)
        {
            foundFood = false;
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
