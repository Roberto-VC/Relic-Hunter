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

    void Start()
    {
        animator = GetComponent<Animator>();

        // If freelookCamera is not assigned, try to find it in the scene
        if (freelookCamera == null)
        {
            freelookCamera = FindObjectOfType<CinemachineFreeLook>();
            freelookCamera.m_XAxis.m_InputAxisName = "";
            freelookCamera.m_YAxis.m_InputAxisName = "";
        }
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = GetMovementDirection(horizontalInput, verticalInput);

        if (movement.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);

            // Disable input to the Freelook camera


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


        }

        isGrounded = IsGrounded(); // Now assigning the value returned by IsGrounded to the isGrounded variable

        if (isGrounded && Input.GetButtonDown("Jump")) // Now using the isGrounded variable directly
        {
            Jump();

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
                animator.SetBool("isJumping", false);
                return true;
            }
        }
        animator.SetBool("isJumping", true);
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

    private void Jump()
    {
        // Apply jump force only when the character is grounded
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

}