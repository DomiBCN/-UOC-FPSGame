using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void Awake()
    {
        //to destroy the bullets that not collide with anything(Ex. shoot to the sky)
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
