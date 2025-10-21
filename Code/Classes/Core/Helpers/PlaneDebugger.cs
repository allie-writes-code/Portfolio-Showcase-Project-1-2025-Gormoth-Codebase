using UnityEngine;

//! Debug helper class. Will draw a Plane in the game and scene view, if Gizmos are enabled.
//! Useful for visualising any planes created using the Plane function in Unity.
public class PlaneDebugger
{
    //! Normal is the facing direction, position, the 'distance' from camera.
    //! Just means feed it a facing direction (i.e. Vector3.up) and a position (i.e. Vector3.zero).
    public void DebugDrawPlane(Vector3 normal, Vector3 position)
    {

        Vector3 v3 = new Vector3();

        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;

        var q = Quaternion.AngleAxis(90, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green, 0, false);
        Debug.DrawLine(corner1, corner3, Color.green, 0, false);
        Debug.DrawLine(corner0, corner1, Color.green, 0, false);
        Debug.DrawLine(corner1, corner2, Color.green, 0, false);
        Debug.DrawLine(corner2, corner3, Color.green, 0, false);
        Debug.DrawLine(corner3, corner0, Color.green, 0, false);
        Debug.DrawRay(position, normal, Color.red);
    }
}
