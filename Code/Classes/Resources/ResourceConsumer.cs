using UnityEngine;

public class ResourceConsumer : MonoBehaviour
{
    private Resource consumedResource;

    private int currentTotalStored = 0;
    private int currentMaxRequired = 0;

    private bool isFull = false;

    public bool IsFull
    {
        get 
        {
            UpdateIfFull();
            return isFull; 
        }
    }

    public int CurrentMaxRequired
    {
        get { return currentMaxRequired; }
        set { currentMaxRequired = value; }
    }

    public Resource ConsumedResource
    {
        get { return consumedResource; }
        set { consumedResource = value; }
    }

    public bool CanAfford(int howMuch)
    {
        if (currentTotalStored >= howMuch) return true;
        return false;
    }

    public void SubtractFromTotal(int amt)
    {
        currentTotalStored -= amt;
        if (currentTotalStored < 0) { currentTotalStored = 0; }
        UpdateIfFull();
    }

    public void ConsumeResource(Resource res)
    {
        Debug.Log("Consuming resouce: " + res);
        if (res == consumedResource)
        {
            Debug.Log("Resource match!");
            currentTotalStored++;
            UpdateIfFull();
        }
    }

    private void UpdateIfFull()
    {
        if (currentTotalStored >= currentMaxRequired) isFull = true;
        else isFull = false;
    }
}
