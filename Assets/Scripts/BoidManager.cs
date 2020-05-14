using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoidManager : Singleton<BoidManager> {
    public GameObject boidPfb;
    public int spawnCount = 50;

    // Start is called before the first frame update
    void Start() {
        for(int count = 0; count < spawnCount; count++) {
            NavMeshHit hit;
            if(NavMesh.SamplePosition(Random.insideUnitSphere * 100 + transform.position, out hit, 100, 1))
                Spawn(hit.position);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private GameObject Spawn(Vector3 _position) {
        _position.y = 0;
        return Instantiate(boidPfb, _position, Quaternion.Euler(0, Random.Range(0, 360), 0));
    }
}
