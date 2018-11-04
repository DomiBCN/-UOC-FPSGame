using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyAI myEnemy;
    float actualTimeBetweenShoots = 0;

    public AttackState(EnemyAI enemy)
    {
        myEnemy = enemy;
    }

    public void GoToAlertState()
    {
        myEnemy.currentState = myEnemy.alertState;
    }

    public void GoToAttackState()
    {
    }

    public void GoToPatrolState()
    {
    }

    public void Impact()
    {
    }

    public void OnTriggerEnter(Collider col)
    {
    }

    public void OnTriggerExit(Collider col)
    {
        GoToAlertState();
    }

    public void OnTriggerStay(Collider col)
    {
        Vector3 lookDirection = col.transform.position - myEnemy.transform.position;

        myEnemy.transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDirection.x, 0, lookDirection.z));

        if (actualTimeBetweenShoots > myEnemy.timeBetweenShoots)
        {
            Debug.Log("attackiiiing");
            actualTimeBetweenShoots = 0;
            col.gameObject.GetComponent<Shooter>().Hit(myEnemy.damageForce);
        }
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShoots += Time.deltaTime;
    }
}
