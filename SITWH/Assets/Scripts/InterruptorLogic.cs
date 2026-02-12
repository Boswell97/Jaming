using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InterruptorZonas : MonoBehaviour
{
    [System.Serializable]
    public class Zona
    {
        public GameObject[] objetosAActivar;
        public GameObject[] objetosADesactivar;
    }

    [Header("Zonas (1 a 4)")]
    public Zona zona1;
    public Zona zona2;
    public Zona zona3;
    public Zona zona4;

    [Header("Métodos de Activación")]
    public bool activarConClick = true;
    public bool activarConTrigger = true;
    public InputActionReference inputAction;

    [Header("Evento")]
    public UnityEvent alCambiarZona;

    private int zonaActual = 0; 
    private int zonaAnterior = 0;

    void OnEnable()
    {
        if (inputAction != null)
            inputAction.action.performed += OnInputAction;
    }

    void OnDisable()
    {
        if (inputAction != null)
            inputAction.action.performed -= OnInputAction;
    }

    private void OnInputAction(InputAction.CallbackContext context)
    {
        Interact();
    }

    
    public void Interact()
    {
        SiguienteZona();
    }

    public void SiguienteZona()
    {
        int nuevaZona = (zonaActual % 4) + 1;
        CambiarAZona(nuevaZona);
    }

    public void CambiarAZona(int numZona)
    {
        if (numZona < 1 || numZona > 4) return;

        zonaAnterior = zonaActual;
        zonaActual = numZona;

        if (zonaAnterior >= 1 && zonaAnterior <= 4)
        {
            Zona ant = ObtenerZona(zonaAnterior);
            AplicarEstado(ant.objetosAActivar, false);   
            AplicarEstado(ant.objetosADesactivar, true); 
        }

        Zona nueva = ObtenerZona(zonaActual);
        AplicarEstado(nueva.objetosAActivar, true);  
        AplicarEstado(nueva.objetosADesactivar, false); 

        alCambiarZona?.Invoke();
    }

    private void AplicarEstado(GameObject[] objetos, bool estado)
    {
        if (objetos == null) return;
        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(estado);
        }
    }

    private Zona ObtenerZona(int numZona)
    {
        switch (numZona)
        {
            case 1: return zona1;
            case 2: return zona2;
            case 3: return zona3;
            case 4: return zona4;
            default: return null;
        }
    }

    // --- Activación por clic ---
    void OnMouseDown()
    {
        if (activarConClick)
            Interact();
    }

    void OnTriggerEnter(Collider other)
    {
        if (activarConTrigger && other.CompareTag("Player"))
            Interact();
    }

  
}