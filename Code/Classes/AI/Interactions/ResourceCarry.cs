using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Monobehaviour class for Character resource carrying behaviours and interactions.
* Attached to GameObjects that have the ability to pick up and carry resources.
* Game Objects with this component must also have a CharacterStats MonoBehaviour attached. */
public class ResourceCarry : MonoBehaviour
{
    //! List of the Resource's carried by this character object. The 'inventory' of resources on hand.
    private List<Resource> carriedResources = new List<Resource>();

    //! Reference to CharacterStats instance.
    [SerializeField]
    private CharacterStats stats;

    //! Collider array used in the Update loop to check for nearby resource items, etc.
    private Collider[] resourceItemColliders;

    //! Because we use overlapspherenonalloc, we need to define a max amount of colliders to hit each time.
    private const int maxResourceItemColliders = 100;

    //! Resource collection radius. Serialised to allow editor access.
    [SerializeField]
    private int resourceCollectRadius = 5;

    //! Bool to determine if this object is the player or not. This class is also used for other collection objects.
    [SerializeField]
    private bool isPlayer = false;

    //! LayerMask used for collisions with Resource Item's.
    [SerializeField]
    private LayerMask resourceItemsLayer;

    //! LayerMask used for collisions with other ResourceCarry objects.
    [SerializeField]
    private LayerMask carryObjectsLayer;

    //! Reference to the ResourceManager used.
    [SerializeField]
    private ResourceManager resourceManager;

    //! Private int to store amount of colliders hit.
    private int numCollidersHit = 0;

    //! MonoBehaviour Start method.
    private void Start()
    {
        // We still need to initialise the collider arrays.
        resourceItemColliders = new Collider[maxResourceItemColliders];
    }

    private void Update()
    {
        numCollidersHit = 0;
        if (isPlayer)
        {
            if (carriedResources.Count < stats.Carry.Value)
            {
                numCollidersHit = Physics.OverlapSphereNonAlloc(transform.position, resourceCollectRadius, resourceItemColliders, resourceItemsLayer, QueryTriggerInteraction.Collide);

                if (numCollidersHit > 0)
                {
                    for (int i = 0; i < numCollidersHit; i++)
                    {
                        ResourceItemInteract ri = resourceItemColliders[i].gameObject.GetComponent<ResourceItemInteract>();

                        if (!ri.BeingCollected && !ri.DroppedByPlayer && !ri.IsWaiting)
                        {
                            ri.CollectMe(this.gameObject);
                        }
                    }
                }

                if (carriedResources.Count > 0)
                {
                    numCollidersHit = Physics.OverlapSphereNonAlloc(transform.position, resourceCollectRadius, resourceItemColliders, carryObjectsLayer);

                    if (numCollidersHit > 0)
                    {
                        DropHeldResources();
                    }
                }
            }
            else
            {
                numCollidersHit = Physics.OverlapSphereNonAlloc(transform.position, resourceCollectRadius, resourceItemColliders, carryObjectsLayer);

                if (numCollidersHit > 0)
                {
                    DropHeldResources();
                }
            }
        }
        else
        {
            numCollidersHit = Physics.OverlapSphereNonAlloc(transform.position, resourceCollectRadius, resourceItemColliders, resourceItemsLayer, QueryTriggerInteraction.Collide);

            if (numCollidersHit > 0)
            {
                for (int i = 0; i < numCollidersHit; i++)
                {
                    ResourceItemInteract ri = resourceItemColliders[i].gameObject.GetComponent<ResourceItemInteract>();

                    if (!ri.BeingCollected && !ri.IsWaiting)
                    {
                        ri.CollectMe(this.gameObject);
                    }
                }
            }
        }
    }

    public void DropHeldResources()
    {
        // We create a copy of the list and pass that, also removing the passed resources.
        // We do that here to prevent duplication, as the function runs in a coroutine.
        List<Resource> resourcesToDrop = new List<Resource>(carriedResources);
        foreach (Resource r in resourcesToDrop)
        {
            carriedResources.Remove(r);
        }

        StartCoroutine(DropResources(resourcesToDrop));
    }

    //! Private Coroutine. Handles loop to drop held resources, breaks it up to prevent lagging.
    private IEnumerator DropResources(List<Resource> resourcesToDrop)
    {
        if (resourcesToDrop.Count > 0)
        {
            foreach (Resource r in resourcesToDrop)
            {
                Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-1, 2),
                        1, transform.position.z + Random.Range(-1, 2));
                Quaternion spawnRot = Quaternion.Euler(r.ResourceItem.ItemPrefab.transform.localEulerAngles.x,
                    Random.Range(0, 360), r.ResourceItem.ItemPrefab.transform.localEulerAngles.z);
                GameObject newResourceItem = Instantiate(r.ResourceItem.ItemPrefab, spawnPos, spawnRot);
                newResourceItem.name = r.MyName;
                ResourceItemInteract ri = newResourceItem.GetComponent<ResourceItemInteract>();

                if (isPlayer)
                {
                    ri.PlayerDrop();
                }

                ri.MyResource = r;
                yield return null;
            }
        }
    }

    public bool AddToTotal(Resource res)
    {
        bool success = false;

        if (isPlayer)
        {
            if (carriedResources.Count < stats.Carry.BaseValue)
            {
                carriedResources.Add(res);
                Debug.Log("Carrying " + carriedResources.Count + " resources.");
                success = true;
            }
        }
        else
        {
            resourceManager.AddToTotal(res, 1);
            success = true;
        }

        return success;
    }
}
