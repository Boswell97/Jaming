using UnityEngine;

public class CalavereFlotando : MonoBehaviour
{
    [Header("Movimiento hacia el jugador")]
    [Tooltip("Velocidad a la que flota hacia el jugador")]
    public float velocidad = 2f;

    [Header("Flotación vertical (estilo fantasma)")]
    [Tooltip("Altura máxima de la oscilación")]
    public float amplitudFlotacion = 0.5f;
    [Tooltip("Velocidad de la oscilación (ciclos por segundo)")]
    public float frecuenciaFlotacion = 2f;

    [Header("Rotación rápida (spinning)")]
    [Tooltip("Velocidad de rotación en grados por segundo en cada eje")]
    public Vector3 velocidadRotacion = new Vector3(180f, 180f, 180f); // Valores altos para giro rápido

    private Transform jugador;
    private float yInicial;

    void Start()
    {
        // Busca al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            jugador = player.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con la etiqueta 'Player'.");
        }

        yInicial = transform.position.y;
    }

    void Update()
    {
        if (jugador == null) return;

        Vector3 posicionActual = transform.position;
        Vector3 direccionHorizontal = jugador.position - posicionActual;
        direccionHorizontal.y = 0; // Ignoramos la diferencia en Y para mantener la altura base

        if (direccionHorizontal.magnitude > 0.01f) // Evitar división por cero
        {
            direccionHorizontal.Normalize();
            Vector3 desplazamiento = direccionHorizontal * velocidad * Time.deltaTime;
            transform.Translate(desplazamiento, Space.World);
        }

        // 2. Flotación vertical (onda senoidal alrededor de la Y inicial)
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.y = yInicial + Mathf.Sin(Time.time * frecuenciaFlotacion) * amplitudFlotacion;
        transform.position = nuevaPosicion;

        // 3. Rotación rápida en todos los ejes (spinning)
        transform.Rotate(velocidadRotacion * Time.deltaTime);
    }

    // Se llama cuando otro collider entra en el trigger de este objeto
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("tocado");

    
    }
}