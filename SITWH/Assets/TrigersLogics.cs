using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public Collider triggerColor;
    public Collider triggerShape;
    public string colorTag = "RightColor";
    public string shapeTag = "RightShape";
    public string targetLayer = "Grabbable";

    private bool colorTriggerActive = false;
    private bool shapeTriggerActive = false;
    private int layerValue;

    private void Start()
    {
        layerValue = LayerMask.NameToLayer(targetLayer);

        if (triggerColor == null || triggerShape == null)
        {
            Debug.LogError("Asigna ambos colliders en el inspector");
        }
    }

    public bool AreBothTriggersActive()
    {
        return colorTriggerActive && shapeTriggerActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerValue) return;

        bool isColorTrigger = other.bounds.Intersects(triggerColor.bounds);
        bool isShapeTrigger = other.bounds.Intersects(triggerShape.bounds);

        if (isColorTrigger)
        {
            if (other.CompareTag(colorTag))
            {
                colorTriggerActive = true;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        else if (isShapeTrigger)
        {
            if (other.CompareTag(shapeTag))
            {
                shapeTriggerActive = true;
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void ResetTriggers()
    {
        colorTriggerActive = false;
        shapeTriggerActive = false;
    }

    public bool GetColorTriggerState()
    {
        return colorTriggerActive;
    }

    public bool GetShapeTriggerState()
    {
        return shapeTriggerActive;
    }
}