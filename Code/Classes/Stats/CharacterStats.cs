using UnityEngine;

//! ScriptableObject class for maintaining stats assigned to character objects in game.
[CreateAssetMenu(fileName = "New Character Stats", menuName = "Scriptable Objects/Stats/Chacter Stats", order = 2)]
public class CharacterStats : ScriptableObject
{
    //! Damage Stat references.
    [SerializeField]
    private Stat damage;

    //! Carry amount Stat references.
    [SerializeField]
    private Stat carry;

    //! Cooldown speed Stat reference.
    [SerializeField]
    private Stat cooldown;

    //! Move speed stat references.
    [SerializeField]
    private Stat moveSpeed;

    //! Public Get method for damage Stat.
    public int Damage { get { return damage.ValueInt; } }

    //! Public Get method for carry Stat.
    public int Carry { get { return carry.ValueInt; } }

    //! Public Get method for cooldown speed Stat.
    public float Cooldown { get { return cooldown.ValueFloat; } }

    //! Public Get method for move speed Stat.
    public float MoveSpeed { get { return moveSpeed.ValueFloat; } }
}