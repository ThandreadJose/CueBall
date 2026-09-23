
using UnityEngine;

public class ObjectFacing : MonoBehaviour
{
    [SerializeField] private GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
            if (target != null)
            {
                if (target.transform.parent.gameObject.CompareTag("Player"))
                {
                    target = target.transform.parent.gameObject;
                }
            }
        }
        if (target != null)
        {
            if (target.activeSelf == true)
            {
                transform.LookAt(target.transform.position);
            }
            else if (target.activeSelf == false)
            {
                foreach (Camera cam in Camera.allCameras)
                {
                    if (cam.enabled == true)
                    {
                        transform.LookAt(cam.transform.position);
                        Debug.Log("called");
                    }
                }
            }
        }
    }
}
