using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using Unity.XR;

public enum RodState
{
    WaitingToCast,
    Casting,
    ReelReleased
}

public class FishingRod : Singleton<FishingRod>
{
    public GameObject hook;
    public GameObject throwingPoint;
    public MeshRenderer rope;

    //FSM for rodstate
    private RodState m_rodState;
    public RodState CurrentRodState { get { return m_rodState; } }

    private Vector3 oldpos;
    
    // Start is called before the first frame update
    void Start()
    {
        WaitToCast();
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
    // Update is called once per frame
    void Update()
    {
        //Manage Inputs
        bool holdingReel = Input.GetButton("XRI_Right_GripButton");

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
                if (holdingReel)
                {
                    WaitToCast();
                }
                break;
        }
    }
}
