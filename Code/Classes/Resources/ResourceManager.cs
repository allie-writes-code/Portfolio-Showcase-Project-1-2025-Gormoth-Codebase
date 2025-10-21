using UnityEngine;

//! ScriptableObject class for defining a resource manager.
[CreateAssetMenu(fileName = "New Resource Manager", menuName = "Scriptable Objects/Resources/Resource Manager", order = 4)]
public class ResourceManager : ScriptableObject
{
    //! Array of defined ResourceTotal's. Assigned in editor and assumes each item is unique.
    [SerializeField]
    private ResourceTotal[] resourceTotals;

    //! Array of all defined Resource's in game. Assigned at runtime when not already populated.
    [SerializeField]
    private Resource[] resourceTable;

    public ResourceTotal[] ResourceTotals
    {
        get { return resourceTotals; }
    }

    //! MonoBehaviour Start method. In this class, assigns the resourceTable items.
    private void PopulateTable()
    {
        if (resourceTotals.Length > 0)
        {
            resourceTable = new Resource[resourceTotals.Length];
            for (int i = 0; i < resourceTotals.Length; i++)
            {
                resourceTable[i] = resourceTotals[i].MyResource;
            }

            Debug.Log("Resource table contains " + resourceTable.Length + " resources.");
        }
    }

    public bool TotalIsMoreOrEqual(Resource res, int amt)
    {
        bool isMore = false;

        foreach(ResourceTotal rt in resourceTotals)
        {
            if (rt.MyResource == res)
            {
                if (rt.MyAmt >= amt)
                {
                    isMore = true;
                }
            }
        }

        return isMore;
    }

    //! Method to add to a ResourceTotal in resourceTotals. Checks to ensure resource total exists first.
    public void AddToTotal(Resource res, int amt)
    {
        if (resourceTable.Length == 0) PopulateTable();

        foreach(ResourceTotal rt in resourceTotals)
        {
            if (rt.MyResource == res)
            {
                rt.MyAmt += amt;
            }
        }
    }

    //! Method to subtract from a ResourceTotal in resourceTotals. Checks to ensure resource total exists first.
    public void SubtractFromTotal(Resource res, int amt)
    {
        if (resourceTable.Length == 0) PopulateTable();

        foreach (ResourceTotal rt in resourceTotals)
        {
            if (rt.MyResource == res)
            {
                rt.MyAmt -= amt;
            }
        }
    }    
}
