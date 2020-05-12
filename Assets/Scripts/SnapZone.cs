using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Triggers events when item enters/leaves collider
/// </summary>
public class SnapZone : MonoBehaviour {
    public event Action<Collider> eOnTriggerEnter;
    public event Action<Collider> eOnTriggerExit;
    public event Action<Collider> eOnTriggerStay;

    private void OnTriggerEnter(Collider other) {
        eOnTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other) {
        eOnTriggerExit?.Invoke(other);
    }

    private void OnTriggerStay(Collider other) {
        eOnTriggerStay?.Invoke(other);
    }
}
