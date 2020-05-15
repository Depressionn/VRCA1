using UnityEngine;
using UnityEngine.AI;

public class BoidController : MonoBehaviour {
    public float sightRange = 1.0f;
    public float moveSpeed = 12;
    public float rotationSpeed = 100;

    // Start is called before the first frame update
    void Start() {
        GetComponentInChildren<SpriteRenderer>().color = new Color(80, 80, Random.Range(150, 255));
    }

    // Update is called once per frame
    void Update() {
        BoidBehaviour();

        transform.position = new Vector3(transform.position.x, -0.4f, transform.position.z);
    }

    private void BoidBehaviour() {
        Vector3 seperation = Vector3.zero;
        Vector3 alignment = transform.forward;
        Vector3 cohesion = transform.position;

        //find nearby boids
        Collider[] nearby = Physics.OverlapSphere(transform.position, sightRange, 1 << 8);

        foreach(Collider boid in nearby) {
            if (boid.gameObject == gameObject) continue;    //me
            Transform t = boid.transform;
            seperation += GetSeperationVector(t);
            alignment += t.forward;
            cohesion += t.position;
        }

        float avg = 1.0f / nearby.Length;
        alignment *= avg;
        cohesion *= avg;
        cohesion = (cohesion - transform.position).normalized;

        Vector3 direction = seperation + alignment + cohesion;
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, direction.normalized);

        if(rot != transform.rotation) {
            float interpolationValue = Mathf.Exp(-rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(rot, transform.rotation, interpolationValue);
        }

        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //NavMeshHit hit;
        //if(!NavMesh.SamplePosition(transform.position + transform.forward, out hit, sightRange, 1)) {
        //    float interpolationValue = Mathf.Exp(-rotationSpeed * Time.deltaTime);
        //    transform.rotation = Quaternion.Slerp(Quaternion.FromToRotation(Vector3.forward, transform.right), transform.rotation, interpolationValue);
        //}
    }

    private Vector3 GetSeperationVector(Transform _target) {
        Vector3 difference = transform.position - _target.transform.position;
        float scaler = Mathf.Clamp01(1.0f - difference.magnitude / sightRange);
        return difference * (scaler / difference.magnitude);
    }
}
