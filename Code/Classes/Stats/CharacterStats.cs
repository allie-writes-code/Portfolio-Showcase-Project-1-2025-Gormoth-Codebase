using UnityEngine;

//! ScriptableObject class for maintaining stats assigned to character objects in game.
[CreateAssetMenu(fileName = "New Character Stats", menuName = "Scriptable Objects/Stats/Chacter Stats", order = 2)]
public class CharacterStats : ScriptableObject
{
    //! Damage Stat reference.
    [SerializeField]
    private Stat damage;

    //! Carry amount Stat reference
    [SerializeField]
    private Stat carry;

    //! Cooldown speed Stat reference.
    [SerializeField]
    private Stat cooldown;

    //! Move speed stat reference.
    [SerializeField]
    private Stat moveSpeed;

    //! Public Get method for damage Stat.
    public Stat Damage { get { return damage; } }

    //! Public Get method for carry Stat.
    public Stat Carry { get { return carry; } }

    //! Public Get method for cooldown speed Stat.
    public Stat Cooldown { get { return cooldown; } }

    //! Public Get method for move speed Stat.
    public Stat MoveSpeed { get { return moveSpeed; } }
}