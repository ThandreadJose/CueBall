using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections;
using UnityEngine;
public class Room2Complete : SpecialEvents
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string[] newScript = new string[1];
    [SerializeField] private Camera[] showcaseCams;
    [SerializeField] private Camera mainCam;

    public string[] NewScript()
    {
        string message1 = "It's nice not having to be afraid of blowing up... Anway, good job!";

        newScript[0] = message1;







        return newScript;
    }
    public override void eventAction()
    {
        if (!occured)
        {
            //Gotta write the event on an actual special Event
            if (room.checkPoints() >= room.checkGoal())
            {

                TextController tc = gameObject.AddComponent<TextController>();

                tc.setMessages(NewScript());

                room.initializeSpeech(tc);

                occured = true;

                CutsceneController cc = gameObject.AddComponent<CutsceneController>();

                cc.setupCutscene("Room2PuzzleDone", showcaseCams, mainCam);

                room.initializeCutscene(cc);

            }
        }
    }


}
