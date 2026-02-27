using UnityEngine;

public class HealthBarCrumps : MonoBehaviour
{
    public CrumpsLogic target;

    public Renderer mainRenderer;
    public Renderer damageRenderer;

    public float maxHealth = 100f;

    public float smoothSpeed = 8f;
    public float damageLagSpeed = 2f;

    public string fillProperty = "_Fill";

    float mainFill = 1f;
    float damageFill = 1f;

    void Update()
    {
        if (!target) return;

        float targetFill = Mathf.Clamp01(target.health / maxHealth);

        mainFill = Mathf.Lerp(mainFill, targetFill, Time.deltaTime * smoothSpeed);

        if (damageFill > mainFill)
            damageFill = Mathf.Lerp(damageFill, mainFill, Time.deltaTime * damageLagSpeed);
        else
            damageFill = mainFill;

        if (mainRenderer)
            mainRenderer.material.SetFloat(fillProperty, mainFill);

        if (damageRenderer)
            damageRenderer.material.SetFloat(fillProperty, damageFill);
    }

    void LateUpdate()
    {
        if (Camera.main)
            transform.forward = Camera.main.transform.forward;
    }
}