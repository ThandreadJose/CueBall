using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInstructions : SpecialEvents   
{
    [SerializeField] private Texture[] instructions;
    [SerializeField] private RawImage rawImage;
    private Animation move;
    private bool started = false;
    private int currentIndex;
    private Coroutine activeCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rawImage = GameObject.FindWithTag("Instructions").GetComponent<RawImage>();
        move = rawImage.GetComponent<Animation>();

    }

    private IEnumerator WaitForDown(int index)
    {

        Debug.Log("Called");
        AnimationClip anim = move.GetClip("InstructionsDown");
        move.Play("InstructionsDown");

        yield return null;
        float animLength = anim.length;
        yield return new WaitForSeconds(animLength);
        newInstructions(index);


    }

    public void newInstructions(int indexInstructions)
    {
        rawImage.texture = instructions[indexInstructions];


        move.Play("InstructionsUp");
    }

    public override void eventAction()
    {
        if (!occured)
        {
            //Gotta write the event on an actual special Event
            if (room.isTalking())
            {
                started = true;
            }



            if (started == true)
            {
                if (room.getIndex() == 4 && currentIndex != room.getIndex())
                {
                    newInstructions(0);

                    currentIndex = 4;
                }
                else if (room.getIndex() == 6 && currentIndex != room.getIndex())
                {


                    activeCoroutine = StartCoroutine(WaitForDown(1));
                    currentIndex = 6;

                }
                else if (room.getIndex() == 8 && currentIndex != room.getIndex())
                {

                    activeCoroutine = StartCoroutine(WaitForDown(2));
                    currentIndex = 8;

                }
                else if (room.getIndex() >= 9 && currentIndex != room.getIndex())
                {
                    StopCoroutine(activeCoroutine);
                    activeCoroutine = null;
                    move.Play("InstructionsDown");
                    occured = true;

                }
            }
        }
    }

}
