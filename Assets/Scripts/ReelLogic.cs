using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class ReelLogic : MonoBehaviour {
    public float ropeLength = 1.5f;
    public float lengthPerSpin = 0.5f;
    public SnapZone reelSnapZone;
    public GameObject reelModel;

    public event Action eOnCompletelyReeledIn;
    public event Action eOnCompletelyReeledOut;

    private float m_lengthOut = 0;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        CheckLength();
    }

    public void CastRope() {
        m_lengthOut = ropeLength;
        reelModel.transform.rotation = Quaternion.Euler(0, 0, (ropeLength / lengthPerSpin) * 360);
    }

    private void CheckLength() {
        //check how much is reeled in alr
        float degreeForFullReel = (ropeLength / lengthPerSpin) * 360;
        m_lengthOut = (reelModel.transform.rotation.eulerAngles.z / degreeForFullReel) * ropeLength;

        if(m_lengthOut == 0) {
            eOnCompletelyReeledIn?.Invoke();
        }else if(m_lengthOut == ropeLength) {
            eOnCompletelyReeledOut?.Invoke();
        }
    }

}
