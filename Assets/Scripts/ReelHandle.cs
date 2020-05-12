using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelHandle : MonoBehaviour {
    public SnapZone snapZone;

    private void Start() {
        snapZone.eOnTriggerStay += CheckSpin;
    }


    private void CheckSpin(Collider _other) {
        if (_other.tag.Equals("LeftHand")) {
            if (Input.GetButton("XRI_Left_GripButton")) {
                Vector2 meToOther = _other.transform.position - transform.position;
                Debug.DrawRay(transform.position, new Vector3(meToOther.x, meToOther.y, 0), Color.red);
            }
        }
    }
}
