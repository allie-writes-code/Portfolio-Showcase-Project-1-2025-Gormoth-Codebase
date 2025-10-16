using UnityEngine;
using UnityEngine.Events;

//! Delegate Pattern class, Listener, scriptable object.
//! Will subscribe any passed functions to the defined broadcaster.
/*! In the script you want to have listening for a broadcast.
 * Reference an instance of a DelegateListener - e.g.
 * 
 * [SerializeField] private DelegateListener myListener;
 * 
 * Register and deregister the function you want to run from this script somewhere at the start and end of this class' life - e.g.
 * 
 * private void OnEnable(){myListener.RegisterFunction(myFunctionIWantToRun);}
 * 
 * private void OnDisable(){myListener.DeregisterFunction(myFunctionIWantToRun);}
 * 
 * Elsewhere in the class, you'd have a function defined for 'myFunctionIWantToRun'.
 */
[CreateAssetMenu(fileName = "New Delegate Listener", menuName = "Scriptable Objects/Helpers/Delegate Subscribers/Listener", order = 1)]
public class DelegateListener : ScriptableObject
{
    //! Reference to a DelegateBroadcaster instance.
    [SerializeField]
    private DelegateBroadcaster broadcaster;

    //! Register functions (UnityAction's) via this function to subscribe them to the associated broadcaster.
    public void RegisterFunction(UnityAction f) 
    {
        broadcaster.OnInvocation += f;
    }

    //! If an object is disabled or destroyed, deregister it via this function.
    public void DeregisterFunction(UnityAction f)
    {
        broadcaster.OnInvocation -= f;
    }
}
