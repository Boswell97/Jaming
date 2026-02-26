using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private SingleTrigger[] childTriggers;
    [SerializeField] private bool allActive = false;




    void Start()
    {
        FindAllTriggers();
    }

    void FindAllTriggers()
    {
        childTriggers = GetComponentsInChildren<SingleTrigger>(true);

        if (childTriggers.Length == 0)
        {
        }
    }

    public bool AreAllTriggersActive()
    {
        if (childTriggers == null || childTriggers.Length == 0)
        {
            FindAllTriggers();
        }

        allActive = true;

        foreach (var trigger in childTriggers)
        {
            if (trigger == null) continue;

            if (!trigger.IsActive())
            {
                allActive = false;
                break;
            }
        }

        return allActive;
    }

    public void ResetAllTriggers()
    {
        foreach (var trigger in childTriggers)
        {
            if (trigger != null)
            {
                trigger.ResetTrigger();
            }
        }
        allActive = false;
    }

    void Update()
    {
      
    }
}