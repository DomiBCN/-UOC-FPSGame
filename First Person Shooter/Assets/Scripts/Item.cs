using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public ItemTypeEnum itemType;

    float rotationTime = 3f;
    bool itemUsed;

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation *= Quaternion.Euler(0f, Time.deltaTime * 360 * 1.0f / rotationTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (itemType)
            {
                case ItemTypeEnum.LIFE:
                    itemUsed = other.gameObject.GetComponent<PlayerHealth>().AddLife(30);
                    break;
                case ItemTypeEnum.SHIELD:
                    itemUsed = other.gameObject.GetComponent<PlayerHealth>().AddShield(30);
                    break;
                case ItemTypeEnum.AMMO:
                    itemUsed = other.gameObject.GetComponent<Shooter>().AddAmmo(60);
                    break;
                default:
                    break;
            }

            //destroy the item just if it's been used
            if (itemUsed)
            {
                Destroy(gameObject);
            }
        }
    }
}
