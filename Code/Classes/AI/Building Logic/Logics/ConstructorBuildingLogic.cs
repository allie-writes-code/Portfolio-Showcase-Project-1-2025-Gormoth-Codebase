using UnityEngine;

/* TO DO:
 * - Check if placeable currently spawned and not yet collected, prevent logic if so.
 */

[CreateAssetMenu(fileName = "New Building Logic - Constructor", menuName = "Scriptable Objects/Logics/Building/Constructor", order = 2)]
public class ConstructorBuildingLogic : BuildingLogic
{
    [SerializeField]
    private ResourceManager resourceManager;

    public void Construct(Placeable placeable, GameObject buildingObject, GameObject coreObject)
    {
        Debug.Log("Construct run");
        ResourceConsumer consumer = buildingObject.GetComponent<ResourceConsumer>();
        
        if (consumer != null)
        {
            // This and the next if condition are checking to see if the values set in the consumer match the placeable.
            // It corrects them if they don't match.
            if (consumer.ConsumedResource != placeable.PlaceableCost.resource)
            { consumer.ConsumedResource = placeable.PlaceableCost.resource; }

            if (consumer.CurrentMaxRequired != placeable.PlaceableCost.costBase.ValueInt)
            { consumer.CurrentMaxRequired = placeable.PlaceableCost.costBase.ValueInt; }

            if (!consumer.IsFull)
            {
                if (resourceManager.TotalIsMoreOrEqual(consumer.ConsumedResource, 1))
                {
                    /*! ConsumeResoure() on the ResourceConsumer component is called here, rather than waiting for ResourceItem to arrive.
                 *! In theory, ResourceItem's should always make it to the consumer once spawned, so this should be safe.
                 *! (Alternatively, you can also fall back on relying on ResourceItem's moving faster than the next cooldown trigger.)
                 *! Because we call ConsumeResource here, we also need to account for this in the ResourceItemInteract class. */
                    consumer.ConsumeResource(placeable.PlaceableCost.resource);
                    SummonResource(placeable.PlaceableCost.resource, buildingObject, coreObject);
                }
            }
            else
            {
                SpawnPlaceable(placeable, buildingObject, consumer);
            }
        }
    }

    private void SummonResource(Resource resourceToConsume, GameObject buildingObject, GameObject coreObject)
    {
        // Logic to spawn a ResourceItem at the core, then tell it to be consumed (collected) by Constructor.
        Vector3 spawnPos = new Vector3(coreObject.transform.position.x, 1, coreObject.transform.position.z);
        GameObject newResourceObject = Instantiate(resourceToConsume.ResourceItem.ItemPrefab, spawnPos, resourceToConsume.ResourceItem.ItemPrefab.transform.rotation);
        ResourceItemInteract ri = newResourceObject.GetComponent<ResourceItemInteract>();
        ri.ConsumeMe(buildingObject);
        resourceManager.SubtractFromTotal(resourceToConsume, 1);
    }

    private void SpawnPlaceable(Placeable placeable, GameObject buildingObject, ResourceConsumer consumer)
    {
        Vector3 placeableSpawnPos = new Vector3
            (Mathf.RoundToInt(buildingObject.transform.position.x), 1, 
            Mathf.RoundToInt(buildingObject.transform.position.z));

        // Spawn the new placeable, update it with it's holding object (in this case, a constructor).
        // Then update the Constructor object's behaviour so it knows it's holding a Placeable.
        GameObject newPlaceableObject = Instantiate(placeable.placeableObjectPrefab, placeableSpawnPos, placeable.placeableObjectPrefab.transform.rotation);
        PlaceableBehaviour newPlaceableBehaviour = newPlaceableObject.GetComponent<PlaceableBehaviour>();
        newPlaceableBehaviour.UpdateHolder(buildingObject);
        newPlaceableBehaviour.MyPlaceable = placeable;
        buildingObject.GetComponent<ConstructorBehaviour>().HasPlaceableAlready = true;
        
        consumer.SubtractFromTotal(placeable.PlaceableCost.costBase.ValueInt);
    }

    public override void Activate() { throw new System.NotImplementedException(); }
    public override void Activate(GameObject buildingObject) { throw new System.NotImplementedException(); }

}
