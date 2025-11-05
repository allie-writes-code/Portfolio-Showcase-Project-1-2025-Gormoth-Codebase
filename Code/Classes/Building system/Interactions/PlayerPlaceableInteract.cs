using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerPlaceableInteract : MonoBehaviour
{
    private bool isCarryingPlaceable = false;

    private Vector3 lastStoppedPosition;
    private float currentStoppedTime;

    [SerializeField]
    private Stat placementTime;

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
                lastStoppedPosition = transform.position;
                currentStoppedTime = 0;
            }
            else
            {
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
    }
}
