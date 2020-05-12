using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR;
using System;

public enum RodState
{
    WaitingToCast,
    Casting,
    ReelReleased,
    WaitingForBite,
    Biting,
    ReelingIn,
    Caught
}

public class FishingRod : MonoBehaviour
{
    public GameObject hook;
    public GameObject throwingPoint;
    public LineRenderer rope;

    //FSM for rodstate
    private RodState m_rodState;
    public RodState CurrentRodState { get { return m_rodState; } }

    private Vector3 oldpos;

    private float biteStrength;

    public event Action WaitForBite;
    
    // Start is called before the first frame update
    void Start()
    {
        WaitToCast();
        WaitForBite += WaitForBiteLocal;
    }

    public void WaitToCast()
    {
        m_rodState = RodState.WaitingToCast;
        hook.GetComponent<Rigidbody>().isKinematic = true;
        rope.enabled = false;
    }

    public void Casting()
    {
        m_rodState = RodState.Casting;
    }

    public void ReelReleased()
    {
        m_rodState = RodState.ReelReleased;
        Vector3 v = (transform.position - oldpos)/Time.deltaTime;
        Debug.Log(v);
        hook.GetComponent<Rigidbody>().isKinematic = false;
        hook.GetComponent<Rigidbody>().velocity = Vector3.Scale(v, new Vector3(1, 0, 2));
        rope.enabled = true;
    }

    public void WaitForBiteLocal()
    {
        Debug.Log("waitforbite");
        hook.GetComponent<Rigidbody>().isKinematic = true;
        m_rodState = RodState.WaitingForBite;
    }

    public void LandedInWater()
    {
        if (m_rodState == RodState.ReelReleased)
        {
            WaitForBite();
        }
    }

    void HapticImpulse(float strength, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        HapticCapabilities capabilities;
        if (device.TryGetHapticCapabilities(out capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                Debug.Log("Impulse sent");
                uint channel = 0;
                device.SendHapticImpulse(channel, strength, duration);
            }
        }
    }

    public void Nibble(float strength)
    {
        HapticImpulse(strength, 0.1f);
    }

    public void Bite(float strength)
    {
        m_rodState = RodState.Biting;
        biteStrength = strength;
    }

    // Update is called once per frame
    void Update()
    {
        if (rope.enabled)
        {
            rope.SetPosition(0, throwingPoint.transform.position);
            rope.SetPosition(1, hook.transform.position);
        }

        //Manage Inputs
        bool holdingReel = Input.GetButton("XRI_Right_GripButton");
        bool reset = Input.GetButton("XRI_Right_TriggerButton");

        if (reset)
        {
            WaitToCast();
        }

        switch (m_rodState)
        {
            case RodState.WaitingToCast:
                hook.transform.position = throwingPoint.transform.position;
                if (holdingReel)
                {
                    Casting();
                }
                break;
            case RodState.Casting:
                hook.transform.position = throwingPoint.transform.position;
                if (!holdingReel)
                {
                    ReelReleased();
                } else
                {
                    oldpos = transform.position;
                }
                break;
            case RodState.ReelReleased:
                break;
            case RodState.WaitingForBite:
                break;
            case RodState.Biting:
                HapticImpulse(biteStrength, Time.deltaTime);
                break;
        }
    }
}
