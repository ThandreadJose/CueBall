using UnityEngine;

public class TutorialDoneEvent : SpecialEvents
{
    [SerializeField] private CutsceneController cutscene;
    private bool started = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void eventAction()
    {
        if (!occured)
        {
            //Gotta write the event on an actual special Event
            if (room.isTalking())
            {
                started = true;
            }
            else if (room.isTalking() == false)
            {
                if (started)
                {
                    room.initializeCutscene(cutscene);

                    occured = true;
                }
            }
            
        }
    }
        
    }
