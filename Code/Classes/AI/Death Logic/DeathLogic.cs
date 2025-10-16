using UnityEngine;

//! Parent abstract class for handling death logic of objects with health.
public abstract class DeathLogic : ScriptableObject
{
    private GameObject deathObject;

    public GameObject DeathObject {  get { return deathObject; } set { deathObject = value; } }

    public abstract void Die();
}
