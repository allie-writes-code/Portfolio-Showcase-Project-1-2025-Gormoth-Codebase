using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buildable", menuName = "Scriptable Objects/Building System/Buildable", order = 1)]
public class Buildable : ScriptableObject
{
    [SerializeField]
    private GameObject buildingPrefab;

    [SerializeField]
    private Material buildingAllowedMaterial;
    [SerializeField]
    private Material buildingDisabledMaterial;

    [SerializeField]
    private ResourceIngredient[] buildableRecipe;

    public GameObject BuildingPrefab { get { return buildingPrefab; } }
    public Material BuildingAllowedMaterial { get { return buildingAllowedMaterial; } }
    public Material BuildingDisabledMaterial { get { return buildingDisabledMaterial; } }

    public ResourceIngredient[] BuildableRecipe { get { return buildableRecipe; } }

    [Serializable]
    public class ResourceIngredient
    {
        public Resource resource;
        public int amount;
    }
}
