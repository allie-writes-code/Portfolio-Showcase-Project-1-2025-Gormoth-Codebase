using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPlaceableInteract : MonoBehaviour
{
    private bool isCarryingPlaceable = false;
    private bool isPlacing = false;

    public bool IsCarryingPlaceable { get  { return isCarryingPlaceable; } }
    public bool IsPlacing { get { return isPlacing; } }

    private Vector3 lastStoppedPosition;
    private float currentStoppedTime;

    [SerializeField]
    private Stat placementTime;

    public float PlacementPercentage{ get { return (currentStoppedTime / placementTime.ValueFloat); } }

    private GameObject heldObject;

    [SerializeField]
    private WorldGridManager grid;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Placeable" && !isCarryingPlaceable)
        {
            isCarryingPlaceable = true;
            heldObject = other.gameObject;
            heldObject.GetComponent<PlaceableBehaviour>().UpdateHolder(this.gameObject);
        }
        
    }

    private void Update()
    {
        if (isCarryingPlaceable)
        {
            if (lastStoppedPosition != transform.position)
            {
                if (isPlacing) isPlacing = false;
                lastStoppedPosition = transform.position;
                currentStoppedTime = 0;
            }
            else
            {
                if (!isPlacing) isPlacing = true;
                currentStoppedTime += Time.deltaTime;
                if (currentStoppedTime >= placementTime.ValueFloat)
                {
                    if (grid.IsNodeClear(transform.position))
                    {
                        BuildPlaceable();
                    }
                }
            }

        }
    }

    private void BuildPlaceable()
    {
        heldObject.GetComponent<PlaceableBehaviour>().PlaceMe();
        isCarryingPlaceable = false;
        isPlacing = false;
    }
}
