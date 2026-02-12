using UnityEngine;

public class LuzRegulator : MonoBehaviour
{
    [Header("Rangos de intensidad")]
    public float intensidadMin = 0.5f;
    public float intensidadMax = 2.0f;

    [Header("Frecuencia de cambio")]
    public float intervaloMin = 0.2f;
    public float intervaloMax = 1.0f;

    private Light[] luces;
    private float temporizador;
    private float intervaloActual;

    void Start()
    {
        // Obtiene todos los componentes Light en este GameObject y sus hijos
        luces = GetComponentsInChildren<Light>();

     
        // Inicia con un intervalo aleatorio
        intervaloActual = Random.Range(intervaloMin, intervaloMax);
        temporizador = 0f;
    }

    void Update()
    {
        if (luces.Length == 0) return;

        temporizador += Time.deltaTime;

        if (temporizador >= intervaloActual)
        {
            CambiarIntensidades();
            temporizador = 0f;
            intervaloActual = Random.Range(intervaloMin, intervaloMax);
        }
    }

    void CambiarIntensidades()
    {
        foreach (Light luz in luces)
        {
            if (luz != null)
            {
                luz.intensity = Random.Range(intensidadMin, intensidadMax);
            }
        }
    }
}