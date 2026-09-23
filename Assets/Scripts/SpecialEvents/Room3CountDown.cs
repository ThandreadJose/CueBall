using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
public class Room3CountDown : SpecialEvents
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string[] newScript = new string[1];
    private int currentScore = 0;

    public string[] NewScript(string script)
    {
        

        newScript[0] = script;







        return newScript;
    }
    public override void eventAction()
    {
        if (!occured)
        {
            //Gotta write the event on an actual special Event
            if (room.checkPoints() == 1 && currentScore != 1)
            {
                TextController tc = gameObject.AddComponent<TextController>();

                tc.setMessages(NewScript("2 more to go!"));

                room.initializeSpeech(tc);
                currentScore = 1;


            }
            else if (room.checkPoints() == 2 && currentScore != 2)
            {
                TextController tc = gameObject.AddComponent<TextController>();

                tc.setMessages(NewScript("1 left! You're so close!"));

                room.initializeSpeech(tc);
                currentScore = 2;
            }
            else if (room.checkPoints() == 3 && currentScore != 3)
            {
                TextController tc = gameObject.AddComponent<TextController>();

                tc.setMessages(NewScript("You did it! I can't believe it! We're finally leaving!"));

                room.initializeSpeech(tc);

                currentScore = 3;
                occured = true;

            }
        }
    }


}
