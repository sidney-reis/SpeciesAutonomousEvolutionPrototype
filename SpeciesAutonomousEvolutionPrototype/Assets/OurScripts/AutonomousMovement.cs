using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutonomousMovement : MonoBehaviour
{
    NavMeshPath pathToFruit;
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().CalculatePath(GameObject.Find("Food/fruit0").transform.position, pathToFruit);
    }

    void Update()
    {
        for (int i = 0; i < pathToFruit.corners.Length; i++)
        {
            gameObject.transform.position = pathToFruit.corners[i];
        }
    }
}