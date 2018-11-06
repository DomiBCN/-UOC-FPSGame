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
            RaycastHit hit;

            if (Physics.Raycast(new Ray(new Vector3(myEnemy.transform.position.x, myEnemy.transform.position.y - 0.6f, myEnemy.transform.position.z), myEnemy.transform.forward * 100f), out hit))
            {
                myEnemy.fireAudio.Play();
                if (hit.collider.gameObject.tag == "Player")
                {
                    col.gameObject.GetComponent<PlayerHealth>().Hit(myEnemy.damageForce);
                }
                else
                {
                    myEnemy.DestroyQuad();
                    myEnemy.totalDecals[myEnemy.actual_decal] = GameObject.Instantiate(myEnemy.decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), hit.collider.gameObject.transform);
                    myEnemy.actual_decal++;
                    if (myEnemy.actual_decal == 10) myEnemy.actual_decal = 0;
                }
            }
            actualTimeBetweenShoots = 0;

        }
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShoots += Time.deltaTime;
    }
}
