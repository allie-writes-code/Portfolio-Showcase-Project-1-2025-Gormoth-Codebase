using UnityEngine;

[CreateAssetMenu(fileName = "New Death Logic - Enemy", menuName = "Scriptable Objects/Logics/Death/Enemy", order = 2)]
public class EnemyDeathLogic : DeathLogic
{
    public override void Die()
    {
        Destroy(DeathObject);
    }
}
