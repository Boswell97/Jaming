using UnityEngine;
using System.Collections;

public class PuertaLevel1 : MonoBehaviour
{
    public SingleTrigger triggerColor;
    public SingleTrigger triggerSitio;

    public float delayDestruccion = 0.5f;
    public float duracionFade = 1.5f;

    public GameObject efectoMagiaPrefab;

    private bool puertaAbierta = false;
    private Renderer[] renderers;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        if (triggerColor == null)
            triggerColor = GameObject.Find("TriggerColor")?.GetComponent<SingleTrigger>();

        if (triggerSitio == null)
            triggerSitio = GameObject.Find("TriggerSitio")?.GetComponent<SingleTrigger>();
    }

    void Update()
    {
        if (!puertaAbierta && AmbosTriggersActivos())
            AbrirPuerta();
    }

    bool AmbosTriggersActivos()
    {
        bool colorActivo = triggerColor != null && triggerColor.IsActive();
        bool sitioActivo = triggerSitio != null && triggerSitio.IsActive();
        return colorActivo && sitioActivo;
    }

    void AbrirPuerta()
    {
        puertaAbierta = true;
        Invoke(nameof(IniciarDesaparicion), delayDestruccion);
    }

    void IniciarDesaparicion()
    {
        if (efectoMagiaPrefab != null)
            Instantiate(efectoMagiaPrefab, transform.position, Quaternion.identity);

        StartCoroutine(FadePuerta());
    }

    IEnumerator FadePuerta()
    {
        efectoMagiaPrefab.gameObject.SetActive(true);

        float tiempo = 0f;
     

        while (tiempo < duracionFade)
        {
            float alpha = Mathf.Lerp(1f, 0f, tiempo / duracionFade);

            foreach (var r in renderers)
            {
                foreach (var mat in r.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color c = mat.color;
                        c.a = alpha;
                        mat.color = c;
                    }
                }
            }

            tiempo += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        efectoMagiaPrefab.gameObject.SetActive(false);
        gameObject.SetActive(false);
 

    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 30), $"TriggerColor activo: {(triggerColor != null ? triggerColor.IsActive().ToString() : "null")}");
        GUI.Label(new Rect(10, 40, 300, 30), $"TriggerSitio activo: {(triggerSitio != null ? triggerSitio.IsActive().ToString() : "null")}");
        GUI.Label(new Rect(10, 70, 300, 30), $"Puerta Abierta: {puertaAbierta}");

        if (AmbosTriggersActivos() && !puertaAbierta)
            GUI.Label(new Rect(10, 100, 300, 30), "¡CONDICIÓN CUMPLIDA! Puerta se abrirá...");
    }
}