using UnityEngine;

public class TextController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string[] messages;

    public string[] getMessages()
    {
        return messages;
    }

    public void setMessages(string[] speech)
    {
        messages = speech;
    }
}
