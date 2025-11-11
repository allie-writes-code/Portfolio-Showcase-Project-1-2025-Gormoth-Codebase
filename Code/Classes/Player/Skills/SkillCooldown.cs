using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Cooldown", menuName = "Scriptable Objects/Skills/Skill Cooldown", order = 2)]
public class SkillCooldown : ScriptableObject
{
    //! Base cooldown time.
    [SerializeField]
    private Stat skillCooldownBase;

    //! Float, the current timer for the cooldown. Used to track the cooldown's progress.
    private float timer;

    public float Timer
    {
        get { return timer; } 
        set 
        { 
            timer = value;
            if (timer > skillCooldownBase.ValueFloat) timer = skillCooldownBase.ValueFloat;
        }
    }

    public void Reset()
    {
        timer = 0;
    }

    public bool IsReady()
    {
        bool ready = false;
        
        if (timer >= skillCooldownBase.ValueFloat)
        {
            ready = true;
        }

        return ready;
    }
}
