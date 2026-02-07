using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private SingleTrigger[] childTriggers;

    void Start()
    {
        childTriggers = GetComponentsInChildren<SingleTrigger>();
    }

    public bool AreAllTriggersActive()
    {
        foreach (var trigger in childTriggers)
        {
            if (!trigger.IsActive()) return false;
        }
        return true;
    }

    public void ResetAllTriggers()
    {
        foreach (var trigger in childTriggers)
        {
            trigger.ResetTrigger();
        }
    }
}