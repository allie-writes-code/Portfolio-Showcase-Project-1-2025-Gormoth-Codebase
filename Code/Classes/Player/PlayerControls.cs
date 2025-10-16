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
            playerStats.MoveSpeed.Value * (Time.deltaTime * 0.5f));
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
}
