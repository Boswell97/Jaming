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
            ContactPoint contacto = collision.GetContact(0);
            Vector3 punto = contacto.point + contacto.normal * offsetSalida;

            ActivarParticulas(punto, Quaternion.LookRotation(contacto.normal));
            Destroy(collision.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(capaDestructor))
        {
            Vector3 punto = other.ClosestPoint(transform.position);
            ActivarParticulas(punto, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }

    void ActivarParticulas(Vector3 posicion, Quaternion rotacion)
    {
        if (particulasPrefab != null)
            Instantiate(particulasPrefab, posicion, rotacion);
    }
}