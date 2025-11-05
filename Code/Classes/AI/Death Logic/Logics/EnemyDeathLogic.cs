using UnityEngine;

[CreateAssetMenu(fileName = "New Death Logic - Enemy", menuName = "Scriptable Objects/Logics/Death/Enemy", order = 2)]
public class EnemyDeathLogic : DeathLogic
{
    [SerializeField]
    private DelegateBroadcaster enemyDiedBroadcast;

    public override void Die()
    {
        Debug.Log("Enemy died");
        enemyDiedBroadcast.InvokeMe();
        Destroy(DeathObject);
    }
}
