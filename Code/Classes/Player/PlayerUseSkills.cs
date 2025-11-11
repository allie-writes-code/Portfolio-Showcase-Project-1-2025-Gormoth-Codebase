using System.Collections.Generic;
using UnityEngine;

//! MonoBehaviour class used to control the player's skill loop in game.
public class PlayerUseSkills : MonoBehaviour
{
    [SerializeField]
    private PlayerSkill[] startingSkills;

    private List<PlayerSkill> activeSkills = new List<PlayerSkill>();

    private void Start()
    {
        foreach (PlayerSkill skill in startingSkills)
        {
            activeSkills.Add(skill);
        }
    }

    private void Update()
    {
        foreach(PlayerSkill skill in activeSkills)
        {
            if (skill.PlayerObject == null) skill.PlayerObject = this.gameObject;

            if (skill.Cooldown.IsReady())
            {
                skill.UseSkill();
            }
            else
            {
                skill.Cooldown.Timer += (Time.deltaTime);
            }
        }
    }
}
