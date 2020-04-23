using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableBehaviors : MonoBehaviour
{
    [SerializeField] private LayerMask platformlayerMask;
    private BoxCollider box;
    private Rigidbody rigid;

    void Start() // Don't use `OnEnable`, otherwise, each time your gameObject is enabled again, the function will run and you may end with several colliders to each children
    {
        foreach (Transform child in transform.transform)
        {
            //rigid = child.GetComponent<Rigidbody>();
            //rigid.useGravity = true;
            box = child.GetComponent<BoxCollider>();
            box.isTrigger = true; // false allows pushing, true does not since you can go through object. 
        }
    }
}
