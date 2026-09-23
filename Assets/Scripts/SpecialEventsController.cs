using UnityEngine;

public class SpecialEventsController : MonoBehaviour
{
    public SpecialEvents[] specialEvents;
    [SerializeField] RoomController roomController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < specialEvents.Length; i++)
        {
            specialEvents[i].initializeEvent(i,roomController);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i < specialEvents.Length; i++)
        {
            specialEvents[i].eventAction();
        }
    }
}
