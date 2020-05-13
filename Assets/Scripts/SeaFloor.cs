using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaFloor : MonoBehaviour
{
    public FishingRod fishingRod;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == fishingRod.hook)
        {
            fishingRod.LandedInWater();
            Debug.Log("LandedInWater");
            UIManager.Instance.SetCastingDistance(Vector3.Distance(fishingRod.hook.transform.position, new Vector3(fishingRod.transform.position.x, fishingRod.hook.transform.position.y, fishingRod.transform.position.z))/2);
            UIManager.Instance.SetHookDistance(Vector3.Distance(fishingRod.hook.transform.position, new Vector3(fishingRod.transform.position.x, fishingRod.hook.transform.position.y, fishingRod.transform.position.z))/2);
        }
    }
}
