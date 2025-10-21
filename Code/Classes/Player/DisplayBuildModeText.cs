using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DisplayBuildModeText : MonoBehaviour
{
    [SerializeField]
    private BuildingManager buildingManager;

    [SerializeField]
    private TMP_Text buildingModeDisplayText;

    private void Update()
    {
        if (buildingManager.BuildModeOn)
        {
            if (!buildingModeDisplayText.IsActive()) { buildingModeDisplayText.gameObject.SetActive(true); }

            buildingModeDisplayText.text = "Building Mode - Selected: " + buildingManager.GetSelectedBuildingDetails().name;
        }
        else
        {
            if (buildingModeDisplayText.IsActive()) { buildingModeDisplayText.gameObject.SetActive(false); }
        }
    }
}
