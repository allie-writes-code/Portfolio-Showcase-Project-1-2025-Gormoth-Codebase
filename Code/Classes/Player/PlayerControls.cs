using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;

    private float hor = 0;
    private float vert = 0;

    [SerializeField]
    private CharacterStats playerStats;

    [SerializeField]
    private LayerMask resourceDropperLayer;

    //! Added this for debugging - remove:
    [SerializeField]
    private NodeGrid grid;

    // Resource load debug variables:
    private GameResourceManager gameResourceManager = new();
    // Remove all these ^

    private void Update()
    {
        moveDirection = Vector3.zero;

        hor = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        if (hor > 0) // Right
        {
            moveDirection += moveDirRight;            
        } 
        else if (hor < 0) // Left
        {
            moveDirection += moveDirLeft;
        }
        
        if (vert > 0) // Up
        {
            moveDirection += moveDirUp;
        } 
        else if (vert < 0) // Down
        {
            moveDirection += moveDirDown;
        }

        RemoveDiagonalSpeedBoost();

        transform.position = Vector3.MoveTowards(
            transform.position, 
            transform.position + moveDirection, 
            playerStats.MoveSpeed * (Time.deltaTime * 0.5f));

        // Debug controls for resource load testing.
        if (Input.GetKeyDown("1")) { gameResourceManager.SaveResourcesToFile(); }
        if (Input.GetKeyDown("2")) { gameResourceManager.LoadResourcesFromFile(); }
    }

    private void RemoveDiagonalSpeedBoost()
    {
        if (moveDirection.x > 0) { moveDirection.x = 1; }
        else if (moveDirection.x < 0) { moveDirection.x = -1; }

        if (moveDirection.z > 0) { moveDirection.z = 1; }
        else if (moveDirection.z < 0) { moveDirection.z = -1; }
    }

    private Vector3 moveDirUp = new Vector3(-1, 0, 1);
    private Vector3 moveDirDown = new Vector3(1, 0, -1);
    private Vector3 moveDirLeft = new Vector3(-1, 0, -1);
    private Vector3 moveDirRight = new Vector3(1, 0, 1);

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.gridWorldSize.x, 1, grid.gridWorldSize.y));

        if (grid.grid != null)
        {
            if (grid.grid.Length > 0)
            {
                foreach (PathNode n in grid.grid)
                {
                    if (n.walkable == false)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(n.worldPosition, Vector3.one * (grid.nodeDiameter));
                    }
                    /*e
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(n.worldPosition, Vector3.one * (grid.nodeDiameter));
                    }
                    */
                }
            }
        }
    }
}
