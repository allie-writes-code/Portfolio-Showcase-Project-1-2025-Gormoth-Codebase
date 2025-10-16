using TMPro;
using UnityEngine;

//! Simple MonoBehaviour class to update a TMP_Text element in game with the resources held.
public class DisplayResourceText : MonoBehaviour
{
    //! Reference to a ResourceManager instance.
    [SerializeField]
    private ResourceManager resourceManager;

    [SerializeField]
    private TMP_Text resourceDisplayText;

    private void Update()
    {
        string s = "";

        for (int i = 0; i < resourceManager.ResourceTotals.Length; i++)
        {
            if (i != 0) s += " | ";
            s += resourceManager.ResourceTotals[i].MyResource.MyName + ": " + resourceManager.ResourceTotals[i].MyAmt;
        }

        resourceDisplayText.text = s;
    }
}
