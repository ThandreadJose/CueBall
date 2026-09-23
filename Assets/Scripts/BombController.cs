using Unity.VisualScripting;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] Vector3 linearVel;
    [SerializeField] GameObject body;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject room;
    [SerializeField] Material originalSprite;
    [SerializeField] Material markedSprite;
    [SerializeField] GameObject ballSprite;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip explodingSound;
    private float charge;
    private Vector3 direction;
    private bool hit;
    private bool marked;
    private bool caught;
    private Transform caughtPos;
    private GameObject last;
    private bool pulling;
    //private bool touchingFloor;
    private bool exploded = false;
    private float deathTimer = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshRenderer mr = ballSprite.GetComponent<MeshRenderer>();
        mr.material = originalSprite;
    }

    public void roomOwner(GameObject owner)
    {
        room = owner;
    }

    public bool checkPulling()
    {
        return pulling;
    }
    public void startPulling()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(0, 0, 0);
        rb.angularVelocity = new Vector3(0, 0, 0);
        pulling = true;
    }

    public void stopPulling()
    {
        pulling = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (last != other.gameObject)
            {
                source.PlayOneShot(hitSound);
                WindBullet bullet = other.gameObject.GetComponent<WindBullet>();
                direction = other.gameObject.transform.forward;
                charge = bullet.charge;
                hit = true;

                last = other.gameObject;
            }
            
        }
        if (other.gameObject.CompareTag("Bomb"))
        {
            if (last != other.gameObject)
            {
                source.PlayOneShot(hitSound);

            }
        }
        if (other.gameObject.CompareTag("Catch"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(0,0,0);


            
        } 

        if (other.gameObject.CompareTag("Bounds"))
        {
           GameController controller = GameObject.FindWithTag("GameController").GetComponent<GameController>();

            controller.Die();
        }
        if (other.gameObject.CompareTag("BallBound"))
        {
            Explode();
        }

        if (other.gameObject.CompareTag("Target"))
        {
            if (!marked)
            {
                GameObject target = other.gameObject.transform.parent.gameObject;
                TargetController tc = target.GetComponent<TargetController>();
                tc.Explode();
                Explode();
                other.gameObject.SetActive(false);

                RoomController rc = room.GetComponent<RoomController>();
                rc.addPoint();

            }

        }
    }

    public void Explode()
    {
        source.PlayOneShot(explodingSound);
        body.SetActive(false);
        particles.SetActive(true);
        ballSprite.SetActive(false);
        exploded = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = new Vector3(0, 0, 0);
    }

    public void markBomb()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            marked = true;
            MeshRenderer mr = ballSprite.GetComponent<MeshRenderer>();
            mr.material = markedSprite;
        }
    }

    public void bombCaught(Transform firepoint)
    {
        caught = true;
        caughtPos = firepoint;
        gameObject.transform.parent = caughtPos.transform;
        gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void launchHeld(float newCharge, Vector3 dir)
    {
        transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        direction = dir;
        charge = newCharge;
        hit = true;
        caught = false;
    }

    public bool checkCaught()
    {
        return caught;
    }

    public bool checkMark()
    {
        return marked;
    }

    public void unmarkBomb()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {  
            marked = false;
            MeshRenderer mr = ballSprite.GetComponent<MeshRenderer>();
            mr.material = originalSprite;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        linearVel = rb.linearVelocity;
        if (caught) {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }else if (exploded)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                Destroy(gameObject);
            }

        } else
        {
            if (hit)
            {

                float force = 50f;
                force = force * charge;
                //Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce(direction * force, ForceMode.Impulse);
                hit = false;
            }
        }

        
    }
}
