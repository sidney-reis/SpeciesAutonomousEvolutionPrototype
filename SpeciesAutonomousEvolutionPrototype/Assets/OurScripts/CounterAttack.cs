using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : MonoBehaviour {
    public void CounterAttackClick()
    {
        if (!GameObject.Find("MenuCanvas").GetComponent<Attack>().lockAttack)
        {
            GameObject.Find("MenuCanvas").GetComponent<Attack>().lockAttack = true;
            GameObject enemy = GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy;
            GameObject player = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString());
            if (enemy)
            {
                EnemiesAutonomousBehavior enemyBehaviour = enemy.GetComponent<EnemiesAutonomousBehavior>();
                if (!enemyBehaviour.counterAttacked)
                {
                    PlayerModel.CurrentModel.defended++;
                    enemyBehaviour.counterAttacked = true;
                }
                int damageDealt = 5 + 5 * player.GetComponent<SpeciesAttributes>().attackUpgrade - 5 * enemy.GetComponent<EnemiesAttributes>().deffenseUpgrade;
                enemy.GetComponent<EnemiesAttributes>().life -= damageDealt;

                GameObject attackSprite = new GameObject("AttackSprite");
                SpriteRenderer spriteRenderer = attackSprite.AddComponent<SpriteRenderer>();
                Sprite cloudSprite = Resources.Load<Sprite>("cloud-normal");
                spriteRenderer.sprite = cloudSprite;
                attackSprite.transform.position = enemy.transform.position;

                GameObject angrySprite = new GameObject("AngrySprite");
                SpriteRenderer angrySpriteRenderer = angrySprite.AddComponent<SpriteRenderer>();
                Sprite angrySpriteRender = Resources.Load<Sprite>(PlayerInfo.selectedSpecies.ToString() + "-atk");
                angrySpriteRenderer.sprite = angrySpriteRender;
                Vector3 angryPosition;
                angryPosition.x = player.transform.position.x + player.GetComponent<BoxCollider>().size.x + 1;
                angryPosition.y = player.transform.position.y + player.GetComponent<BoxCollider>().size.y + 1;
                angryPosition.z = player.transform.position.z;
                angrySprite.transform.position = angryPosition;

                StartCoroutine(FlashCloud(attackSprite));
                StartCoroutine(FinishAttack(attackSprite, angrySprite));

            }
        }
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
        GameObject.Find("MenuCanvas").GetComponent<Attack>().lockAttack = false;
    }
}
