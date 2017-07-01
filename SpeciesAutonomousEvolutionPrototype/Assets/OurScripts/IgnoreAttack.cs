using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreAttack : MonoBehaviour
{
    public void IgnoreAttackClick()
    {
        GameObject enemy = GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().approachingEnemy;
        if (enemy)
        {
            EnemiesAutonomousBehavior enemyBehaviour = enemy.GetComponent<EnemiesAutonomousBehavior>();
            if (!enemyBehaviour.ran)
            {
                PlayerModel.CurrentModel.ran++;
                Debug.Log("Current Model 'ran' value increased by 1.\nCurrent 'ran' is: " + PlayerModel.CurrentModel.ran);
                enemyBehaviour.counterAttacked = true;
            }
        }
        CounterCombatHandler counterHandler = GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>();
        GameObject enemyToIgnore = counterHandler.approachingEnemy;
        counterHandler.ignoredEnemy.Add(enemyToIgnore);
        StartCoroutine(RemoveIgnoredEnemy(enemyToIgnore));
        GameObject.Find("CounterBackground").SetActive(false);
    }

    IEnumerator RemoveIgnoredEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(10);
        GameObject.Find("CounterMenuCanvas").GetComponent<CounterCombatHandler>().ignoredEnemy.Remove(enemy);
    }
}
