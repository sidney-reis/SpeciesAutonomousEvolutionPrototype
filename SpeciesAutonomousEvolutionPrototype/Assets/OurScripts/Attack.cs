using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public bool lockAttack = false;

    public void AttackClick()
    {
        GameObject activeCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString()).gameObject;
        GameObject passiveCreature = GameObject.Find("EnemiesCreatures/Enemies" + PlayerInfo.selectedMenuEnemySpecies.ToString() + "/" + PlayerInfo.selectedMenuEnemy.ToString()).gameObject;

        if (Vector3.Distance(activeCreature.transform.position, passiveCreature.transform.position) > 12.0)
        {
            Debug.Log(Vector3.Distance(activeCreature.transform.position, passiveCreature.transform.position));
            GameObject.Find("MenuCanvas/MenuBackground/CombatMenu/ErrorText").SetActive(true);
            GameObject.Find("MenuCanvas/MenuBackground/CombatMenu/ErrorText").GetComponent<Text>().text = "Creature is too far!";
        }
        else if (!lockAttack)
        {
            lockAttack = true;
            GameObject.Find("MenuCanvas/MenuBackground/CombatMenu/ErrorText").GetComponent<Text>().text = "";
            GameObject.Find("MenuCanvas/MenuBackground/CombatMenu/ErrorText").SetActive(false);

            GameObject attackSprite = new GameObject("AttackSprite");
            SpriteRenderer spriteRenderer = attackSprite.AddComponent<SpriteRenderer>();
            Sprite cloudSprite = Resources.Load<Sprite>("cloud-normal");
            spriteRenderer.sprite = cloudSprite;
            attackSprite.transform.position = passiveCreature.transform.position;

            GameObject angrySprite = new GameObject("AngrySprite");
            SpriteRenderer angrySpriteRenderer = angrySprite.AddComponent<SpriteRenderer>();
            Sprite angrySpriteRender = Resources.Load<Sprite>(PlayerInfo.selectedSpecies.ToString() + "-atk");
            angrySpriteRenderer.sprite = angrySpriteRender;
            Vector3 angryPosition;
            angryPosition.x = activeCreature.transform.position.x + activeCreature.GetComponent<BoxCollider>().size.x + 1;
            angryPosition.y = activeCreature.transform.position.y + activeCreature.GetComponent<BoxCollider>().size.y + 1;
            angryPosition.z = activeCreature.transform.position.z;
            angrySprite.transform.position = angryPosition;


            int damageDealt = 5 + 5 * activeCreature.GetComponent<SpeciesAttributes>().attackUpgrade - 5 * passiveCreature.GetComponent<EnemiesAttributes>().deffenseUpgrade;
            if(damageDealt < 0)
            {
                damageDealt = 0;
            }
            passiveCreature.GetComponent<EnemiesAttributes>().life -= damageDealt;
            PlayerModel.CurrentModel.attacked += damageDealt;

            StartCoroutine(FlashCloud(attackSprite));
            StartCoroutine(FinishAttack(attackSprite, angrySprite));
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
        lockAttack = false;
    }
}