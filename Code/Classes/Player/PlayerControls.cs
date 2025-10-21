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

    //! Reference to a BuildingManager instance. We send control input, positions, etc, to this to control the in game building system.
    [SerializeField]
    private BuildingManager buildingManager;

    private Vector3 testBuildPos;

    //! Bool to control interaction with the BuildingManager
    private bool buildModeOn = false;

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

        if (buildingManager != null)
        {
            if (Input.GetKeyDown("b"))
            {
                buildModeOn = !buildModeOn;
                buildingManager.ToggleBuildMode(buildModeOn);
            }

            if (buildModeOn)
            {
                if (Input.GetKeyDown("e")) buildingManager.CycleSelected(1);
                else if (Input.GetKeyDown("q")) buildingManager.CycleSelected(-1);
                
                Plane plane = new Plane(Vector3.up, Vector3.up);

                Vector3 worldPosition = Vector3.zero;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float enter)) {
                    worldPosition = ray.GetPoint(enter);
                    buildingManager.BuildPos = worldPosition;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (buildModeOn)
                {
                    buildingManager.Build();
                }
            }
        }
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
