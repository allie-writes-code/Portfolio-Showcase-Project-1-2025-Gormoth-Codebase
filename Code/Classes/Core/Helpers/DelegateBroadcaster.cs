using UnityEngine;
using UnityEngine.Events;

//! Delegate Pattern class, Broadcaster, scriptable object.
//! Will broadcast and run any subscribed functions when invoked.
/*! In the script you want to have broadcast:
 * Reference an instance of DeleteBroadcaster - e.g.
 * 
 * [SerializeField]
 * private DelegateBroadcaster myBroadcaster;
 * 
 * Then invoke the broadcaster from a function - e.g.
 * 
 * private void ExampleFunction(){myBroadcaster.InvokeMe();}
 * 
 * Don't forget to also configure a DelegateListener!
 */
[CreateAssetMenu(fileName = "New Delegate Broadcaster", menuName = "Scriptable Objects/Helpers/Delegate Subscribers/Broadcaster", order = 2)]
public class DelegateBroadcaster : ScriptableObject
{
    private UnityAction onInvocation;

    //! Add or subtract UnityAction's from this to subscribe them.
    public UnityAction OnInvocation 
    {  
        get { return onInvocation; } 
        set { onInvocation = value; }
    }

    //! Call this from wherever a change is made that you want to monitor for.
    public void InvokeMe()
    {
        onInvocation?.Invoke();
    }
}
