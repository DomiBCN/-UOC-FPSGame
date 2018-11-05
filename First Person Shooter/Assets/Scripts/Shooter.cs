using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    public GameObject decalPrefab;
    public AudioSource gunAudioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public float damageForce = 10f;
    public Text cartridgeTxt;
    public Text totalAmmunitionTxt;
    public Text shieldTxt;
    public Text lifeTxt;
    public Slider shieldSlider;
    public Slider lifeSlider;

    float life = 150;
    float shield = 5;
    int totalAmmunition = 120;
    int cartridgeAmmo = 30;

    int maxAmmunition = 990;
    float maxLife = 150;
    float maxShield = 150;

    GameObject[] totalDecals;
    int actual_decal = 0;
    bool reloading;

    private void Awake()
    {
        cartridgeTxt.text = cartridgeAmmo.ToString();
        totalAmmunitionTxt.text = totalAmmunition.ToString();
        shieldTxt.text = shield.ToString();
        lifeTxt.text = life.ToString();
        shieldSlider.value = shield;
        lifeSlider.value = life;
    }

    private void Start()
    {
        totalDecals = new GameObject[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !reloading)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
                Destroy(totalDecals[actual_decal]);
                totalDecals[actual_decal] = GameObject.Instantiate(decalPrefab, hit.point + hit.normal * 0.01f, Quaternion.FromToRotation(Vector3.forward, -hit.normal), hit.collider.gameObject.transform);

                Shoot();

                if (hit.collider.gameObject.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponentInParent<EnemyAI>().Hit(damageForce);
                }

                actual_decal++;
                if (actual_decal == 10) actual_decal = 0;

            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            reloading = true;
            Reload();
        }
    }


    public void Hit(float damage)
    {
        if (shield > 0)
        {
            shield -= damage;

            if (shield < 0)
            {
                life += shield;
                shield = 0;
            }
            shieldSlider.value = shield;
            lifeSlider.value = life;

            shieldTxt.text = shield.ToString();
            lifeTxt.text = life.ToString();
        }
        else
        {
            life -= damage;
            if (life < 0) life = 0;
            lifeSlider.value = life;
            lifeTxt.text = life.ToString();
        }

    }

    #region ITEMS
    //If life or ammo are already at his maximum, do not get the items
    public bool AddLife(float lifePlus)
    {
        bool useItem = false;
        if (life < maxLife)
        {
            useItem = true;
            life += lifePlus;
            lifeSlider.value = life;
            lifeTxt.text = life.ToString();
        }
        return useItem;
    }

    public bool AddShield(float shieldPlus)
    {
        bool useItem = false;
        if (shield < maxShield)
        {
            useItem = true;
            shield += shieldPlus;
            shieldSlider.value = shield;
            shieldTxt.text = shield.ToString();
        }
        return useItem;
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
    #endregion

    #region GUN
    void Shoot()
    {
        Fire();
        if (cartridgeAmmo == 1)
        {
            cartridgeAmmo = 0;
            cartridgeTxt.text = cartridgeAmmo.ToString();
            reloading = true;
            //we want to let the fire clip to sound before start the reloading one
            Invoke("Reload", 0.9f);
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

    void Reload()
    {
        if (totalAmmunition > 0)
        {
            reloading = true;
            gunAudioSource.clip = reloadSound;
            gunAudioSource.Play();

            //The reloading sound takes almost 3 seconds to complete
            Invoke("UpdateAmmo", 1.5f);
            Invoke("StopReloading", 3f);
        }
    }

    void UpdateAmmo()
    {
        int bulletsNeeded = 30 - cartridgeAmmo;
        cartridgeAmmo = totalAmmunition >= 30 ? cartridgeAmmo += bulletsNeeded : totalAmmunition;
        totalAmmunition = Mathf.Clamp(totalAmmunition - bulletsNeeded, 0, maxAmmunition);

        cartridgeTxt.text = cartridgeAmmo.ToString();
        totalAmmunitionTxt.text = totalAmmunition.ToString();
    }

    void StopReloading()
    {
        reloading = false;
    }
    #endregion
}
