using System.Collections;
using UnityEngine;

//! MonoBehaviour class to define the main AI loop for enemies.
[RequireComponent(typeof(AIMove))]
public class EnemyAILoop : MonoBehaviour
{
    private AIMove aiMove;

    private GameObject player;
    private GameObject core;

    [SerializeField]
    private float playerDistanceCheckRadius;
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private CharacterStats stats;

    private CharacterHealth myHealth;

    private void Start()
    {
        aiMove = GetComponent<AIMove>();
        player = GameObject.Find("Player");
        core = GameObject.Find("Core");

        myHealth = GetComponent<CharacterHealth>();

        StartCoroutine("PlayerNearCheck");
    }

    public void DamageCheck()
    {
        float dist = 0;

        if (aiMove.target == player.transform)
        {
            dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist <= attackRange) 
            {
                Debug.Log("Distance from player: " + dist + " - Player pos " + player.transform.position + " - My pos = " + transform.position);
                player.GetComponent<CharacterHealth>().Hurt(Mathf.RoundToInt(stats.Damage));
                DestroyThisEnemy();
            }
        }
        else if (aiMove.target == core.transform)
        {
            dist = Vector3.Distance(transform.position, core.transform.position);
            if (dist <= attackRange)
            {
                Debug.Log("Distance from core: " + dist + " - Core pos = " + core.transform.position + " - My pos = " + transform.position);
                core.GetComponent<CharacterHealth>().Hurt(Mathf.RoundToInt(stats.Damage));
                DestroyThisEnemy();
            }
        }
    }

    private void DestroyThisEnemy()
    {
        this.gameObject.GetComponent<CharacterHealth>().Kill();
    }

    private void Update()
    {
        if (aiMove.target == null)
        {
            aiMove.target = core.transform;
        }
    }

    private IEnumerator PlayerNearCheck()
    {
        if (Physics.CheckSphere(transform.position, playerDistanceCheckRadius, playerLayer))
        {
            aiMove.target = player.transform;
        }
        else
        {
            aiMove.target = core.transform;
        }

        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine("PlayerNearCheck");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            SimpleProjectileBehaviour behaviour = other.gameObject.GetComponent<SimpleProjectileBehaviour>();
            myHealth.Hurt(Mathf.RoundToInt(behaviour.Damage));
            Destroy(other.gameObject);
        }
    }
}
