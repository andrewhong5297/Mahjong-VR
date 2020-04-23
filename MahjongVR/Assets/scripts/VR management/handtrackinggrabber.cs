using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class handtrackinggrabber : OVRGrabber
{
    private OVRHand hand;
    public float pinchThreshold = 0.7f;

    protected override void Start()
    {
        base.Start();
        hand = GetComponent<OVRHand>();
    }

    public override void Update()
    {
        base.Update();
        CheckIndexPinch();
    }

    void CheckIndexPinch()
    {
        float pinchStrength = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        bool isPinching = pinchStrength > pinchThreshold;

        if (!m_grabbedObj && isPinching & m_grabCandidates.Count > 0)
            GrabBegin();
        else if (m_grabbedObj && !isPinching)
            GrabEnd();
    }

    protected override void GrabEnd() //added virtual in front of the function
    {
        if(m_grabbedObj)
        {
            Vector3 linearVelocity = (transform.position- m_lastPos) / Time.fixedDeltaTime;
            Vector3 angularVelocity = (transform.eulerAngles - m_lastRot.eulerAngles) / Time.fixedDeltaTime;

            GrabbableRelease(linearVelocity, angularVelocity);
        }

        GrabVolumeEnable(true);
    }
}
