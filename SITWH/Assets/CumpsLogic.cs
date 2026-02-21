using UnityEngine;
using UnityEngine.AI;

public class CrumpsLogic : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public NavMeshAgent agent;
    public Collider dodgeTrigger;
    public LayerMask grabbableLayer;

    [Header("Salud")]
    public float health = 100f;
    public float healthChangeAmount = 10f;

    [Header("Estados de animación (bools)")]
    private bool claps;
    private bool ups;
    private bool dodges;
    private bool no;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        animator.SetBool("Claps", claps);
        animator.SetBool("Ups", ups);
        animator.SetBool("Dodges", dodges);
        animator.SetBool("No", no);
    }

    public void OnGoodObjectDestroyed(Vector3 position)
    {
        health += healthChangeAmount;
        claps = true;
        ups = true;
        no = false;
    }

    public void OnBadObjectDestroyed(Vector3 position)
    {
        health -= healthChangeAmount;
        no = true;
        claps = false;
        ups = false;
        if (agent != null)
        {
            agent.SetDestination(position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, grabbableLayer))
        {
            dodges = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject, grabbableLayer))
        {
            dodges = false;
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}