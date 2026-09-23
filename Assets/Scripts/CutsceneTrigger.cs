using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
           

            if (gameObject.CompareTag("TextBox"))
            {
                Debug.Log("haha");
                TextController tc = GetComponent<TextController>();
                if (tc != null)
                {
                    RoomController rc = GetComponentInParent<RoomController>();
                    if (rc != null)
                    {
                        rc.initializeSpeech(tc);
                    }
                }

            }
            else if (gameObject.CompareTag("Cutscene"))
            {
                CutsceneController cc = GetComponent<CutsceneController>();
                if (cc != null)
                {
                    RoomController rc = GetComponentInParent<RoomController>();
                    if (rc != null)
                    {
                        rc.initializeCutscene(cc);
                    }
                }
            }

            GameObject.Destroy(gameObject);

           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
