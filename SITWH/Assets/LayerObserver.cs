using UnityEngine;

public class LayerObserver : MonoBehaviour
{
    [Tooltip("Nombre de la capa de los objetos que serán destruidos al ser tocados por este objeto.")]
    public string capaDestructor = "Destructors";

    // Se llama al iniciar una colisión (necesita Collider + Rigidbody)
    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el objeto con el que colisionamos pertenece a la capa "Destructors"
        if (collision.gameObject.layer == LayerMask.NameToLayer(capaDestructor))
        {
            // Destruir el objeto destructor
            Destroy(collision.gameObject);
        }
    }

    // Alternativa si usas Triggers (Collider con "Is Trigger" activado)
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(capaDestructor))
        {
            Destroy(other.gameObject);
        }
    }
}