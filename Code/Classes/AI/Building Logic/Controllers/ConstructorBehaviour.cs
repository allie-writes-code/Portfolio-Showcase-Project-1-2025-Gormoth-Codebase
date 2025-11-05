using UnityEngine;

/* TO DO:
 * - Functionality to control / select Placeable assigned to this constructor.
 */

public class ConstructorBehaviour : MonoBehaviour
{
    [SerializeField]
    private Placeable placeable;

    [SerializeField]
    private BuildingLogic myLogic;

    private float cooldownCurrentTime = 0;

    [SerializeField]
    private Stat cooldown;

    private GameObject coreObject;

    private ConstructorBuildingLogic constructor;

    //! Bool to control if Constructor already has a placeable spawwned.
    private bool hasPlaceableAlready = false;

    private void Start()
    {
        //! We cast myLogic to a ConstructorBuildingLogic type so we can access the Construct function.
        constructor = (ConstructorBuildingLogic)myLogic;
    }

    //! Public Get Set method for hasPlaceableAlready.
    public bool HasPlaceableAlready { get { return hasPlaceableAlready; } set { hasPlaceableAlready = value; } }

    private void Update()
    {
        //Debug.Log("Con checks - Placeable: " + placeable + " - MyLogic: " + myLogic + " - Constructor: " + constructor + " - Has placeable already? " + hasPlaceableAlready);
        if (placeable == null || myLogic == null || constructor == null || hasPlaceableAlready) return;
        
        if (cooldownCurrentTime >= cooldown.ValueFloat)
        {
            if (coreObject == null) { coreObject = GameObject.Find("Core"); }

            if (coreObject != null)
            {
                cooldownCurrentTime = 0;
                constructor.Construct(placeable, this.gameObject, coreObject);
            }
        }
        else
        {
            if (cooldownCurrentTime > cooldown.ValueFloat)
            {
                cooldownCurrentTime = cooldown.ValueFloat;
            }
            else
            {
                cooldownCurrentTime += Time.deltaTime;
            }
        }
    }
}
