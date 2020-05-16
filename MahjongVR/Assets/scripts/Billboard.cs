using Doozy.Engine.Utils.ColorModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pivotaxis { X, Y, Z, XY, XZ, YZ, Free}
public class Billboard : MonoBehaviour
{
    private Pivotaxis pivotaxis = Pivotaxis.XY;
    public Pivotaxis PivotAxis { get { return pivotaxis; } set { pivotaxis = value; } }
    
    [SerializeField]
    private Transform targetTransform;

    private void OnEnable() //called when gameobject (UI) is enabled Unity thing
    {
        if(targetTransform==null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate() //every frame when behavior is enabled?
    {
        Vector3 directionToTarget = targetTransform.position - transform.position; //direction towards camera
        switch(PivotAxis)
        {
            case Pivotaxis.X:
                directionToTarget.x = 0.0f;
                break;
            case Pivotaxis.Y:
                directionToTarget.y = 0.0f;
                break;
            case Pivotaxis.Z:
                directionToTarget.x = 0.0f;
                directionToTarget.y = 0.0f;
                break;
            case Pivotaxis.XY:
                break;
            case Pivotaxis.XZ:
                directionToTarget.x = 0.0f;
                break;
            case Pivotaxis.YZ:
                directionToTarget.y = 0.0f;
                break;
            case Pivotaxis.Free:
            default: break;
        }

        transform.rotation = Quaternion.LookRotation(-directionToTarget);

    }
}
