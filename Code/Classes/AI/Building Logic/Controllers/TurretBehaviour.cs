using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField]
    private BuildingLogic myLogic;

    private float cooldownCurrentTime;

    //! Unlike where we use cooldowns with PlayerSkill's, we do the cooldown calculation locally.
    //! This is because SkillCooldown's are ScriptableObject's and won't be separate instances.
    [SerializeField]
    private Stat cooldown;

    private void Update()
    {
        if (cooldownCurrentTime >= cooldown.ValueFloat)
        {
            cooldownCurrentTime = 0;
            myLogic.Activate(this.gameObject);
        }
        else
        {
            if (cooldownCurrentTime > cooldown.ValueFloat)
            {
                cooldownCurrentTime = cooldown.ValueFloat;
            }
            else
            {
                cooldownCurrentTime += Time.deltaTime;
            }
        }
    }
}
