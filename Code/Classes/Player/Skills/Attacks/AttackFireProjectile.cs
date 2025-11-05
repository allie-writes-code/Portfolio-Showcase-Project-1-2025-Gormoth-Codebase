using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Attack Skill - Fire Projectile", menuName = "Scriptable Objects/Skills/Attacks/Fire Projectile Skill", order = 1)]
public class AttackFireProjectile : PlayerSkill
{
    //! Float to define radius to check for targets.
    [SerializeField]
    private float targetCheckRadius = 5f;

    //! LayerMask reference to define enemy target layer.
    [SerializeField]
    private LayerMask enemiesLayer;

    //! LayerMask reference to define ResoureDropper layer.
    [SerializeField]
    private LayerMask droppersLayer;

    public override void UseSkill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(PlayerObject.transform.position, targetCheckRadius, enemiesLayer);

        if (hitColliders.Length > 0)
        {
            Debug.Log("Enemies found: " + hitColliders.Length);
            // Sort colliders by distance to the sphere's center
            Collider closestCollider = hitColliders
                .OrderBy(c => Vector3.SqrMagnitude(PlayerObject.transform.position - c.transform.position))
                .FirstOrDefault();

            if (closestCollider != null)
            {
                GameObject newProjectile = Instantiate(SkillPrefab, PlayerObject.transform.position, SkillPrefab.transform.rotation);
                SimpleProjectileBehaviour behaviour = newProjectile.GetComponent<SimpleProjectileBehaviour>();
                behaviour.FireProjectile((closestCollider.transform.position - PlayerObject.transform.position).normalized, SkillStat.ValueInt);

                Cooldown.Reset();
            }
        }
        else
        {
            hitColliders = Physics.OverlapSphere(PlayerObject.transform.position, targetCheckRadius, droppersLayer);

            if (hitColliders.Length > 0)
            {
                // Sort colliders by distance to the sphere's center
                Collider closestCollider = hitColliders
                    .OrderBy(c => Vector3.SqrMagnitude(PlayerObject.transform.position - c.transform.position))
                    .FirstOrDefault();

                if (closestCollider != null)
                {
                    GameObject newProjectile = Instantiate(SkillPrefab, PlayerObject.transform.position, SkillPrefab.transform.rotation);
                    SimpleProjectileBehaviour behaviour = newProjectile.GetComponent<SimpleProjectileBehaviour>();
                    behaviour.FireProjectile((closestCollider.transform.position - PlayerObject.transform.position).normalized, SkillStat.ValueInt);

                    Cooldown.Reset();
                }
            }
        }
    }
}
