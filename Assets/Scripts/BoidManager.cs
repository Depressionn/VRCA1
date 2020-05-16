using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class BoidManager : Singleton<BoidManager> {
    public GameObject boidPfb;
    public int spawnCount = 50;

    private List<GameObject> m_boids = new List<GameObject>();
    // Start is called before the first frame update
    void Start() {
        for(int count = 0; count < spawnCount; count++) {
            m_boids.Add(Spawn(Random.insideUnitSphere * 10));
        }
    }

    // Update is called once per frame
    void Update() {
        m_boids.ForEach(x => {
            if(x.transform.position.x > 10 || x.transform.position.x < -10 || x.transform.position.z > 10 || x.transform.position.z < -10) {
                x.transform.position -= x.transform.forward * 19;
            }
        });
    }

    private GameObject Spawn(Vector3 _position) {
        _position.y = -0.4f;
        return Instantiate(boidPfb, _position, Quaternion.Euler(0, Random.Range(0, 360), 0));
    }
}
