using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ReelLogic : MonoBehaviour {
    public Transform reelHinge;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Spin();
    }

    private void Spin() {
        float rotateBy = Vector2.Angle(transform.position, reelHinge.position);

    }
}
