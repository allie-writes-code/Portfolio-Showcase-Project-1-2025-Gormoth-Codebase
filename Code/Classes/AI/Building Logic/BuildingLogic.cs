using UnityEngine;

public abstract class BuildingLogic : ScriptableObject
{
    public abstract void Activate();
    public abstract void Activate(GameObject buildingObject);
}
