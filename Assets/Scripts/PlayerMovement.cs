using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    private float sprintSpeed;
    private float baseSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode shiftKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Falling Physics")]
    public float longHold = 3;
    public float tapJump = 3.5f;
    public float airDamp = 2;

    [Header("AudioStuff")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip jumpSound;

    public Transform orientation;
    public GameObject go;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        sprintSpeed = moveSpeed * 1.5f;
        baseSpeed = moveSpeed;
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(go.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);


        MyInput();
        SpeedControl();

        //handle Drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDamp;


            if (rb.linearVelocity.y < 0) // Falling down
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (longHold - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump")) // Short hop
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (tapJump - 1) * Time.deltaTime;
            }

        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {

            readyToJump = false;

            Jump();
            source.PlayOneShot(jumpSound);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKey(shiftKey))
        {
            moveSpeed = sprintSpeed;
        }else { moveSpeed = baseSpeed; }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //Reset y Vel
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //    if (other.CompareTag("Death"))
        //    {
        //        GameController gc = GetComponentInChildren<GameController>();
        //        gc.GameOver();
        //    }
    }

}
