using UnityEngine;

public class SimpleProjectileBehaviour : MonoBehaviour
{
    //! Float variable, move speed for this projectile.
    [SerializeField]
    private Stat moveSpeedStat;

    //! Float variable, how long will this projectile live before destroying itself.
    [SerializeField]
    private float timeToLive = 3f;
    private float aliveTimer = 0;

    //! Vector3 to store direction this projectile is moving in.
    private Vector3 direction;

    //! Bool variable, once set to true, this projectile will start moving.
    private bool alive = false;

    private float damage = 0;

    public float Damage
    {
        get { return damage; }
    }

    public void FireProjectile(Vector3 dir, float dmg)
    {
        direction = dir;
        damage = dmg;
        alive = true;
    }

    private void Update()
    {
        if (alive)
        {
            aliveTimer += (Time.deltaTime);
            if (aliveTimer > timeToLive) Destroy(this.gameObject);
            
            transform.Translate(direction * moveSpeedStat.Value * (Time.deltaTime * 0.5f));
        }
    }
}
