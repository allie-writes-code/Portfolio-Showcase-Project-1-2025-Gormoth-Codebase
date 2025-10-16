using UnityEngine;

//! This class is responsible for the managment of levels (an active game state, world, data and objects), including loading, generation and saving.
public class LevelManager : MonoBehaviour
{
    //! Prefab for the Path Manager object (A* pathfinding). Set in editor.
    [SerializeField]
    private GameObject pathManagerPrefab;

    //! Prefab for the Player object. Set in editor.
    [SerializeField]
    private GameObject playerPrefab;

    //! Prefab for the Ground Plane object. Set in editor.
    [SerializeField]
    private GameObject groundPlanePrefab;

    //! Reference to a WorldObjectManager.
    [SerializeField]
    private WorldObjectManager worldManager;

    //! MonoBehaviour Start method.
    private void Start()
    {
        GenerateNewLevel();
    }

    //! Method to generate a new level.
    private void GenerateNewLevel()
    {
        if (groundPlanePrefab != null)
        {
            GameObject groundPlane = Instantiate(groundPlanePrefab, new Vector3(0, -1, 0), Quaternion.identity);
            groundPlane.name = "Ground Plane";
        }

        if (worldManager != null)
        {
            worldManager.GenerateNewWorld();
        }

        if (pathManagerPrefab != null)
        {
            GameObject pathManger = Instantiate(pathManagerPrefab, Vector3.zero, Quaternion.identity);
            pathManger.name = "Path Manager";
        }

        if (playerPrefab != null)
        {
            GameObject playerObject = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
            playerObject.name = "Player";
        }
    }
}
