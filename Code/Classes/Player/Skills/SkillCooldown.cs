using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Cooldown", menuName = "Scriptable Objects/Skills/Skill Cooldown", order = 2)]
public class SkillCooldown : ScriptableObject
{
    //! Base cooldown time.
    [SerializeField]
    private int baseAmt = 0;

    //! Stat reference. The stat used to adjust cooldown time - e.g. player cooldown speed.
    [SerializeField]
    private Stat cooldownStat;

    //! Float, the current timer for the cooldown. Used to track the cooldown's progress.
    private float timer;

    public float Timer
    {
        get { return timer; } 
        set 
        { 
            timer = value;
            if (timer > (baseAmt - (baseAmt * cooldownStat.ValueFloat))) timer = (baseAmt - (baseAmt * cooldownStat.ValueFloat));
        }
    }

    public void Reset()
    {
        timer = 0;
    }

    public bool IsReady()
    {
        bool ready = false;
        // Debug.Log("Cooldown check: " + timer + " checked against: " + (baseAmt - (baseAmt * cooldownStat.ValueFloat)) + " - from " + this.name);
        if (timer >= (baseAmt - (baseAmt * cooldownStat.ValueFloat)))
        {
            ready = true;
        }

        return ready;
    }
}
