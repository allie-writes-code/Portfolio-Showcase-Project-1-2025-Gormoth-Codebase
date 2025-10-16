using System;
using UnityEngine;

//! ScriptableObject class for handling shared totals of Resources in game.
[CreateAssetMenu(fileName = "New Resource Total", menuName = "Scriptable Objects/Resources/Resource Total", order = 3)]
public class ResourceTotal : ScriptableObject
{
    //! Reference to the Resource tracked by this total. Assigned in editor.
    [SerializeField]
    private Resource myResource;

    //! The total amt of this Resource held in game. Non-serialised to ensure resets between executions.
    [NonSerialized]
    private int myAmt;

    //! Public Get / Set method for myAmt.
    public int MyAmt
    {
        get { return myAmt; }
        set { myAmt = value; }
    }

    //! Public Get method for myResource. It should never change at runtime but me may want to check it.
    public Resource MyResource
    {
        get { return myResource; }
    }
}
