using System.Runtime.CompilerServices;
using UnityEngine;

public class SpecialEvents : MonoBehaviour
{
    int id;
    protected RoomController room;
    protected string eventType;
    protected bool occured = false;



    public virtual void initializeEvent(int newId, RoomController Owner)
    {
        id = newId;
        room = Owner;
    }

    public virtual string getEventType()
    {
        return eventType;
    }
    public virtual void eventAction()
    {
        //Gotta write the event on an actual special Event
    }

}
