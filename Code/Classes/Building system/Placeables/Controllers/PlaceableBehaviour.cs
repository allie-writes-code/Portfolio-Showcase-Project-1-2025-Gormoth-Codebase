using UnityEngine;

//! MonoBehaviour class, attach as a component on Placeable prefabs.
public class PlaceableBehaviour : MonoBehaviour
{
    private GameObject holdingObject;
    //! Public Get Set method for holdingObject.
    public GameObject HoldingObject { get { return holdingObject; } set { holdingObject = value; } }

    [SerializeField]
    private Stat placeableRotateSpeedStat;

    private Placeable myPlaceable;

    public Placeable MyPlaceable { get { return myPlaceable; } set { myPlaceable = value; } }

    [SerializeField]
    private WorldGridManager grid;

    private void Update()
    {
        if (holdingObject != null)
        {
            if (transform.position != (holdingObject.transform.position + (Vector3.up * 1.5f)))
            {
                transform.position = (holdingObject.transform.position + (Vector3.up * 1.5f));
            }
        }
        
        transform.Rotate(Vector3.up * placeableRotateSpeedStat.ValueFloat * Time.deltaTime);
    }

    public void UpdateHolder(GameObject holder)
    {
        if (holdingObject != null)
        {
            // Check if existing holder is a Constructor (i.e. placeable is being 'picked up').
            // If so, we need to set the HasPlaceableAlready bool to false on the Constructor.
            ConstructorBehaviour behaviour = holdingObject.GetComponent<ConstructorBehaviour>();
            if (behaviour != null) { behaviour.HasPlaceableAlready = false; }
            holdingObject = null;
        }

        holdingObject = holder;
        transform.position = (holdingObject.transform.position + (Vector3.up * 1.5f));
    }

    public void PlaceMe()
    {
        Vector3 spawnPos = new Vector3(Mathf.RoundToInt(transform.position.x), 1, Mathf.RoundToInt(transform.position.z));
        GameObject newBuilding = Instantiate(myPlaceable.placeableBuildingPrefab, spawnPos, myPlaceable.placeableBuildingPrefab.transform.rotation);
        grid.OccupyNode(spawnPos);
        grid.SetNodeObject(spawnPos, newBuilding);
        Destroy(this.gameObject);
    }

}
