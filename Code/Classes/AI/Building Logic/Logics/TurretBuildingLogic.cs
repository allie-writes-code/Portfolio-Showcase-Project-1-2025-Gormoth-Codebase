using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Logic - Turret", menuName = "Scriptable Objects/Logics/Building/Turret", order = 1)]
public class TurretBuildingLogic : BuildingLogic
{
    [SerializeField]
    private float targetCheckRadius = 1f;

    [SerializeField]
    private LayerMask enemiesLayer;
    public override void Activate(){throw new System.NotImplementedException();}

    public override void Activate(GameObject buildingObject)
    {
        FireProjectile(buildingObject);
    }

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Stat turretDamageStat;

    private void FireProjectile(GameObject buildingObject)
    {
        
        Collider[] hitColliders = Physics.OverlapSphere(buildingObject.transform.position, targetCheckRadius, enemiesLayer);

        if (hitColliders.Length > 0)
        {
            // Sort colliders by distance to the sphere's center
            Collider closestCollider = hitColliders
                .OrderBy(c => Vector3.SqrMagnitude(buildingObject.transform.position - c.transform.position))
                .FirstOrDefault();

            if (closestCollider != null)
            {
                GameObject newProjectile = Instantiate(projectilePrefab, buildingObject.transform.position, projectilePrefab.transform.rotation);
                SimpleProjectileBehaviour behaviour = newProjectile.GetComponent<SimpleProjectileBehaviour>();
                behaviour.FireProjectile((closestCollider.transform.position - buildingObject.transform.position).normalized, turretDamageStat.ValueInt);
            }
        }
    }
}
