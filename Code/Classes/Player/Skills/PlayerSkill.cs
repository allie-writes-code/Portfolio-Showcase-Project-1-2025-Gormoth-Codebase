using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerSkill : ScriptableObject
{
    //! SkillCooldown reference. The cooldown for this skill.
    [SerializeField]
    private SkillCooldown cooldown;

    //! Reference to a GameObject prefab, used whenever the skill spawns an object (e.g. a projectile).
    [SerializeField]
    private GameObject skillPrefab;

    //! Stat reference, stat used for this skill.
    [SerializeField]
    private Stat skillStat;

    //! GameObject reference to track the player object.
    private GameObject playerObject;

    public SkillCooldown Cooldown { get { return cooldown; } }
    public GameObject SkillPrefab { get { return skillPrefab; } }
    public Stat SkillStat { get { return skillStat; } }

    public GameObject PlayerObject { get { return playerObject; } set { playerObject = value; } }

    public abstract void UseSkill();
}
