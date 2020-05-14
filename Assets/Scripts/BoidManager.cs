using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : Singleton<BoidManager> {
    public GameObject boidPfb;
    public int spawnCount = 50;
    public float spawnRadius = 20;

    // Start is called before the first frame update
    void Start() {
        for(int count = 0; count < spawnCount; count++) {
            Spawn(transform.position + Random.insideUnitSphere * spawnRadius);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private GameObject Spawn(Vector3 _position) {
        _position.y = 0;
        return Instantiate(boidPfb, _position, Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
