using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Death Logic - Resource Dropper", menuName = "Scriptable Objects/Logics/Death/Resource Dropper", order = 1)]
public class ResourceDropperDeathLogic : DeathLogic
{
    //! Array of ResourceDrop's, assign these in editor to define which resources this object drops.
    [SerializeField]
    private ResourceDrop[] droppableResources;

    public override void Die()
    {
        DropResources();
        Destroy(DeathObject);
    }

    //! Method to spawn the droppable resources from the table.
    public void DropResources()
    {
        int amtToDrop = 0;
        foreach (ResourceDrop rd in droppableResources)
        {
            amtToDrop = UnityEngine.Random.Range(rd.minAmt, rd.maxAmt + 1);

            for (int i = 0; i < amtToDrop; i++)
            {
                Vector3 spawnPos = new Vector3(DeathObject.transform.position.x + UnityEngine.Random.Range(-1, 2),
                    1, DeathObject.transform.position.z + UnityEngine.Random.Range(-1, 2));
                Quaternion spawnRot = Quaternion.Euler(rd.myResource.ResourceItem.ItemPrefab.transform.localEulerAngles.x,
                    UnityEngine.Random.Range(0, 360), rd.myResource.ResourceItem.ItemPrefab.transform.localEulerAngles.z);
                GameObject newResourceItem = Instantiate(rd.myResource.ResourceItem.ItemPrefab, spawnPos, spawnRot);
                newResourceItem.name = rd.myResource.MyName;
                newResourceItem.GetComponent<ResourceItemInteract>().MyResource = rd.myResource;
            }
        }
    }

    //! Serialisable class, drop rates you can define in editor for the object.
    [Serializable]
    public class ResourceDrop
    {
        //! Resource dropped.
        public Resource myResource;
        //! Minimum to drop.
        public int minAmt;
        //! Maximum to drop.
        public int maxAmt;
    }
}
