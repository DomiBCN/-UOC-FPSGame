using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject decalPrefab;
    public AudioSource fireSound;
    public float life = 100;
    public float damageForce = 10f;

    GameObject[] totalDecals;
    int actual_decal = 0;

    private void Start()
    {
        totalDecals = new GameObject[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
                Destroy(totalDecals[actual_decal]);
                totalDecals[actual_decal] = GameObject.Instantiate(decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), hit.collider.gameObject.transform);

                fireSound.Play();

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyAI>().Hit(damageForce);
                }

                actual_decal++;
                if (actual_decal == 10) actual_decal = 0;

            }
        }
    }

    public void Hit(float damage)
    {
        life -= damage;
        Debug.Log("Player hitted: " + life);
    }
}
