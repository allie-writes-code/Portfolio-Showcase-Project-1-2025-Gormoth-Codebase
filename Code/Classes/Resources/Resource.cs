using UnityEngine;

//! Scriptable Object class for defining resources.
//! None of the values in this class should change at runtime, all set in editor.
[CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Objects/Resources/Resource", order = 1)]
public class Resource : ScriptableObject
{
    //! Reference to the PickupItem for the Resource.
    [SerializeField]
    private ResourceItem resourceItem;

    //! Name of this Resource.
    [SerializeField]
    private string myName;

    //! An index value for the resource.
    [SerializeField]
    private int myIndex;

    //! Public Get method for myPrefab.
    public ResourceItem ResourceItem
    {
        get { return resourceItem; }
    }

    //! Public Get method for myName.
    public string MyName
    {
        get { return myName; }
    }

    //! Public Get method for myIndex.
    public int MyIndex
    {
        get { return myIndex; }
    }
}
