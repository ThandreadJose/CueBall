using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RawImage held;
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask floor;
    [SerializeField] private Transform holdPos;
    [SerializeField] private GameObject catchCollider;
    [SerializeField] GameObject marked;
    private BombController controlMarked;
    private GameObject heldBomb;
    private bool weaponActive = false;
    private RawImage weaponObj;
    private HeldItem weapon;
    [SerializeField]private TypewriteEffect typewriter;
    private float baseFOV = 90f;
    private bool pulling = false;
    private bool chargingBool;
    private Coroutine audioCoroutine;
    private Coroutine pullingCoroutine;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip charging;
    [SerializeField] private AudioClip shoot;
    [SerializeField] private AudioClip pullingSound;
    






    [SerializeField] private float force = 25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initializeWeapon();
        weaponActive = true;

    }

    public void initializeWeapon()
    {
        
        weaponObj = Instantiate(held, canvas.transform);
        weapon = weaponObj.GetComponent<HeldItem>();

    }

    public void markBomb(GameObject bomb)
    {
        if (marked != null)
        {
            controlMarked.unmarkBomb();
        }
        marked = bomb;
        controlMarked = bomb.GetComponentInParent<BombController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bomb"))
        {
            if (other.gameObject != heldBomb)
            {
                if (!pulling)
                {
                    GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
                    BombController bc = other.gameObject.transform.parent.GetComponent<BombController>();
                    bc.Explode();
                    gameController.Die();
                }
            }

        }
        if (other.CompareTag("PlayerBound"))
        {
            GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            gameController.Die();
        }
    }
    public Transform getHold()
    {
        return holdPos;
    }
    public void playerLaunch(float charge)
    {
        float maxDistance = 5f;
        float medDistance = 3.5f;
        float smallDistance = 2.5f;

        Vector3 origin = player.transform.position;
        Vector3 direction = firePoint.forward;

        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, smallDistance, floor))
        {

            force = 75f;


        }else if (Physics.Raycast(origin, direction, medDistance, floor))
        {

            force = 50f;

        }else if (Physics.Raycast(origin, direction, maxDistance, floor))
        {
            force = 25f;

        }

        force = force * charge;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.AddForce(-direction * force, ForceMode.Impulse);


    }

    public void catchBomb(GameObject bomb)
    {
        heldBomb = bomb;
    }

    public IEnumerator chargingSound()
    {
        audioSource.clip = charging;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        audioCoroutine = null;
    }

    public IEnumerator pullSound()
    {
        audioSource.clip = pullingSound;
        audioSource.Play();

        yield return new WaitWhile(() => audioSource.isPlaying);

        pullingCoroutine = null;
    }
    // Update is called once per frame
    void Update()
    {
         if (weaponActive == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (audioCoroutine == null || weapon.checkCharge() <= 0)
                {
                    audioCoroutine = StartCoroutine(chargingSound());
                }
                weapon.Charge(this);

                float fovDiff = weapon.checkCharge() * 20;

                float calcfov = 10 * Mathf.Sin(fovDiff / 2 - Mathf.PI / 2) + 10;

                _camera.fieldOfView = baseFOV + fovDiff;
                if (charging == false)
                {
                    
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                audioSource.Stop();
                StopCoroutine(audioCoroutine);
                audioCoroutine = null;
                
                audioSource.PlayOneShot(shoot);
                _camera.fieldOfView = baseFOV;
                string gun = weapon.getGun();
                if (gun == "GUN")
                {
                    float charge = weapon.getCharge();
                    GameObject bullet = weapon.getBullet();


                    

                    WindBullet wind = bullet.GetComponent<WindBullet>();
                    wind.charge = charge;
                    wind.owner = this.gameObject;

                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);

                    if (heldBomb != null)
                    {
                        controlMarked.launchHeld(charge, firePoint.transform.forward);
                        heldBomb = null;

                    }

                    playerLaunch(charge);



                }
            }

            if (Input.GetKey(KeyCode.E))
            {
                catchCollider.SetActive(true);
                pulling = true;

                
                if (marked != null)
                {

                    if (controlMarked.checkPulling() != true)
                    {
                        controlMarked.startPulling();
                    }
                    if (controlMarked.checkCaught() == false)
                    {
                        if (pullingCoroutine == null)
                        {
                            pullingCoroutine = StartCoroutine(pullSound());

                        }
                        Vector3 bombLocation = marked.transform.position;
                        Vector3 targetLocation = firePoint.transform.position;

                        Vector3 targetDirection = (targetLocation - bombLocation).normalized;

                        Rigidbody bombRb = marked.GetComponentInParent<Rigidbody>();


                        bombRb.AddForce(targetDirection * 10f);
                    } else if (controlMarked.checkCaught() == true){
                        if (pullingCoroutine != null)
                        {
                            audioSource.Stop();
                            StopCoroutine(pullingCoroutine);
                            pullingCoroutine = null;
                        }

                    }
                    
                }

            }
            else
            {
                pulling = false;
                catchCollider.SetActive(false);
                if (marked != null)
                {
                    
                    if (controlMarked.checkPulling())
                    {
                        controlMarked.stopPulling();
                        if (pullingCoroutine != null)
                        {
                            audioSource.Stop();
                            StopCoroutine(pullingCoroutine);
                            pullingCoroutine = null;
                        }


                    }
                }
            }
        }
        

    }
}
