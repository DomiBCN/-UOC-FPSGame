using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    float rotationTime = 3f;
    bool itemUsed;

	// Update is called once per frame
	void Update () {
        transform.parent.rotation *= Quaternion.Euler(0f, Time.deltaTime * 360 * 1.0f / rotationTime, 0f);
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(gameObject.tag == "Life")
            {
                itemUsed = other.gameObject.GetComponent<Shooter>().AddLife(30);
            }
            else if(gameObject.tag == "Ammo")
            {
                itemUsed = other.gameObject.GetComponent<Shooter>().AddAmmo(60);
            }
            else if(gameObject.tag == "Shield")
            {
                itemUsed = other.gameObject.GetComponent<Shooter>().AddShield(30);
            }

            //destroy the item just if it's been used
            if (itemUsed)
            {
                Destroy(gameObject);
            }
        }
    }
}
