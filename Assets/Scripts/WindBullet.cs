
using UnityEngine;

public class WindBullet : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject wind;
    public GameObject owner;

    private Rigidbody rb;

    public float charge;

    private float speed = 5f;
    private float lifeTime = 1;
    private bool isTouched = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        speed = speed * charge * 2;
        rb = GetComponent<Rigidbody>();
        if (!isTouched)
        {
            rb.linearVelocity = transform.forward * speed;
        }else {
            rb.linearVelocity = transform.forward;
        }

            Destroy(bullet, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!isTouched)
            {
                wind.SetActive(false);
                isTouched = true;
            }
        }
        if (other.gameObject.CompareTag("Bomb"))
        {

            MouseController player = owner.GetComponent<MouseController>();
            player.markBomb(other.gameObject);
            BombController bomb = other.GetComponentInParent<BombController>();
            bomb.markBomb();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouched)
        {
            wind.transform.Rotate(Vector3.forward * 1440 * Time.deltaTime);
            //wind.transform.forward = bullet.transform.forward;
        }
        
    }
}
