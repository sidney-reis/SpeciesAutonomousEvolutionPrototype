using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Prey
{
    public GameObject obj;
    public bool attackable;

    public Prey(GameObject ob, bool attack)
    {
        obj = ob;
        attackable = attack;
    }
}

public class PlayerAutonomousBehavior : MonoBehaviour {
    private Animator anim;
    public SpeciesAttributes attributes;
    public int character;
    public bool isWalking = false;
    public bool resting = false;
    public bool fastResting = true;
    public bool huntingFood = false;
    public bool foundFood = false;
    public bool lockAttack = false;
    public bool running = false;
    public int hitByEnemy = 0;
    public GameObject enemyCreatureHit;
    public GameObject enemyRunningFrom;
    public int cancelTimeout = 0;
    public int attackTimes = 0;
    public Vector3 destination;
    public float walkSide = 0;
    public float walkUp = 0;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public SpriteRenderer sprite;
    public GameObject closestObject;
    public Prey closestEnemy;
    public List<Prey> enemies = new List<Prey>();
    public List<GameObject> attackingEnemies = new List<GameObject>();

    void Start () {
        anim = GetComponent<Animator>();
        attributes = GetComponent<SpeciesAttributes>();
        character = int.Parse(gameObject.name);
        float randomStart = Random.value * 3;
        InvokeRepeating("WanderOrStay", randomStart, 3);
        InvokeRepeating("HuntForFood", randomStart, 1);
        InvokeRepeating("FightCreatures", randomStart, 1);
        InvokeRepeating("DefendOrRun", randomStart, 0.5f);
        agent = gameObject.GetComponent<NavMeshAgent>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        sprite = gameObject.GetComponent<SpriteRenderer>();

        StartCoroutine(SetAgentOffset(1));
    }

    IEnumerator SetAgentOffset(float time)
    {
        yield return new WaitForSeconds(time);
        agent.baseOffset = (sprite.bounds.size.y) / 10;
    }

    void WanderOrStay ()
    {
        if (!attributes.dying && !isWalking && character != PlayerInfo.selectedCreature && !resting && !huntingFood && attackTimes == 0)
        {
            float randomValue = Random.value;
            if (randomValue < 0.7)
            {
                Wander();
            }
        }
        else if(!attributes.dying && character != PlayerInfo.selectedCreature && resting && !huntingFood && attackTimes == 0)
        {
            if (anim)
            {
                anim.SetBool("walking", false);
            }
            isWalking = false;
        }
    }

    void Wander()
    {
        if (attributes.movementRemaining > 0)
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
    }
    
    void HuntForFood ()
    {
        if (attributes.hungry < 150 && !fastResting && character != PlayerInfo.selectedCreature && !attributes.dying)
        {
            huntingFood = true;
            List<GameObject> foodObjects = new List<GameObject>();
            foodObjects.AddRange(GameObject.FindGameObjectsWithTag("Food"));
            foodObjects.AddRange(GameObject.FindGameObjectsWithTag("RandomFood"));

            closestObject = null;
            int creatureHunting;
            foreach (GameObject obj in foodObjects)
            {
                if (obj)
                {
                    creatureHunting = (int)(obj.GetComponent<FoodMarks>().speciesHunting[PlayerInfo.selectedSpecies]);
                    if ((creatureHunting == -1 || creatureHunting == character) && (closestObject == null))
                    {
                        closestObject = obj;
                    }
                    else if ((creatureHunting == -1 || creatureHunting == character) && (Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestObject.transform.position)))
                    {
                        closestObject = obj;
                    }
                }
            }

            if (closestObject == null && !fastResting && attackTimes == 0)
            {
                Wander();
            }
            else if (closestObject != null)
            {
                if (Vector3.Distance(transform.position, closestObject.transform.position) <= (attributes.perceptionRay + attributes.perceptionRay * attributes.perceptionUpgrade))
                {
                    closestObject.GetComponent<FoodMarks>().speciesHunting[PlayerInfo.selectedSpecies] = character;
                    destination = closestObject.transform.position;
                    foundFood = true;

                    attackTimes = 0;
                    stopAttack();

                    obstacle.enabled = false;
                    agent.enabled = true;
                    agent.speed = (6.0f + attributes.movementUpgrade * 3) * GameConstants.movementSpeed;

                    if (anim)
                    {
                        anim.SetBool("walking", true);
                    }
                    agent.SetDestination(closestObject.transform.position);
                }
                else if (!fastResting && attackTimes == 0)
                {
                    Wander();
                }
            }
        }
        else if (foundFood && closestObject == null)
        {
            foundFood = false;
        }
        else if (foundFood && closestObject.activeSelf == false)
        {
            foundFood = false;
        }

        if ((attributes.hungry >= 150) && (huntingFood == true))
        {
            if (closestObject != null)
            {
                closestObject.GetComponent<FoodMarks>().speciesHunting[PlayerInfo.selectedSpecies] = -1;
            }
            foundFood = false;
            huntingFood = false;
            if (anim)
            {
                anim.SetBool("walking", false);
            }
            if(agent.enabled == true)
            {
                agent.Stop();
                agent.enabled = false;
            }
            obstacle.enabled = true;
            Wander();
        }
    }

    void FightCreatures()
    {
        if (!attributes.dying && character != PlayerInfo.selectedCreature && !resting && !fastResting && !foundFood && attackTimes == 0)
        {
            List<GameObject> enemiesObj = new List<GameObject>();
            enemiesObj.AddRange(GameObject.FindGameObjectsWithTag("EnemySpecies"));

            closestEnemy = null;
            foreach (GameObject obj in enemiesObj)
            {
                Prey enemy = new Prey(obj, true);
                int sameEnemyIndex = enemies.FindIndex(x => x.obj == enemy.obj);

                if (sameEnemyIndex == -1)
                {
                    enemies.Add(enemy);
                }

                if (closestEnemy == null)
                {
                    if (sameEnemyIndex == -1)
                    {
                        closestEnemy = enemy;
                    }
                    else if (enemies[sameEnemyIndex].attackable)
                    {
                        closestEnemy = enemy;
                    }
                }
                else if (Vector3.Distance(transform.position, obj.transform.position) <= Vector3.Distance(transform.position, closestEnemy.obj.transform.position))
                {
                    if (sameEnemyIndex == -1)
                    {
                        closestEnemy = enemy;
                    }
                    else if (enemies[sameEnemyIndex].attackable)
                    {
                        closestEnemy = enemy;
                    }
                }
            }

            if (closestEnemy != null)
            {
                if (Vector3.Distance(transform.position, closestEnemy.obj.transform.position) <= 100)
                {
                    float randomChance = Random.value * 100;
                    if (attributes.movementUpgrade == 1)
                    {
                        if (randomChance >= 90)
                        {
                            isWalking = false;
                            goToEnemy();
                            attackTimes = 3;
                            StartCoroutine(TimeoutAttack());
                        }
                        else
                        {
                            int sameEnemyIndex = enemies.FindIndex(x => x.obj == closestEnemy.obj);
                            if (sameEnemyIndex >= 0)
                            {
                                enemies[sameEnemyIndex].attackable = false;
                            }
                            StartCoroutine(EnableAttackingEnemy(closestEnemy));
                        }
                    }
                    else if (attributes.attackUpgrade == 0 && attributes.movementUpgrade == 0)
                    {
                        if (randomChance >= 85)
                        {
                            isWalking = false;
                            goToEnemy();
                            attackTimes = 3;
                            StartCoroutine(TimeoutAttack());
                        }
                        else
                        {
                            int sameEnemyIndex = enemies.FindIndex(x => x.obj == closestEnemy.obj);
                            if(sameEnemyIndex >= 0)
                            {
                                enemies[sameEnemyIndex].attackable = false;
                            }
                            StartCoroutine(EnableAttackingEnemy(closestEnemy));
                        }
                    }
                    else if (attributes.attackUpgrade == 1)
                    {
                        if (randomChance >= 50)
                        {
                            isWalking = false;
                            goToEnemy();
                            attackTimes = 3;
                            StartCoroutine(TimeoutAttack());
                        }
                        else
                        {
                            int sameEnemyIndex = enemies.FindIndex(x => x.obj == closestEnemy.obj);
                            if (sameEnemyIndex >= 0)
                            {
                                enemies[sameEnemyIndex].attackable = false;
                            }
                            StartCoroutine(EnableAttackingEnemy(closestEnemy));
                        }
                    }
                    else if (attributes.attackUpgrade == 2)
                    {
                        if (randomChance >= 25)
                        {
                            isWalking = false;
                            goToEnemy();
                            attackTimes = 3;
                            StartCoroutine(TimeoutAttack());
                        }
                        else
                        {
                            int sameEnemyIndex = enemies.FindIndex(x => x.obj == closestEnemy.obj);
                            if (sameEnemyIndex >= 0)
                            {
                                enemies[sameEnemyIndex].attackable = false;
                            }
                            StartCoroutine(EnableAttackingEnemy(closestEnemy));
                        }
                    }
                }
            }
        }
        else if (!attributes.dying && character != PlayerInfo.selectedCreature && !fastResting && !foundFood && attackTimes > 0)
        {
            if (!closestEnemy.obj)
            {
                attackTimes = 0;
                stopAttack();
            }
            else if (Vector3.Distance(gameObject.transform.position, closestEnemy.obj.transform.position) <= 12.0)
            {
                if (agent.enabled == true)
                {
                    agent.Stop();
                    agent.enabled = false;
                }
                obstacle.enabled = true;
                if (anim)
                {
                    anim.SetBool("walking", false);
                }
                if (!lockAttack)
                {
                    attackEnemy(closestEnemy.obj);
                }
            }
            else
            {
                goToEnemy();
            }
        }
    }

    void DefendOrRun()
    {
        if (!attributes.dying && character != PlayerInfo.selectedCreature && !foundFood && hitByEnemy > 0)
        {
            float randomRunAttack = Random.value * 100;

            if ((((attributes.movementUpgrade == 1 && randomRunAttack > 75) ||
                (attributes.deffenseUpgrade == 0 && attributes.movementUpgrade == 0 && randomRunAttack > 50) ||
                (attributes.deffenseUpgrade == 1 && randomRunAttack > 25) ||
                (attributes.deffenseUpgrade == 2)) || attackingEnemies.Contains(enemyCreatureHit)) && 
                (enemyRunningFrom == null || enemyRunningFrom != enemyCreatureHit))
            {
                attackTimes = 0;
                stopAttack();
                isWalking = false;

                if (enemyCreatureHit)
                {
                    if (!attackingEnemies.Contains(enemyCreatureHit))
                    {
                        attackingEnemies.Add(enemyCreatureHit);
                        StartCoroutine(RemoveAttackingEnemy(enemyCreatureHit));
                    }
                    attackEnemy(enemyCreatureHit);
                    hitByEnemy--;
                }
            }
            else if (!fastResting && !attackingEnemies.Contains(enemyCreatureHit))
            {
                if (Vector3.Distance(gameObject.transform.position, enemyCreatureHit.transform.position) < 45)
                {
                    running = true;
                    enemyRunningFrom = enemyCreatureHit;
                    Vector3 diffPosition = gameObject.transform.position - enemyCreatureHit.transform.position;
                    Vector3 positionToRaycast = gameObject.transform.position;
                    Vector3 positionToRun = gameObject.transform.position;

                    for (int i = 4; i >= 1; i--)
                    {
                        positionToRaycast.x = gameObject.transform.position.x + 6 * i;
                        if (diffPosition.x < 0)
                        {
                            positionToRaycast.x = gameObject.transform.position.x - 6 * i;
                        }

                        positionToRaycast.z = gameObject.transform.position.z + 6 * i;
                        if (diffPosition.z < 0)
                        {
                            positionToRaycast.z = gameObject.transform.position.z - 6 * i;
                        }

                        RaycastHit hitInfo = new RaycastHit();
                        bool hit = Physics.Raycast(positionToRaycast, Vector3.down, out hitInfo);
                        if (hit)
                        {
                            if (hitInfo.transform.gameObject.tag == "Terrain")
                            {
                                positionToRun = positionToRaycast;
                            }
                        }
                    }

                    if (positionToRun != gameObject.transform.position)
                    {
                        obstacle.enabled = false;
                        agent.enabled = true;
                        agent.speed = (6.0f + attributes.movementUpgrade * 3) * GameConstants.movementSpeed;

                        if (anim)
                        {
                            anim.SetBool("walking", true);
                        }
                        agent.SetDestination(positionToRun);
                    }
                    else
                    {
                        Wander();
                    }
                }
                else
                {
                    enemyRunningFrom = null;
                    running = false;
                    hitByEnemy--;
                    if (agent.enabled == true)
                    {
                        agent.Stop();
                        agent.enabled = false;
                    }
                    obstacle.enabled = true;
                    if (anim)
                    {
                        anim.SetBool("walking", false);
                    }
                }
            }
        }
        else if (running)
        {
            if (agent.enabled == true)
            {
                agent.Stop();
                agent.enabled = false;
            }
            obstacle.enabled = true;
            if (anim)
            {
                anim.SetBool("walking", false);
            }
            running = false;
        }
    }

    IEnumerator RemoveAttackingEnemy(GameObject enemyCreatureHit)
    {
        yield return new WaitForSeconds(10);
        attackingEnemies.Remove(enemyCreatureHit);
    }

    private void goToEnemy()
    {
        obstacle.enabled = false;
        agent.enabled = true;
        agent.speed = (6.0f + attributes.movementUpgrade * 3) * GameConstants.movementSpeed;

        if (anim)
        {
            anim.SetBool("walking", true);
        }
        agent.SetDestination(closestEnemy.obj.transform.position);
    }

    private void attackEnemy(GameObject enemyOb)
    {
        lockAttack = true;
        GameObject attackSprite = new GameObject("AttackSprite");
        SpriteRenderer spriteRenderer = attackSprite.AddComponent<SpriteRenderer>();
        Sprite cloudSprite = Resources.Load<Sprite>("cloud-normal");
        spriteRenderer.sprite = cloudSprite;
        attackSprite.transform.position = enemyOb.transform.position;

        GameObject angrySprite = new GameObject("AngrySprite");
        SpriteRenderer angrySpriteRenderer = angrySprite.AddComponent<SpriteRenderer>();
        Sprite angrySpriteRender = Resources.Load<Sprite>(PlayerInfo.selectedSpecies.ToString() + "-atk");
        angrySpriteRenderer.sprite = angrySpriteRender;
        Vector3 angryPosition;
        angryPosition.x = gameObject.transform.position.x + gameObject.GetComponent<BoxCollider>().size.x + 1;
        angryPosition.y = gameObject.transform.position.y + gameObject.GetComponent<BoxCollider>().size.y + 1;
        angryPosition.z = gameObject.transform.position.z;
        angrySprite.transform.position = angryPosition;

        enemyOb.GetComponent<EnemiesAutonomousBehavior>().hitByEnemy++;
        enemyOb.GetComponent<EnemiesAutonomousBehavior>().enemyCreatureHit = gameObject;

        int damageDealt = 5 + 5 * attributes.attackUpgrade - 5 * enemyOb.GetComponent<EnemiesAttributes>().deffenseUpgrade;
        if (damageDealt < 0)
        {
            damageDealt = 0;
        }
        enemyOb.GetComponent<EnemiesAttributes>().life -= damageDealt;

        StartCoroutine(FlashCloud(attackSprite));
        StartCoroutine(FinishAttack(attackSprite, angrySprite));
    }

    IEnumerator FlashCloud(GameObject attackSprite)
    {
        yield return new WaitForSeconds(0.333f);
        SpriteRenderer spriteRenderer = attackSprite.GetComponent<SpriteRenderer>();
        Sprite cloudSprite = Resources.Load<Sprite>("cloud-flash");
        spriteRenderer.sprite = cloudSprite;
        yield return new WaitForSeconds(0.333f);
        cloudSprite = Resources.Load<Sprite>("cloud-normal");
        spriteRenderer.sprite = cloudSprite;
    }

    IEnumerator FinishAttack(GameObject attackSprite, GameObject angrySprite)
    {
        yield return new WaitForSeconds(1);
        Destroy(attackSprite);
        Destroy(angrySprite);
        attackTimes--;
        if(attackTimes == 0)
        {
            stopAttack();
        }
        lockAttack = false;
        if (closestEnemy != null)
        {
            if (closestEnemy.obj)
            {
                closestEnemy.obj.GetComponent<EnemiesAutonomousBehavior>().hitByEnemy--;
            }
        }
    }
    
    IEnumerator TimeoutAttack()
    {
        yield return new WaitForSeconds(20);
        if (cancelTimeout == 0)
        {
            attackTimes = 0;
            stopAttack();
        }
        else
        {
            cancelTimeout--;
        }
    }

    IEnumerator EnableAttackingEnemy(Prey enemy)
    {
        yield return new WaitForSeconds(20);

        int sameEnemyIndex = enemies.FindIndex(x => x.obj == enemy.obj);
        if (sameEnemyIndex >= 0)
        {
            enemies[sameEnemyIndex].attackable = true;
        }
    }

    private void stopAttack()
    {
        if (agent.enabled == true)
        {
            agent.Stop();
            agent.enabled = false;
        }
        obstacle.enabled = true;
        if (anim)
        {
            anim.SetBool("walking", false);
        }

        if(closestEnemy != null) {
            int sameEnemyIndex = enemies.FindIndex(x => x.obj == closestEnemy.obj);
            if (sameEnemyIndex >= 0)
            {
                enemies[sameEnemyIndex].attackable = false;
            }

            if (closestEnemy.obj)
            {
                StartCoroutine(EnableAttackingEnemy(closestEnemy));
            }
            else if (sameEnemyIndex >= 0)
            {
                enemies.RemoveAt(sameEnemyIndex);
            }
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

        if (!running)
        {
            isWalking = true;
        }
        if(anim)
        {
            anim.SetBool("walking", true);
        }
    }

    void Update ()
    {
        if(attackTimes < 0)
        {
            attackTimes = 0;
        }
        if(hitByEnemy < 0)
        {
            hitByEnemy = 0;
        }
        if(character == PlayerInfo.selectedCreature)
        {
            isWalking = false;
            foundFood = false;
            huntingFood = false;
            attackTimes = 0;
            hitByEnemy = 0;

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
            resting = true;
        }

        if(resting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT/1.07f))
        {
            resting = false;
        }

        if(attributes.movementRemaining <= 0)
        {
            fastResting = true;
            if (anim)
            {
                anim.SetBool("walking", false);
            }
            if(agent.enabled == true)
            {
                agent.Stop();
                agent.enabled = false;
            }
            obstacle.enabled = true;
        }

        if(fastResting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT / 3.0f))
        {
            fastResting = false;
        }

        if(agent.enabled && agent.velocity.x >= 0)
        {
            sprite.flipX = false;
            attributes.movementRemaining--;
        }
        else if (agent.enabled && agent.velocity.x < 0)
        {
            sprite.flipX = true;
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
                if (anim)
                {
                    anim.SetBool("walking", false);
                }
            }
            else
            {
                Vector3 newPosition = gameObject.transform.position;
                newPosition.x = newPosition.x + walkSide * GameConstants.movementSpeed + (walkSide * GameConstants.movementSpeed / 2 * attributes.movementUpgrade);
                newPosition.z = newPosition.z + walkUp * GameConstants.movementSpeed + (walkUp * GameConstants.movementSpeed / 2 * attributes.movementUpgrade);
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
            if(!huntingFood && !foundFood && attackTimes < 1 && anim)
            {
                anim.SetBool("walking", false);
            }
        }
    }
}
