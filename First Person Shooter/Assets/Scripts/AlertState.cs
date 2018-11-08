using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    EnemyAI myEnemy;
    float currentRotationTime = 0;
    Transform target;


    public AlertState(EnemyAI enemy)
    {
        myEnemy = enemy;
    }

    public void GoToAlertState()
    {
    }

    public void GoToAttackState()
    {
        myEnemy.currentState = myEnemy.attackState;
    }

    public void GoToPatrolState()
    {
        myEnemy.navMeshAgent.isStopped = false;
        myEnemy.currentState = myEnemy.patrolState;
    }

    public void Impact()
    {
        GoToAttackState();
    }

    public void OnTriggerEnter(Collider col)
    {
    }

    public void OnTriggerExit(Collider col)
    {
    }

    public void OnTriggerStay(Collider col)
    {
        target = col.transform;
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.yellow;

        myEnemy.transform.rotation *= Quaternion.Euler(0f, Time.deltaTime * 360 * 1.0f / myEnemy.rotationTime, 0f);

        if (currentRotationTime > myEnemy.rotationTime)
        {
            currentRotationTime = 0;
            GoToPatrolState();
        }
        else
        {
            if (target != null)
            {
                RaycastHit hit;

                //Get the angle X at wich the drone should be looking to hit us, and apply it to it's rotation 
                //Otherwise, if we are at a different height than the drone, it would never detect us.
                //Example: If the drone is upstairs and we are downstairs, the dron need to be looking for us in the correct angle 
                Vector3 lookDirection = target.position - myEnemy.transform.position;
                Quaternion rot = Quaternion.LookRotation(lookDirection);
                float angleX = rot.eulerAngles.x;
                myEnemy.transform.rotation = Quaternion.Euler(rot.eulerAngles.x, myEnemy.transform.rotation.eulerAngles.y, myEnemy.transform.rotation.eulerAngles.z);
                
                Ray ray = new Ray(myEnemy.transform.position, myEnemy.transform.forward * 100);
                //Debug.DrawRay(myEnemy.transform.position, myEnemy.transform.forward * 100);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        target = null;
                        GoToAttackState();
                    }
                }
                
            }
            currentRotationTime += Time.deltaTime;
        }
    }
}
