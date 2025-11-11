using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Scriptable Objects/Stats/Stat", order = 1)]
public class Stat : ScriptableObject
{
    //! Value to define the 'default' for this stat.
    //! This value will be saved to the JSON file via the GameResourceManager.
    [SerializeField]
    private float defaultValue;

    //! This is the value returned in game when anything gets the Stat.
    private float myValue;

    //! Reference to a Stat to use as a multiplier. If not present, will be ignored.
    [SerializeField]
    private Stat xPlier;

    //! Boolean set in editor to flag that this Stat is used as a bool.
    //! This is really only to make the formatting nicer in the saved JSON output.
    [SerializeField]
    private bool isBoolValue;

    //! Note set by developer to show against Stat when saved to JSON by the GameResourceManager.
    //! End users should not be able to change this note, only ever set in editor.
    [SerializeField]
    private string moddingNote;

    //! Public Get method for Stat value as a float.
    public float ValueFloat
    {
        get 
        {
            float f;
            if (xPlier != null) 
            {
                f = myValue * xPlier.ValueFloat;
            }
            else
            {
                f = myValue;
            }

            return f;
        }
    }

    //! Public Get method for Stat value as an int.
    public int ValueInt
    {
        get
        {
            int i;
            if (xPlier != null) 
            { 
                i = Mathf.RoundToInt(myValue * xPlier.ValueFloat); 
            }
            else
            {
                i = Mathf.RoundToInt(myValue);
            }

            return i;
        }
    }

    //! Public Get method for returning this Stat value as a boolean.
    //! For a boolean stat, 0 = false and 1 = any non 0 number.
    public bool ValueBool
    {
        get
        {
            return Convert.ToBoolean(myValue);
        }
    }

    public float DefaultValue 
    { 
        get 
        { 
            return defaultValue; 
        } 
        set 
        { 
            defaultValue = value; 
            myValue = value;
        } 
    }

    public string ModdingNote { get { return moddingNote; } }

    public bool IsBoolValue {  get { return isBoolValue; } }
}
