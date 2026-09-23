using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Camera playerCam;
    [SerializeField] Camera[] CutsceneCam;
    [SerializeField] string cutsceneName;
    private int cameraIndex = 0;
    private GameObject player;

    public void OpenCamera()
    {
        playerCam = Camera.main;
         player = GameObject.FindWithTag("Player");
        if (player.transform.parent != null)
        {
            if (player.transform.parent.CompareTag("Player"))
            {
                player = player.transform.parent.gameObject;
            }
        }
        for (int i = 0; i < CutsceneCam.Length; i++)
        {
            CutsceneCam[i].enabled = false;
        }
        cameraIndex = 0;
        player.SetActive(false);
        playerCam.enabled = false;
        CutsceneCam[0].enabled = true;
    }

    public void NextCamera()
    {
        CutsceneCam[cameraIndex].enabled = false;
        cameraIndex++;
        CutsceneCam[cameraIndex].enabled = true;
    }

    public void CloseCam()
    {
        playerCam.enabled = true;
        CutsceneCam[cameraIndex].enabled = false;
        player.SetActive(true);

    }

    public string getCutsceneName()
    {
        return cutsceneName;
    }

    public Camera[] getAllCameras()
    {
        return CutsceneCam;
    }

    public Camera getPlayerCamera()
    {
        return playerCam;
    }

    public void setupCutscene(string cname, Camera[] ccameras, Camera pcam)
    {
        cutsceneName = cname;
        CutsceneCam = ccameras;
        playerCam = pcam;

    }
}
