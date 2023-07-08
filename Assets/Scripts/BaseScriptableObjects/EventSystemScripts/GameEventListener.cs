using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // Attach this script to an object that wants to listen out for something

    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }

    public void TestFunction()
    {
        Debug.Log("Test to see if your event is raised");
    }
}
