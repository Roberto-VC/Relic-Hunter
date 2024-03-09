using UnityEngine;
using Cinemachine;

public class PlayerAnimation : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpForce = 10f;
    public float backwardMovementMultiplier = 0.5f; // Adjust this value for the backward movement speed
    public CinemachineFreeLook freelookCamera;
    public LayerMask groundLayer; // Assign the ground layer in the inspector
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public string groundTag = "Ground";

    private Animator animator;
    private bool isMovingBackward = false;
    private bool isGrounded;

    public GemManager gemas;
    public KeyManager keys;

    public GameObject feet;
    public GameObject run;
    public AudioClip touchSound; // Sound clip to play when the character touches something
    public float volume = 1f; // Volume of the sound clip

    private AudioSource audioSource;
    public GameObject winScreen;


    private bool treasure;
    private GameObject chest;
    private int tesoros = 2;


    void Start()
    {
        animator = GetComponent<Animator>();

        // If freelookCamera is not assigned, try to find it in the scene
        if (freelookCamera == null)
        {
            freelookCamera = FindObjectOfType<CinemachineFreeLook>();
            //freelookCamera.m_XAxis.m_InputAxisName = "";
            freelookCamera.m_YAxis.m_InputAxisName = "";
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null || touchSound == null)
        {
            Debug.LogError("AudioSource or AudioClip is not assigned.");
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = GetMovementDirection(horizontalInput, verticalInput);

        Debug.Log(tesoros);

        if (chest != null && Input.GetKeyDown(KeyCode.T))
        {
            chest.SetActive(false);
            Collider chestCollider = chest.GetComponent<Collider>(); // Get the collider component
            if (chestCollider != null)
            {
                chestCollider.enabled = false; // Disable the collider
            }
            tesoros -= 1;
            chest = null;
            gemas.treasures--;

        }

        if (movement.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
            feet.SetActive(true);

            // Disable input to the Freelook camera

            if (Input.GetKey(KeyCode.LeftShift) && movement.magnitude > 0.1f)
            {
                animator.SetBool("isRunning", true);
                moveSpeed = 8f;
                feet.SetActive(false);
                run.SetActive(true);

            }
            else
            {
                animator.SetBool("isRunning", false);
                moveSpeed = 5f;
                feet.SetActive(true);
                run.SetActive(false);

            }


            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            // Check if moving backward relative to the camera
            float dotProduct = Vector3.Dot(transform.forward, movement);
            if (dotProduct < 0)
            {
                isMovingBackward = true;
                movement *= backwardMovementMultiplier;
            }
            else
            {
                isMovingBackward = false;
            }

            // Move in the direction relative to the camera
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            animator.SetBool("isMoving", false);
            isMovingBackward = false;
            feet.SetActive(false);
        }

        isGrounded = IsGrounded(); // Now assigning the value returned by IsGrounded to the isGrounded variable

        if (isGrounded && Input.GetButtonDown("Jump")) // Now using the isGrounded variable directly
        {
            Jump();
        }

        // Return to normal state after the jump animation finishes playing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            animator.SetBool("isJumping", false);
        }
        if (tesoros == 0)
        {
            ShowWinScreen();
        }
    }

    private bool IsGrounded()
    {
        // Perform a raycast downward to check if the character is grounded based on the ground tag
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f))
        {
            if (hit.collider.CompareTag(groundTag))
            {
                return true;
            }
            Debug.Log(hit.collider.tag);
        }
        return false;
    }

    private Vector3 GetMovementDirection(float horizontalInput, float verticalInput)
    {
        // Calculate movement direction relative to the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        Vector3 movement = (verticalInput * cameraForward + horizontalInput * cameraRight).normalized;
        movement.y = 0f; // Keep movement in the horizontal plane
        return movement;
    }

    void ShowWinScreen()
    {
        // Activate the win screen UI element
        winScreen.SetActive(true);
    }

    private void Jump()
    {
        // Play the jump animation
        animator.SetBool("isJumping", true);

        // Apply jump force only when the character is grounded
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)

    {

        if (other.gameObject.CompareTag("Gema"))
        {
            gemas.count++;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(touchSound, volume);
        }


        if (other.gameObject.CompareTag("DoorKey"))
        {
            keys.doorkey = true;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(touchSound, volume);
        }
        if (other.gameObject.CompareTag("ChestKey"))
        {
            keys.chestkey = true;
            Destroy(other.gameObject);
            audioSource.PlayOneShot(touchSound, volume);
        }

        if (other.gameObject.CompareTag("Treasure") && keys.chestkey == true)
        {

            treasure = true;
            chest = other.gameObject;
            audioSource.PlayOneShot(touchSound, volume);
        }


    }
    void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone of the treasure
        if (other.gameObject.CompareTag("Treasure"))
        {
            treasure = false;
            chest = null;
        }

    }

}
