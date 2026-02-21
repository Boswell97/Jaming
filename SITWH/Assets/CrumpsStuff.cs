using UnityEngine;

public class CrumpsStuff : MonoBehaviour
{
    public CrumpsLogic crumpsLogic;

    void Start()
    {
        if (crumpsLogic == null)
        {
            GameObject crumps = GameObject.FindGameObjectWithTag("Crumps");
            if (crumps != null)
                crumpsLogic = crumps.GetComponent<CrumpsLogic>();
        }
    }

    public void NotifyCrumps()
    {
        if (crumpsLogic == null) return;

        if (CompareTag("ObjectosCrumpsGood"))
        {
            crumpsLogic.OnGoodObjectDestroyed(transform.position);
        }
        else if (CompareTag("ObjectosCrumpsBad"))
        {
            crumpsLogic.OnBadObjectDestroyed(transform.position);
        }
    }

    void OnDestroy()
    {
        NotifyCrumps();
    }
}