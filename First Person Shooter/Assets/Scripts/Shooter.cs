using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public Text reloadingTxt;
    public GameObject decalPrefab;
    public AudioSource gunAudioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public float damageForce = 10f;
    public Text cartridgeTxt;
    public Text totalAmmunitionTxt;
    public Transform gunEnd;
    public Transform shellEject;
    public GameObject bulletPrefab;
    public GameObject shellPrefab;

    public ParticleSystem bulletImpact;


    int totalAmmunition = 120;
    int cartridgeAmmo = 30;

    int maxAmmunition = 990;
    int cartridgeCapacity = 30;

    float bulletSpread = 0.025f; //we will use it to add some inaccuracy to our aiming

    GameObject[] totalDecals;
    int actual_decal = 0;
    bool reloading;

    private void Awake()
    {
        cartridgeTxt.text = cartridgeAmmo.ToString();
        totalAmmunitionTxt.text = totalAmmunition.ToString();
    }

    private void Start()
    {
        totalDecals = new GameObject[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cartridgeAmmo > 0)
        {
            RaycastHit hit;
            Ray raySpread = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //modify the direction using the bulletSpread factor
            Vector3 bulletSpreadDirection = new Vector3(raySpread.direction.x + Random.Range(-bulletSpread, bulletSpread), raySpread.direction.y + Random.Range(-bulletSpread, bulletSpread), raySpread.direction.z);
            raySpread.direction = bulletSpreadDirection;
            Shoot(bulletSpreadDirection);
            if (Physics.Raycast(raySpread, out hit))
            {
                bulletImpactEffect(hit);

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyAI>().Hit(damageForce);
                }
                else
                {
                    Destroy(totalDecals[actual_decal]);
                    totalDecals[actual_decal] = GameObject.Instantiate(decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), hit.collider.gameObject.transform);
                    actual_decal++;
                    if (actual_decal == 10) actual_decal = 0;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            if (totalAmmunition > 0 && cartridgeAmmo < cartridgeCapacity)
            {
                reloading = true;
                Reload();
            }
        }
    }

    public bool AddAmmo(int bullets)
    {
        bool useItem = false;
        if (totalAmmunition < maxAmmunition)
        {
            useItem = true;
            totalAmmunition = totalAmmunition + bullets > maxAmmunition ? maxAmmunition : totalAmmunition + bullets;
            totalAmmunitionTxt.text = totalAmmunition.ToString();
        }
        return useItem;
    }

    #region SHOOT && RELOAD
    void Shoot(Vector3 bulletSpreadDirection)
    {
        InstantiateBulletAndShell(bulletSpreadDirection);

        Fire();
        
        if (cartridgeAmmo == 1)
        {
            cartridgeAmmo = 0;
            cartridgeTxt.text = cartridgeAmmo.ToString();
            if (totalAmmunition > 0)
            {
                //we want to let the fire clip to sound before start the reloading one
                Invoke("Reload", 0.9f);
            }
        }
        else
        {
            cartridgeAmmo -= 1;
            cartridgeTxt.text = cartridgeAmmo.ToString();
        }
    }

    void Fire()
    {
        gunAudioSource.clip = fireSound;
        gunAudioSource.Play();
    }

    void InstantiateBulletAndShell(Vector3 bulletSpreadDirection)
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, gunEnd.position, gunEnd.rotation);
        bullet.transform.Rotate(Vector3.left * 90);
        bullet.GetComponent<ParticleSystem>().Play();
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpreadDirection * 10000);

        GameObject shell = GameObject.Instantiate(shellPrefab, shellEject.position, shellEject.rotation);
        shell.transform.Rotate(Vector3.left * 90);
        shell.GetComponent<Rigidbody>().AddForce(-shellEject.transform.right * 150);
        Destroy(shell, 1f);
    }

    void Reload()
    {
        reloadingTxt.enabled = true;
        reloading = true;
        gunAudioSource.clip = reloadSound;
        gunAudioSource.Play();

        //The reloading sound takes almost 3 seconds to complete
        Invoke("StopReloading", 3f);
    }

    void StopReloading()
    {
        int bulletsNeeded = cartridgeCapacity - cartridgeAmmo;
        cartridgeAmmo = totalAmmunition >= cartridgeCapacity ? cartridgeAmmo += bulletsNeeded : totalAmmunition;
        totalAmmunition = Mathf.Clamp(totalAmmunition - bulletsNeeded, 0, maxAmmunition);

        cartridgeTxt.text = cartridgeAmmo.ToString();
        totalAmmunitionTxt.text = totalAmmunition.ToString();

        reloadingTxt.enabled = false;
        reloading = false;
    }

    void bulletImpactEffect(RaycastHit impact)
    {
        ParticleSystem impactEffect = GameObject.Instantiate(bulletImpact, impact.point, Quaternion.Euler(0,0,0));
        impactEffect.Play();
    }
    #endregion

}
