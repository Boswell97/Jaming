using UnityEngine;

public class SingleTrigger : MonoBehaviour
{
    public string requiredTag;
    public string targetLayer = "Grabbable";

    [SerializeField] private bool isActive = false;
    private int layerValue;

    void Start()
    {
        layerValue = LayerMask.NameToLayer(targetLayer);
        GetComponent<Collider>().isTrigger = true;
        Debug.Log($"✅ Trigger '{gameObject.name}' listo para tag: {requiredTag}");
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == layerValue)
        {
            if (other.CompareTag(requiredTag))
            {
                isActive = true;
             
                Destroy(other.gameObject);
            }
            else
            {
              
                Destroy(other.gameObject);
            }
        }
    }

    public bool IsActive() => isActive;
    public void ResetTrigger() => isActive = false;
}