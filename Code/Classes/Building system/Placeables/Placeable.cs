using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable", menuName = "Scriptable Objects/Building System/Placeable", order = 1)]
public class Placeable : ScriptableObject
{
    public GameObject placeableObjectPrefab;
    public GameObject placeableBuildingPrefab;

    [SerializeField]
    private Stat distanceFromCoreBase;

    public float DistanceFromCore
    {
        get { return distanceFromCoreBase.ValueFloat; }
    }

    [SerializeField]
    private CostIngredient placeableCost;

    public CostIngredient PlaceableCost
    {
        get { return placeableCost; }
    }

    [Serializable]
    public class CostIngredient
    {
        public Resource resource;
        public Stat costBase;
    }
}
