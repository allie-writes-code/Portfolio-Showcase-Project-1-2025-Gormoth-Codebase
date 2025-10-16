using System;
using UnityEngine;

//! ScriptableObject class for defining stats. Stat name is defined via object / file name.
[CreateAssetMenu(fileName = "New Stat", menuName = "Scriptable Objects/Stats/Stat", order = 1)]
public class Stat : ScriptableObject
{
    //!  Float value, set in editor, no access at runtime to change value.
    //! Return for this value, by default, gives the adjusted value of: (base + add) * xplier 
    [SerializeField]
    private float baseAmt;

    [SerializeField]
    private float addAmt;
    [SerializeField]
    private float xPlier;


    //! Public Get method for stat value.
    public float Value
    {
        get {  return (baseAmt + addAmt) * xPlier; }
    }

    //! Public Get method for baseAmt.
    public float BaseValue
    {
        get { return baseAmt; }
    }

    //! Public Get and Set method for addAmt.
    public float AddAmt
    {
        get { return addAmt; }
        set { addAmt = value; }
    }

    //! Public Get and Set method for xPlier.
    public float XPlier
    {
        get { return xPlier; }
        set { xPlier = value; }
    }
}
