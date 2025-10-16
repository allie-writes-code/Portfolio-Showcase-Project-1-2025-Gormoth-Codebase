using UnityEngine;

//! ScriptableObject class to define resource items.
[CreateAssetMenu(fileName = "New Resource Item", menuName = "Scriptable Objects/Resources/Resource Item", order = 2)]
public class ResourceItem : ScriptableObject
{
    //! Reference to prefab object for resource item.
    [SerializeField]
    private GameObject itemPrefab;

    //! Public Get method for itemPrefab.
    public GameObject ItemPrefab
    {
        get { return itemPrefab; }
    }
}
