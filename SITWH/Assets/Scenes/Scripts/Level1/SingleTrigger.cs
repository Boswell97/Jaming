using UnityEngine;

public class SingleTrigger : MonoBehaviour
{
    public string requiredTag;
    public string targetLayer = "Grabbable";

    [SerializeField] private bool isActive = false;
    private int layerValue;
    private Collider myCollider;
    [SerializeField] public GameObject myCollider1;
    [SerializeField] public GameObject myCollider2;
    void Start()
    {
        layerValue = LayerMask.NameToLayer(targetLayer);
        myCollider = GetComponent<Collider>();

        myCollider.isTrigger = true;
        isActive = false; // Asegurar que empiece en false

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerValue)
        {
            Debug.Log($"{gameObject.name}: Ignorado {other.name} - Layer incorrecta");
            return;
        }

        Debug.Log($"🎯 {gameObject.name} activado por: {other.name} (Tag: {other.tag})");

        if (other.CompareTag(requiredTag))
        {
            isActive = true;
            Debug.Log($"✅ {gameObject.name}: TAG CORRECTO '{requiredTag}'!");
            Destroy(other.gameObject);
            
            myCollider1.SetActive(false);
            myCollider2.SetActive(true);

        }
        else
        {
            Debug.Log($"❌ {gameObject.name}: Tag incorrecto. Esperaba '{requiredTag}', tiene '{other.tag}'");

            Rigidbody rb = other.attachedRigidbody;

            if (rb != null)
            {
                float multiplicador = 6f;
                rb.linearVelocity *= multiplicador;
            }
        }
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void ResetTrigger()
    {
        isActive = false;
    }

    void OnDrawGizmos()
    {
        if (myCollider != null && myCollider.enabled)
        {
            Gizmos.color = isActive ? Color.green : Color.red;
            Gizmos.DrawWireCube(myCollider.bounds.center, myCollider.bounds.size);
        }
    }
}