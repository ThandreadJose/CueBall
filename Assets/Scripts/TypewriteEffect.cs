using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TypewriteEffect: MonoBehaviour
{
    
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private float timeBetweenLetters = 0.03f;
    [SerializeField] Animation director;
    [SerializeField] AnimationClip animationClip;
    [SerializeField] private FaceController faceController;
    [SerializeField] bool typing = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] talkingSounds;
    private bool skipText = false;

    [SerializeField] private bool bubbleActive;

    private Coroutine typingCoroutine;

    public void talkSound()
    {
        audioSource.PlayOneShot(talkingSounds[Random.Range(0, talkingSounds.Length)], 0.8f);

    }

    public void ShowText(string fullText)
    {

        if (bubbleActive == false)
        {
            director.Play("SpeechBubble");
            bubbleActive = true;
            
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        faceController.ResetFace();
        typingCoroutine = StartCoroutine(TypeText(fullText));
    }

    public void DropBubble()
    {
        director.Play("SpeechDown");
        bubbleActive = false;
        typingCoroutine = null;
    }

    public void SkipText(bool skip)
    {
        skipText = skip;

 
    }

    public void StopText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            Debug.Log("called");
        }
        typingCoroutine = null;
    }

    public bool checkSkip()
    {
        return skipText;
    }

    public bool checkTyping()
    {
        return typing;
    }

    public bool checkActive()
    {
        return bubbleActive;
    }



    private IEnumerator TypeText(string textToType)
    {
        textComponent.text = textToType;

        textComponent.maxVisibleCharacters = 0;
        typing = true;

        for (int i = 0; i <= textToType.Length; i++)
        {
            textComponent.maxVisibleCharacters = i;
            faceController.ChangeFace();
            talkSound();
            yield return new WaitForSeconds(timeBetweenLetters);
        }
        typingCoroutine = null;
        typing = false;
        faceController.ResetFace();
    }

    public void Update()
    {

    }

}
