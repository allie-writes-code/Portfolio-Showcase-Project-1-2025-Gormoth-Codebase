using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! MonoBehaviour class, attach to ResourceItem's to enable their behaviours in game.
public class ResourceItemInteract : MonoBehaviour
{
    //! Reference to the Resource assigned to this item.
    private Resource myResource;

    //! Public Get and Set method for myResource.
    public Resource MyResource
    {
        get { return myResource; }
        set { myResource = value; }
    }

    //! Bool to track if item is being collected currently.
    private bool beingCollected = false;
    //! Bool to track if recently dropped by the player.
    private bool droppedByPlayer = false;

    //! Reference to the target collector object's transform.
    private Transform collector;

    //! Public Get method for beingCollected. Cannot be changed directly.
    public bool BeingCollected
    {
        get { return beingCollected; }
    }

    //! Public Get method for droppedByPlayer;
    public bool DroppedByPlayer
    {
        get { return droppedByPlayer; }
    }

    //! Private float for collection speed.
    [SerializeField]
    private Stat collectSpeed;

    //! Private Bool to control if item waits for a bit before allowing collection again.
    private bool isWaiting = false;
    //! Float to control how long it waits for.
    private float waitTime = 10;

    //! Public Get method for isWaiting.
    public bool IsWaiting
    {
        get { return isWaiting; }
    }

    //! Run this method to initiate collection of the item by an object with the ResourceCarry component.
    public void CollectMe(GameObject c)
    {
        if (!beingCollected && !isWaiting)
        {
            beingCollected = true;
            collector = c.transform;
        }
    }

    public void PlayerDrop()
    {
        droppedByPlayer = true;
        StartCoroutine("PlayerDropReset");
    }

    //! MonoBehaviour Update method.
    private void Update()
    {
        if (beingCollected && collector != null && !isWaiting)
        {
            transform.position = Vector3.MoveTowards(transform.position, collector.position, collectSpeed.Value * (Time.deltaTime * 0.5f));
        }
    }

    //! MonoBehaviour OnTriggerEnter class. Using to detect when the ResourceItem colliders with the (hopefully) defined collector object.
    //! We're using OnTriggerEnter so a trigger can be used on ResourceItem's instead, intentionally so they don't collide with other objects.
    private void OnTriggerEnter(Collider other)
    {
        if (beingCollected && collector != null && !isWaiting)
        {
            if (other.gameObject.transform == collector)
            {
                if (other.gameObject.GetComponent<ResourceCarry>().AddToTotal(myResource))
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    collector = null;
                    isWaiting = true;
                    beingCollected = false;
                    StartCoroutine("WaitABit");
                }
            }
        }
    }

    private IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    private IEnumerator PlayerDropReset()
    {
        yield return new WaitForSeconds(waitTime);
        droppedByPlayer = false;
    }
}
