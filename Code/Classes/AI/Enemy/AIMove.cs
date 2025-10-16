using System.Collections;
using UnityEngine;

//! Move AI for non player characters.
public class AIMove : MonoBehaviour
{
    //! Reference to a Transform to use as a pathfinding target.
    public Transform target;

    //! Move speed (UPDATE THIS TO USE STATS)
    [SerializeField]
    private CharacterStats stats;

    //! Vector3 array to store path.
    Vector3[] path;

    //! Index value to track position on path.
    int targetIndex;

    //! Vector3 to track position of target and update path when it changes.
    Vector3 lastKnownTargetPosition = Vector3.zero;

    [SerializeField]
    private DelegateBroadcaster aiMoveBroadcaster;

    //! Start method.
    private void Start()
    {
        StartCoroutine("UpdatePathCheck");
    }

    //! Looping Coroutine to check if a new path is needed. Runs every X seconds.
    //! If target is not null and last known target position is not the target's current position, it finds a new path.
    IEnumerator UpdatePathCheck()
    {
        if (target != null)
        {
            if (lastKnownTargetPosition != target.position)
            {
                lastKnownTargetPosition = target.position;
                PathQueue.RequestPath(transform.position, target.position, OnPathFound);
            }
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine("UpdatePathCheck");
    }

    //! Callback from FinishedProcessingPath in PathQueue, returned after PathManager processes a path.
    //! Will not run if a successful path is not found.
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        StopCoroutine("FollowPath");

        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }

    //! Looping Coroutine to move this GameObject's transform along the path.
    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    PathFinish();
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, stats.MoveSpeed.Value * (Time.deltaTime * 0.5f));
            yield return null;
        }
    }

    private void PathFinish()
    {
        aiMoveBroadcaster.InvokeMe();
    }
}
