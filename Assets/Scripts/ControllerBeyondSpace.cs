using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ControllerBeyondSpace : MonoBehaviour
{
    public EventBeyondSpace TriggerEnterHasHappened;

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterHasHappened?.Invoke(other);
    }
}

public delegate void EventBeyondSpace(Collider collider);