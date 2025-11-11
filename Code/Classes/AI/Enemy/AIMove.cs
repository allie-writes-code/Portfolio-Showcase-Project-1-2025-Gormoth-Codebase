using System.Collections;
using UnityEngine;

//! Move AI for non player characters.
public class AIMove : MonoBehaviour
{
    //! Reference to a Transform to use as a pathfinding target.
    public Transform target;

    //! Stats for move speed.
    [SerializeField]
    private CharacterStats stats;

    [SerializeField]
    private float waypointCheckDistance = 0f;

    //! Vector3 array to store path.
    Vector3[] path;

    //! Index value to track position on path.
    int targetIndex;

    private Vector3 currentWaypoint;

    private bool onPath = false;

    //! Start method.
    private void Start()
    {
        StartCoroutine("UpdatePathCheck");
    }

    //! Looping Coroutine to check if a new path is needed. Runs every X seconds.
    //! If target is not null and last known target position is not the target's current position, it finds a new path.
    IEnumerator UpdatePathCheck()
    {
        while (true)
        {
            if (target != null)
            {
                onPath = false;
                PathQueue.RequestPath(transform.position, target.position, OnPathFound);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    //! Callback from FinishedProcessingPath in PathQueue, returned after PathManager processes a path.
    //! Will not run if a successful path is not found.
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            currentWaypoint = path[0];
            onPath = true;
        }
    }

    private void Update()
    {
        if (!onPath) return;

        if (Vector3.Distance(currentWaypoint, transform.position) <= waypointCheckDistance)
        {
            targetIndex++;
            if (targetIndex >= path.Length)
            {
                onPath = false;
            }

            currentWaypoint = path[targetIndex];
        }

        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, stats.MoveSpeed * (Time.deltaTime * 0.5f));
    }
}
