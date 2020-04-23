using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundcheck : MonoBehaviour
{
    [SerializeField] private LayerMask platformlayerMask;
    private BoxCollider box;
    private Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        box = gameObject.GetComponent<BoxCollider>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // use for snapback to original position? 
        // if not grabbed and not in played box, snap back to where it was taken from. 
    }

    private bool OnGround()
    {
        float extraHeightText = 0.1f; //to avoid mathematical perfection issues
        RaycastHit hit;

        bool isHit = Physics.Raycast(transform.position, -transform.up, out hit, extraHeightText, platformlayerMask);

        //Physics.BoxCast(box.bounds.center, box.bounds.size, Vector3.down, transform.rotation, extraHeightText, platformlayerMask);

        return isHit;
    }

    void OnDrawGizmos()
    {
        float extraHeightText = 0.01f; //to avoid mathematical perfection issues
        RaycastHit hit;

        bool isHit = Physics.Raycast(transform.position,-transform.up, out hit, extraHeightText, platformlayerMask);

        //Physics.BoxCast(box.bounds.center, box.bounds.size, Vector3.down, transform.rotation, extraHeightText, platformlayerMask);

        if (isHit)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, -transform.up * extraHeightText);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * extraHeightText);
        }
    }
}
