using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;
public class TooQuickEvent : SpecialEvents
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string[] newScript = new string[3];

    public string[] NewScript()
    {
        string message1 = "Wow... OK, I guess you learn faster than I can speak...";
        string message2 = "I WAS going to give you all kinds of neat tips and tricks";
        string message3 = "But I guess YOU know better...";

        newScript[0] = message1;
        newScript[1] = message2;
        newScript[2] = message3;






        return newScript;
    }
    public override void eventAction()
    {
        if (!occured)
        {
            //Gotta write the event on an actual special Event
            if (room.isTalking())
            {
                if (room.checkPoints() == 1)
                {
                    TextController tc = gameObject.AddComponent<TextController>();

                    tc.setMessages(NewScript());

                    room.initializeSpeech(tc);

                    occured = true;
                }
            }
        } 
    }

    
}
