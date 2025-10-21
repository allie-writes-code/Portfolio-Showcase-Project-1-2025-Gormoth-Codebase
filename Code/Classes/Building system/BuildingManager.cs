using System;
using TMPro;
using UnityEngine;

//! ScriptableObject class for defining a building manager. This class is responsible for managing the in game building system.
[CreateAssetMenu(fileName = "New Building Manager", menuName = "Scriptable Objects/Building System/Manager", order = 10)]
public class BuildingManager : ScriptableObject
{
    //! Reference to a WorldGridManager instance. We use this grid for getting and setting buildable areas.
    [SerializeField]
    private WorldGridManager gridManager;

    //! Reference to a ResourceManager instance. Used to check held resource totals for building.
    [SerializeField]
    private ResourceManager resourceManager;

    private Vector3 buildPos;

    [NonSerialized]
    private bool buildModeOn = false;

    public bool BuildModeOn {  get { return buildModeOn; } }

    [SerializeField]
    private Buildable[] availableBuildables;

    [NonSerialized]
    private int selectedBuildableID = 0;
    private GameObject currentlySpawnedBuildable;

    public Vector3 BuildPos
    {
        get { return buildPos; }
        set 
        {
            buildPos = new Vector3(Mathf.RoundToInt(value.x),
                Mathf.RoundToInt(value.y),
                Mathf.RoundToInt(value.z));
            
            UpdateBuildPosition();
        }
    }

    public void Build()
    {
        Buildable b = availableBuildables[selectedBuildableID];
        if (gridManager.IsNodeClear(buildPos))
        {
            if (CanAffordBuilding(b))
            {
                GameObject newBuilding = Instantiate(b.BuildingPrefab, buildPos,
                b.BuildingPrefab.transform.rotation);

                newBuilding.name = b.name;

                gridManager.OccupyNode(buildPos);

                foreach(Buildable.ResourceIngredient ri in b.BuildableRecipe)
                {
                    resourceManager.SubtractFromTotal(ri.resource, ri.amount);
                }
            }
        }
    }

    private bool CanAffordBuilding(Buildable b)
    {
        bool canAfford = true;

        foreach (Buildable.ResourceIngredient ri in b.BuildableRecipe)
        {
            if (!resourceManager.TotalIsMoreOrEqual(ri.resource, ri.amount))
            {
                canAfford = false;
            }
        }

        return canAfford;
    }

    private void UpdateBuildPosition()
    {
        if (!buildModeOn) return;

        if (currentlySpawnedBuildable == null) { currentlySpawnedBuildable = Instantiate(availableBuildables[selectedBuildableID].BuildingPrefab); }
        
        currentlySpawnedBuildable.transform.position = buildPos;

        if (gridManager.IsNodeClear(buildPos) && CanAffordBuilding(availableBuildables[selectedBuildableID])) 
        {
            currentlySpawnedBuildable.GetComponent<Renderer>().material = availableBuildables[selectedBuildableID].BuildingAllowedMaterial; 
        }
        else 
        {
            currentlySpawnedBuildable.GetComponent<Renderer>().material = availableBuildables[selectedBuildableID].BuildingDisabledMaterial; 
        }
    }

    public Buildable GetSelectedBuildingDetails()
    {
        return availableBuildables[selectedBuildableID];
    }

    public void CycleSelected(int increment)
    {
        selectedBuildableID += increment;
        if (selectedBuildableID >= availableBuildables.Length) selectedBuildableID = 0;
        else if (selectedBuildableID < 0) selectedBuildableID = availableBuildables.Length - 1;
    }

    public void ToggleBuildMode(bool isOn)
    {
        buildModeOn = isOn;
        if (!buildModeOn && currentlySpawnedBuildable != null) { Destroy(currentlySpawnedBuildable); }
    }
}
