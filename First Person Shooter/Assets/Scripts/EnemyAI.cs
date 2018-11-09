using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public IEnemyState currentState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public GameObject[] totalDecals;
    [HideInInspector] public int actual_decal = 0;
    [HideInInspector] public bool selfDestroying;

    public Light myLight;
    public float life = 100;
    public float timeBetweenShoots = 1.0f;
    public float damageForce = 10f;
    public float rotationTime = 3.0f;
    public Transform[] wayPoints;
    public GameObject decalPrefab;
    public AudioSource laserAudio;
    public AudioSource plasmaExplosionSound;
    public ParticleSystem plasmaExplosion;
    
    [Header("Laser")]
    //glow effect when the enemy laser hit us
    public Light laserImpactLight;
    public LineRenderer laser;
    public ParticleSystem laserGunParticles;
    
    List<MeshRenderer> droneMeshes;
    

    private void Awake()
    {
        droneMeshes = gameObject.GetComponentsInChildren<MeshRenderer>().ToList();
    }

    // Use this for initialization
    void Start()
    {
        totalDecals = new GameObject[10];
        patrolState = new PatrolState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        currentState = patrolState;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();

        if (life < 0 && !selfDestroying)
        {
            selfDestroying = true;
            //get the duration of the particle explosion
            float explosionDuration = plasmaExplosion.GetComponent<ParticleSystem>().main.duration;
            
            plasmaExplosion.Play();
            plasmaExplosionSound.Play();
            //hide the drone emeshes
            droneMeshes.ForEach(m => m.enabled = false);
            laserImpactLight.enabled = false;
            //Destroys the drone when the particle explosion finishes
            Destroy(this.gameObject, explosionDuration);
        }
    }
    
    public void Hit(float damage)
    {
        life -= damage;
        currentState.Impact();
    }

    public void DestroyQuad()
    {
        Destroy(totalDecals[actual_decal]);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    //to execute State coroutines
    public void ExecuteCoroutine(IEnumerator functionToRun)
    {
        StartCoroutine(functionToRun);
    }
}
