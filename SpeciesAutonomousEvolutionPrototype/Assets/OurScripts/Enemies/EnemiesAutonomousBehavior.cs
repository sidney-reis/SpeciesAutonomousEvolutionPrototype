using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesAutonomousBehavior : MonoBehaviour
{
    private Animator anim;
    public EnemiesAttributes attributes;
    public int character;
    public bool isWalking = false;
    public bool resting = false;
    public bool fastResting = true;
    public bool huntingFood = false;
    public bool foundFood = false;
    public bool counterAttacked = false;
    public int hitByEnemy = 0;
    public GameObject enemyCreatureHit;
    public Vector3 destination;
    public float walkSide = 0;
    public float walkUp = 0;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public SpriteRenderer sprite;
    public GameObject closestObject;
    public int species;
    public bool lockAttack = false;
    public GameObject enemyRunningFrom;
    public int cancelTimeout = 0;
    public int attackTimes = 0;
    public Prey closestEnemy;
    public List<Prey> enemies = new List<Prey>();


    void Start()
    {
        anim = GetComponent<Animator>();
        attributes = GetComponent<EnemiesAttributes>();
        character = int.Parse(gameObject.name);
        float randomStart = Random.value * 3;
        InvokeRepeating("WanderOrStay", randomStart, 3);
        InvokeRepeating("HuntForFood", randomStart, 1);
        InvokeRepeating("FightCreatures", randomStart, 1);
        InvokeRepeating("DefendOrRun", randomStart, 1);
        agent = gameObject.GetComponent<NavMeshAgent>();
        obstacle = gameObject.GetComponent<NavMeshObstacle>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        species = gameObject.transform.parent.GetComponent<EnemiesGlobalAttributes>().species;

        StartCoroutine(SetAgentOffset(1));
    }

    IEnumerator SetAgentOffset(float time)
    {
        yield return new WaitForSeconds(time);
        agent.baseOffset = (sprite.bounds.size.y) / 10;
    }

    void WanderOrStay()
    {
        if (!attributes.dying && !isWalking && !resting && !huntingFood && attackTimes == 0)
        {
            float randomValue = Random.value;
            if (randomValue < 0.7)
            {
                Wander();
            }
        }
        else if (!attributes.dying && resting && !huntingFood && attackTimes == 0)
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

    void HuntForFood()
    {
        if (attributes.hungry < 150 && !fastResting && !attributes.dying)
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
                    creatureHunting = (int)(obj.GetComponent<FoodMarks>().speciesHunting[species]);
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
                    closestObject.GetComponent<FoodMarks>().speciesHunting[species] = character;
                    destination = closestObject.transform.position;
                    foundFood = true;

                    attackTimes = 0;
                    stopAttack();

                    obstacle.enabled = false;
                    agent.enabled = true;
                    agent.speed = (6.0f + attributes.movementUpgrade * 3.5f) * GameConstants.movementSpeed;

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
                closestObject.GetComponent<FoodMarks>().speciesHunting[species] = -1;
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
        if (!attributes.dying && !resting && !fastResting && !foundFood && attackTimes == 0)
        {
            List<GameObject> enemiesObj = new List<GameObject>();
            enemiesObj.AddRange(GameObject.FindGameObjectsWithTag("EnemySpecies").Where(x => x.transform.parent.name != gameObject.transform.parent.name));
            enemiesObj.AddRange(GameObject.FindGameObjectsWithTag("ControllableSpecies"));

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
                if (Vector3.Distance(transform.position, closestEnemy.obj.transform.position) <= 125)
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
                            if (sameEnemyIndex >= 0)
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
                cancelTimeout++;
                stopAttack();
            }
            else if (Vector3.Distance(gameObject.transform.position, closestEnemy.obj.transform.position) <= 12.0)
            {
                if (closestEnemy.obj.tag == "ControllableSpecies" && closestEnemy.obj.name == PlayerInfo.selectedCreature.ToString() && GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy == null)
                {
                    GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy = gameObject;
                }

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

            if (((attributes.movementUpgrade == 2 && randomRunAttack > 90) ||
                (attributes.movementUpgrade == 1 && randomRunAttack > 75) ||
                (attributes.deffenseUpgrade == 0 && attributes.movementUpgrade == 0 && randomRunAttack > 50) ||
                (attributes.deffenseUpgrade == 1 && randomRunAttack > 25) ||
                (attributes.deffenseUpgrade == 2 && randomRunAttack > 10) ||
                (attributes.deffenseUpgrade == 3)) &&
                (enemyRunningFrom == null || enemyRunningFrom != enemyCreatureHit))
            {
                attackTimes = 0;
                stopAttack();
                isWalking = false;

                if (enemyCreatureHit)
                {
                    attackEnemy(enemyCreatureHit);
                    hitByEnemy--;
                }
            }
            else if (!fastResting)
            {
                if (Vector3.Distance(gameObject.transform.position, enemyCreatureHit.transform.position) < 125)
                {
                    enemyRunningFrom = enemyCreatureHit;
                    Vector3 diffPosition = gameObject.transform.position - enemyCreatureHit.transform.position;
                    Vector3 positionToRaycast = gameObject.transform.position;
                    Vector3 positionToRun = gameObject.transform.position;

                    for (int i = 4; i <= 1; i--)
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
                        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(positionToRaycast), out hitInfo);
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
                        agent.speed = (6.0f + attributes.movementUpgrade * 3.5f) * GameConstants.movementSpeed;

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
    }

    private void goToEnemy()
    {
        obstacle.enabled = false;
        agent.enabled = true;
        agent.speed = (6.0f + attributes.movementUpgrade * 3.5f) * GameConstants.movementSpeed;

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
        Sprite angrySpriteRender = Resources.Load<Sprite>(gameObject.transform.parent.GetComponent<EnemiesGlobalAttributes>().species + "-atk");
        angrySpriteRenderer.sprite = angrySpriteRender;
        Vector3 angryPosition;
        angryPosition.x = gameObject.transform.position.x + gameObject.GetComponent<BoxCollider>().size.x + 1;
        angryPosition.y = gameObject.transform.position.y + gameObject.GetComponent<BoxCollider>().size.y + 1;
        angryPosition.z = gameObject.transform.position.z;
        angrySprite.transform.position = angryPosition;


        if (enemyOb.tag == "ControllableSpecies")
        {
            enemyOb.GetComponent<PlayerAutonomousBehavior>().hitByEnemy++;
            enemyOb.GetComponent<PlayerAutonomousBehavior>().enemyCreatureHit = gameObject;
            int damageDealt = 5 + 5 * attributes.attackUpgrade - 5 * enemyOb.GetComponent<SpeciesAttributes>().deffenseUpgrade;
            if (damageDealt < 0)
            {
                damageDealt = 0;
            }
            enemyOb.GetComponent<SpeciesAttributes>().life -= damageDealt;
        }
        else
        {
            enemyOb.GetComponent<EnemiesAutonomousBehavior>().hitByEnemy++;
            enemyOb.GetComponent<EnemiesAutonomousBehavior>().enemyCreatureHit = gameObject;
            int damageDealt = 5 + 5 * attributes.attackUpgrade - 5 * enemyOb.GetComponent<EnemiesAttributes>().deffenseUpgrade;
            if (damageDealt < 0)
            {
                damageDealt = 0;
            }
            enemyOb.GetComponent<EnemiesAttributes>().life -= damageDealt;
        }

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
        if (attackTimes == 0)
        {
            cancelTimeout++;
            stopAttack();
        }
        lockAttack = false;

        if(GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy == gameObject)
        {
            StartCoroutine(RemoveFromApproachingEnemy());
        }

        if (closestEnemy.obj.tag == "ControllableSpecies")
        {
            closestEnemy.obj.GetComponent<PlayerAutonomousBehavior>().hitByEnemy--;
        }
        else
        {
            closestEnemy.obj.GetComponent<EnemiesAutonomousBehavior>().hitByEnemy--;
        }
    }

    IEnumerator RemoveFromApproachingEnemy()
    {
        if (GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy == gameObject)
        {
            yield return new WaitForSeconds(3);
            GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy = null;

            if (!counterAttacked)
            {
                PlayerModel.CurrentModel.ran++;
            }
        }
        counterAttacked = false;
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
        if (anim)
        {
            anim.SetBool("walking", true);
        }
    }

    void Update()
    {
        if (attackTimes < 0)
        {
            attackTimes = 0;
        }
        if (hitByEnemy < 0)
        {
            hitByEnemy = 0;
        }
        if (!isWalking && !agent.enabled)
        {
            if (attributes.movementRemaining < SpeciesAttributes.MAX_MOVEMENT)
            {
                attributes.movementRemaining++;
            }
        }

        if (isWalking && attributes.movementRemaining < SpeciesAttributes.MAX_MOVEMENT / 2)
        {
            resting = true;
        }

        if (resting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT / 1.07f))
        {
            resting = false;
        }

        if (attributes.movementRemaining == 0)
        {
            fastResting = true;
            if (anim)
            {
                anim.SetBool("walking", false);
            }
            if (agent.enabled == true)
            {
                agent.Stop();
                agent.enabled = false;
            }
            obstacle.enabled = true;
        }

        if (fastResting && attributes.movementRemaining > Mathf.Round(SpeciesAttributes.MAX_MOVEMENT / 3.0f))
        {
            fastResting = false;

        }

        if (agent.enabled && agent.velocity.x >= 0)
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
        if (target.gameObject.tag != "Terrain")
        {
            isWalking = false;
            if (!huntingFood && !foundFood && anim)
            {
                anim.SetBool("walking", false);
            }
        }
    }
}
