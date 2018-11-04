using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    EnemyAI myEnemy;
    int nextWayPoint = 0;

    public PatrolState(EnemyAI enemy)
    {
        myEnemy = enemy;
    }

    public void GoToAlertState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {
        myEnemy.navMeshAgent.isStopped = true;
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {

    }

    public void Impact()
    {
        GoToAlertState();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            GoToAlertState();
        }
    }

    public void OnTriggerExit(Collider col)
    {
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Found!!");
            GoToAlertState();
        }
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.green;

        myEnemy.navMeshAgent.destination = myEnemy.wayPoints[nextWayPoint].position;

        if (myEnemy.navMeshAgent.remainingDistance <= myEnemy.navMeshAgent.stoppingDistance)
        {
            nextWayPoint = (nextWayPoint + 1) % myEnemy.wayPoints.Length;
        }
    }
}
