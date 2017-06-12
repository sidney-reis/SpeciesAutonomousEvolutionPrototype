using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreAttack : MonoBehaviour
{
    public void IgnoreAttackClick()
    {
        PlayerModel.CurrentModel.ran++;
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
