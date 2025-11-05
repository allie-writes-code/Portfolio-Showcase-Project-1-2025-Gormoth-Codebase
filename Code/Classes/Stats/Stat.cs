using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Scriptable Objects/Stats/Stat", order = 1)]
public class Stat : ScriptableObject
{
    //! Value to define the 'default' for this stat. Will reset to this on enable.
    [SerializeField]
    private float defaultValue;

    private float value;

    private void OnEnable()
    {
        value = defaultValue;
    }

    [SerializeField]
    private Stat xPlier;

    //! Public Get method for stat value.
    public float ValueFloat
    {
        get 
        {
            float f;
            if (xPlier != null) 
            {
                f = value * xPlier.ValueFloat;
            }
            else
            {
                f = value;
            }
            //Debug.Log("Returning " + f + " as a float from stat " + this.name);
            return f;
        }
    }

    public int ValueInt
    {
        get
        {
            int i;
            if (xPlier != null) 
            { 
                i = Mathf.RoundToInt(value * xPlier.ValueFloat); 
            }
            else
            {
                i = Mathf.RoundToInt(value);
            }
            //Debug.Log("Returning " + i + " as an int from stat " + this.name);
            return i;
        }
    }
}
