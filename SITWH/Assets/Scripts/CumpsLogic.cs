using UnityEngine;
using System.Collections;

public class CrumpsLogic : MonoBehaviour
{
    public Animator animator;
    public Collider dodgeTrigger;
    public LayerMask grabbableLayer;

    public float health = 100f;
    public float healthChangeAmount = 10f;

    bool claps;
    bool ups;
    bool dodges;
    bool no;

    Coroutine reactionRoutine;
    Coroutine dodgeRoutine;

    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
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

        if (reactionRoutine != null)
            StopCoroutine(reactionRoutine);

        reactionRoutine = StartCoroutine(GoodReaction());
    }

    public void OnBadObjectDestroyed(Vector3 position)
    {
        health -= healthChangeAmount;

        if (reactionRoutine != null)
            StopCoroutine(reactionRoutine);

        reactionRoutine = StartCoroutine(BadReaction());
    }

    IEnumerator GoodReaction()
    {
        ResetReactions();
        claps = true;
        ups = true;
        yield return new WaitForSeconds(1f);
        ResetReactions();
    }

    IEnumerator BadReaction()
    {
        ResetReactions();
        no = true;
        yield return new WaitForSeconds(1f);
        ResetReactions();
    }

    IEnumerator DodgeReaction()
    {
        dodges = true;
        yield return new WaitForSeconds(0.7f);
        dodges = false;
    }

    void ResetReactions()
    {
        claps = false;
        ups = false;
        no = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsInLayerMask(other.gameObject, grabbableLayer))
        {
            if (dodgeRoutine != null)
                StopCoroutine(dodgeRoutine);

            dodgeRoutine = StartCoroutine(DodgeReaction());
        }
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }
}