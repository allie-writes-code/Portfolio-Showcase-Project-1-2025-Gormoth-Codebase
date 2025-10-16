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
    private float attackRadius;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private DelegateListener aiMoveListener;
    
    private void Start()
    {
        aiMove = GetComponent<AIMove>();
        player = GameObject.Find("Player");
        core = GameObject.Find("Core");

        aiMoveListener.RegisterFunction(DamageCheck);

        StartCoroutine("PlayerNearCheck");
    }

    //! Don't forget to deregister!!!
    private void OnDestroy()
    {
        aiMoveListener.DeregisterFunction(DamageCheck);
    }

    private void DamageCheck()
    {
        Debug.Log("Damage check!!!");
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
}
