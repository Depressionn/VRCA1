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

public class FishingRod : Singleton<FishingRod>
{
    public GameObject hook;
    public GameObject throwingPoint;
    public LineRenderer rope;
    public ReelLogic reel;
    public GameObject catchPoint;

    //FSM for rodstate
    private RodState m_rodState;
    public RodState CurrentRodState { get { return m_rodState; } }

    private Vector3 oldpos;

    Fish currentFish;

    public event Action WaitForBite;
    
    // Start is called before the first frame update
    void Start()
    {
        WaitToCast();
        WaitForBite += WaitForBiteLocal;
        reel.eOnSpinReel += OnSpinReel;
    }

    public void WaitToCast()
    {
        m_rodState = RodState.WaitingToCast;
        hook.GetComponent<Rigidbody>().isKinematic = true;
        rope.enabled = false;
        UIManager.Instance.HideCatchScreen();
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
        hook.GetComponent<Rigidbody>().velocity = Vector3.Scale(v, new Vector3(2, 0, 2)) + new Vector3(0, 1, 0);
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

    public void Bite(Fish fish)
    {

        m_rodState = RodState.Biting;
        currentFish = fish;
    }

    public void CanReelIn()
    {
        m_rodState = RodState.ReelingIn;
    }

    public void WaitForBiteEvent()
    {
        WaitForBite?.Invoke();
    }

    public void Caught()
    {
        m_rodState = RodState.Caught;
        UIManager.Instance.ShowCatchScreen(currentFish);
    }

    public void OnSpinReel(float spin)
    {

        if ((int)m_rodState > 2)
        {
            if (Vector3.Distance(hook.transform.position, new Vector3(transform.position.x, hook.transform.position.y, transform.position.z)) < 4)
            {
                if (m_rodState == RodState.ReelingIn || m_rodState == RodState.Caught) Caught();
                else WaitToCast();
            }
            Vector3 target = new Vector3();
            if (m_rodState == RodState.Caught)
            {
                target = catchPoint.transform.position;
            }
            else
            {
                target = new Vector3(transform.position.x, hook.transform.position.y, transform.position.z);
            }
            hook.transform.position = Vector3.MoveTowards(hook.transform.position, target, 0.001f * spin * Time.deltaTime);
            UIManager.Instance.SetHookDistance(Vector3.Distance(transform.position, hook.transform.position));
        }
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
                hook.transform.position += new Vector3(UnityEngine.Random.Range(-20f, 20f), 0f, UnityEngine.Random.Range(-20f, 20f)) * Time.deltaTime * currentFish.aggressiveness;
                HapticImpulse(currentFish.nibbleStrength * 0.6f + 0.4f, Time.deltaTime);
                break;
            case RodState.ReelingIn:
                hook.transform.position += new Vector3(UnityEngine.Random.Range(-20f, 20f), 0f, UnityEngine.Random.Range(-20f, 20f + 2f * currentFish.aggressiveness)) * Time.deltaTime * currentFish.aggressiveness;
                HapticImpulse(currentFish.nibbleStrength * 0.4f + 0.2f, Time.deltaTime);
                break;
            case RodState.Caught:

                break;
        }
    }
}
