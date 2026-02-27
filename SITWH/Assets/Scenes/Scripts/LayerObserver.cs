using UnityEngine;

public class LayerObserver : MonoBehaviour
{
    public string capaDestructor = "Destructor";
    public GameObject particulasPrefab;
    public float offsetSalida = 0.02f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(capaDestructor))
        {
            Vector3 spawnPoint = GetSurfacePoint(collision.contacts[0].point, collision.collider);
            Quaternion spawnRotation = Quaternion.LookRotation(collision.contacts[0].normal);
            ActivarParticulas(spawnPoint, spawnRotation);
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(capaDestructor))
        {
            Vector3 spawnPoint = GetSurfacePoint(transform.position, other);
            ActivarParticulas(spawnPoint, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    Vector3 GetSurfacePoint(Vector3 referencePoint, Collider targetCollider)
    {
        Vector3 closest = targetCollider.ClosestPoint(referencePoint);

        Vector3 direction = (closest - targetCollider.bounds.center).normalized;
        if (direction == Vector3.zero) direction = Vector3.up; // fallback
        return closest + direction * offsetSalida;
    }

    void ActivarParticulas(Vector3 posicion, Quaternion rotacion)
    {
        if (particulasPrefab != null)
            Instantiate(particulasPrefab, posicion, rotacion);
    }
}