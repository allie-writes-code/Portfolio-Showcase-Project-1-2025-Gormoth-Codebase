using UnityEngine;
using UnityEngine.UI;

public class PlacementDisplayBehaviour : MonoBehaviour
{
    private GameObject player;
    private PlayerPlaceableInteract playerPlace;

    private Image placementGroundImage;
    private Image placementProgressImage;
    private Image placementProgressBackgroundImage;

    private void Update()
    {
        if (!PlayerLoaded() || !ImagesLoaded()) return;
        ToggleImages();

        if (playerPlace.IsCarryingPlaceable)
        {
            if (playerPlace.IsPlacing)
            {
                UpdateProgressImage();
            }
        }

        MoveWithPlayer();
    }

    private void UpdateProgressImage()
    {
        placementProgressImage.fillAmount = playerPlace.PlacementPercentage;
    }

    private void ToggleImages()
    {
        placementGroundImage.gameObject.SetActive(playerPlace.IsCarryingPlaceable);
        placementProgressImage.gameObject.SetActive(playerPlace.IsPlacing);
        placementProgressBackgroundImage.gameObject.SetActive(playerPlace.IsPlacing);
    }

    private void MoveWithPlayer()
    {
        Vector3 newPos = new Vector3(
            Mathf.RoundToInt(player.transform.position.x),
            player.transform.position.y - 1,
            Mathf.RoundToInt(player.transform.position.z));

        transform.position = newPos;
    }

    private bool PlayerLoaded()
    {
        if (player == null) { player = GameObject.Find("Player"); }
        if (player != null && playerPlace == null) { playerPlace = player.GetComponent<PlayerPlaceableInteract>(); }

        if (player != null && playerPlace != null) return true;
        else return false;
    }

    private bool ImagesLoaded()
    {
        if (placementGroundImage == null) placementGroundImage = GameObject.Find("Placement Ground Image").GetComponent<Image>();
        if (placementProgressImage == null) placementProgressImage = GameObject.Find("Placement Progress Image").GetComponent<Image>();
        if (placementProgressBackgroundImage == null) placementProgressBackgroundImage = GameObject.Find("Placement Progress Background").GetComponent<Image>();
        
        if (placementGroundImage != null && placementProgressImage != null && placementProgressBackgroundImage != null) return true;
        else return false;
    }
}
