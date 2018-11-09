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
        if (!myEnemy.selfDestroying)
        {
            Vector3 lookDirection = col.transform.position - myEnemy.transform.position;

            myEnemy.transform.rotation = Quaternion.FromToRotation(Vector3.forward, lookDirection);
            if (actualTimeBetweenShoots > myEnemy.timeBetweenShoots)
            {
                RaycastHit hit;
                Ray ray = new Ray(myEnemy.transform.position, myEnemy.transform.forward * 100f);
                Debug.DrawRay(myEnemy.transform.position, myEnemy.transform.forward * 100);
                if (Physics.Raycast(ray, out hit))
                {
                    myEnemy.ExecuteCoroutine(ShootLaser(hit.point));

                    myEnemy.laserAudio.Play();
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        var phealth = col.gameObject.GetComponent<PlayerHealth>();
                        if (phealth != null) phealth.Hit(myEnemy.damageForce);
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
    }

    public void UpdateState()
    {
        myEnemy.myLight.color = Color.red;
        actualTimeBetweenShoots += Time.deltaTime;
    }

    IEnumerator ShootLaser(Vector3 hitPosition)
    {
        myEnemy.laser.enabled = true;
        myEnemy.laserGunParticles.Play();
        myEnemy.laser.SetPosition(0, myEnemy.transform.position);
        myEnemy.laser.SetPosition(1, hitPosition);
        myEnemy.laserImpactLight.enabled = true;
        yield return new WaitForSeconds(0.2f);
        myEnemy.laserImpactLight.enabled = false;
        myEnemy.laser.enabled = false;
        myEnemy.laserGunParticles.Stop();
    }
}
