using UnityEngine;
using UnityEngine.UI; // Necesario para Image
using System.Collections;

public class FadeInicio : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Panel de UI (debe tener componente Image)")]
    public Image panelImage; // Cambiado de CanvasGroup a Image

    [Tooltip("GameObject del jugador (debe contener el script PlayerLogic)")]
    public GameObject jugador;

    [Tooltip("Primer objeto a activar después del fade")]
    public GameObject objeto1;

    [Tooltip("Segundo objeto a activar después del fade")]
    public GameObject objeto2;

    [Header("Configuración")]
    [Tooltip("Duración del fade en segundos")]
    public float duracionFade = 2f;

    void Start()
    {
        StartCoroutine(FadeOutYActivar());
    }

    IEnumerator FadeOutYActivar()
    {
        // Verificar que el panel tenga Image asignado
        if (panelImage == null)
        {
            Debug.LogError("FadeInicio: No se asignó la imagen del panel.");
            yield break;
        }

        // Guardar el color original
        Color colorOriginal = panelImage.color;
        // Asegurar alpha inicial 1
        panelImage.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 1f);

        // Fade out progresivo
        float tiempo = 0f;
        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float nuevoAlpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            panelImage.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, nuevoAlpha);
            yield return null;
        }

        // Alpha final 0 por seguridad
        panelImage.color = new Color(colorOriginal.r, colorOriginal.g, colorOriginal.b, 0f);

        // (Opcional) Desactivar el panel por completo
        // panelImage.gameObject.SetActive(false);

        // Activar componente PlayerLogic en el jugador
        if (jugador != null)
        {
            PlayerLogic playerLogic = jugador.GetComponent<PlayerLogic>();
            if (playerLogic != null)
            {
                playerLogic.enabled = true;
                Debug.Log("PlayerLogic activado correctamente.");
            }
            else
            {
                Debug.LogWarning("No se encontró el componente PlayerLogic en el jugador.");
            }
        }
        else
        {
            Debug.LogWarning("No se asignó el GameObject del jugador.");
        }

        // Activar objetos adicionales
        if (objeto1 != null)
        {
            objeto1.SetActive(true);
            Debug.Log("Objeto1 activado.");
        }

        if (objeto2 != null)
        {
            objeto2.SetActive(true);
            Debug.Log("Objeto2 activado.");
        }
    }
}