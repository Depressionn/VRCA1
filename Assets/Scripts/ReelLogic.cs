using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR;

public class ReelLogic : MonoBehaviour {
    public SnapZone reelSnapZone;
    public GameObject reelModel;

    [HideInInspector]
    public float spinPerSec;
    private float[] m_prevSpins = new float[5];
    private int m_count = 0;
    private float m_degChange = 0;

    public event Action<float> eOnSpinReel;
    private bool m_shuttingDown = false;

    // Start is called before the first frame update
    void Start() {
        reelSnapZone.eOnTriggerStay += CheckSpin;

        StartCoroutine(SetSpm());
    }

    // Update is called once per frame
    void Update() {
        CheckLength();
        float avg = 0;
        foreach (float f in m_prevSpins) avg += f;
        UIManager.Instance.lmao.text = $"Spm: {avg / 5}";
    }

    private void OnApplicationQuit() {
        m_shuttingDown = true;
    }

    private IEnumerator SetSpm() {
        while (!m_shuttingDown) {
            m_prevSpins[m_count] = m_degChange;
            m_count = ++m_count % 5;
            m_degChange = 0;
            yield return new WaitForSeconds(1);
        }
    }

    private void CheckLength() {

    }

    private void CheckSpin(Collider _other) {
        if (_other.tag.Equals("LeftHand")) {
            if (Input.GetButton("XRI_Left_GripButton")) {
                float currentZ = reelModel.transform.localRotation.eulerAngles.z;
                reelModel.transform.LookAt(_other.transform.position, reelModel.transform.forward);
                float newZ = reelModel.transform.localRotation.eulerAngles.z;
                m_degChange += newZ;
                reelModel.transform.localRotation = Quaternion.Euler(0, 0, newZ);

                float avg = 0;
                foreach (float f in m_prevSpins) avg += f;
                eOnSpinReel?.Invoke(avg / 5);
            }
        }
    }
}
