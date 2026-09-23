using UnityEngine;

public class PickupCollider : MonoBehaviour
{
    [SerializeField] MouseController mc;
    [SerializeField] GameObject box;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip catchSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mc = GetComponentInParent<MouseController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            GameObject bomb = other.gameObject;
            BombController bc = bomb.GetComponentInParent<BombController>();
            if (bc != null)
            {
                if (bc.checkMark() == true)
                {
                    if (bc.checkCaught() == false)
                    {
                        source.PlayOneShot(catchSound);
                    }
                    Transform firepoint = mc.getHold();
                    mc.catchBomb(other.gameObject);
                    bc.bombCaught(firepoint);



                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
