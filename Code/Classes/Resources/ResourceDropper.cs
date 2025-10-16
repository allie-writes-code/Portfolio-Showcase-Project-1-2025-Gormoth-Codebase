using System;
using UnityEngine;

//! MonoBehaviour class, attach to game objects that can drop ResourceItem's.
//! Objects can have multiple instances of this class attached as components to conigure 
//! I.e. prefabs of destroyable resource nodes / objects - e.g. a tree, or stone.
[RequireComponent(typeof(CharacterHealth))]
public class ResourceDropper : MonoBehaviour
{
    //! Reference to a CharacterHealth instance, used to track the health of this ResourceDropper.
    private CharacterHealth myHealth;

    private void Start()
    {
        myHealth = GetComponent<CharacterHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            SimpleProjectileBehaviour behaviour = other.gameObject.GetComponent<SimpleProjectileBehaviour>();
            myHealth.Hurt(Mathf.RoundToInt(behaviour.Damage));
            Destroy(other.gameObject);
        }
    }
}


