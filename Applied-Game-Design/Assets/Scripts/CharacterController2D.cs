using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float movementSpeed = 500;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private int jumpCount = 2;

    [Header("Physics")]
    public Rigidbody2D rigidBody;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private List<Vector2> rayPositions = new List<Vector2>();
    [SerializeField] private float rayLength = 0.55f;
    public enum KeyState { Off, Down, Held, Up }
    public KeyState ksSpace = KeyState.Off;
    private int framesPassed;

    [Header("Visuals")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool facingLeft;

    [Header("Controls")]
    private PlayerControllerInput playerInput;
    private float movementInput;
    private bool jumpPressed;

    [Header("Miscelanous")]
    public Collider2D collider;
    private float horizontalMovement;
    public GameObject deathAnimationPrefab;
    private float verticalMovement;
    private Vector2 moveDirection;
    public Transform startingPosition;

    private bool canSpinAttack;
    [SerializeField] private bool isGrounded;
    private bool canJump;
    public bool canMove = false;

    private void Start()
    {
        transform.position = startingPosition.position;
        //playerInput = new PlayerControllerInput();
        //playerInput.LighterController.Enable();
        //IniateRespawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("PlatformParentCollider"))
        //{
        //    transform.SetParent(collision.transform.parent);
        //}
        //if (collision.CompareTag("FlameSpike"))
        //{
        //    deathAnimationPrefab.SetActive(true);
        //    deathAnimationPrefab.transform.position = transform.position;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("PlatformParentCollider"))
        //{
        //    transform.SetParent(null);
        //}
    }

    void Update()
    {   
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //	IniateRespawn();
        //}

        if (canMove)
        {
            PlayerInput();
        }
        FlipPlayer();

        for (int i = 0; i < rayPositions.Count; i++)
        {
            isGrounded = CheckGround();
        }

        if (!isGrounded)
        {
            framesPassed++;
            //Debug.Break();
        }
        else
        {
            framesPassed = 0;
            jumpCount = 2;
            canJump = true;
        }

        if (framesPassed >= 5 && canJump)
        {
            jumpCount = 1;
        }


        //if (isGrounded)
        //{
        //    jumpCount = 1;
        //}
    }

    private void FixedUpdate()
    {
        if (ksSpace == KeyState.Down && jumpCount > 0 && canMove)
        {
            Jump();
            ksSpace = KeyState.Off;
        }

        if (canMove)
        {
            rigidBody.velocity = new Vector2(moveDirection.x * movementSpeed, rigidBody.velocity.y);
        }
        //rigidBody.AddForce(moveDirection.normalized * movementSpeed * Time.deltaTime, ForceMode2D.Force);
        //rigidBody.AddForce(Vector3.right * moveDirection.normalized * movementSpeed * Time.deltaTime, ForceMode2D.Force);
    }

    private void ControlVelocity(float _multiplier)
    {
        if (horizontalMovement < 0)
        {
            facingLeft = true;
            rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, -7.5f * _multiplier, -5f * _multiplier), rigidBody.velocity.y);
        }
        else if (horizontalMovement > 0)
        {
            facingLeft = false;
            rigidBody.velocity = new Vector2(Mathf.Clamp(rigidBody.velocity.x, 5f * _multiplier, 7.5f * _multiplier), rigidBody.velocity.y);
        }
    }
    private void PlayerInput()
    {

        //Manage accelaration and decceleration
        if (movementInput > 0 && horizontalMovement <= 1)
        {
            horizontalMovement += 0.125f;
        }
        else if (movementInput < 0 && horizontalMovement >= -1)
        {
            horizontalMovement -= 0.125f;
        }
        else if (movementInput == 0 && horizontalMovement != 0)
        {
            if (horizontalMovement > 0)
            {
                if (horizontalMovement == 0.125f)
                {
                    horizontalMovement -= 0.125f;
                }
                else
                {
                    horizontalMovement -= 0.25f;
                }
            }
            else if (horizontalMovement < 0)
            {
                if (horizontalMovement == -0.125f)
                {
                    horizontalMovement += 0.125f;
                }
                else
                {
                    horizontalMovement += 0.25f;
                }
            }
        }

        horizontalMovement = Mathf.Clamp(horizontalMovement, -1f, 1f);

        moveDirection = new Vector2(horizontalMovement, verticalMovement);
    }


    public void OnSpin(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            canSpinAttack = true;
        }
    }

    public void OnMovement(InputAction.CallbackContext context) => movementInput = context.ReadValue<float>();

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpPressed = context.action.triggered;
        if (jumpPressed)
        {
            ksSpace = KeyState.Down;
        }
        else
        {
            ksSpace = KeyState.Up;
        }
    }
    private void Jump()
    {
        jumpCount--;
        canJump = false;
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 1 * jumpForce) * Time.deltaTime;
        //rigidBody.AddForce(new Vector2(rigidBody.velocity.x, jumpForce), ForceMode2D.Impulse);
    }

    private bool CheckGround()
    {
        int isTrue = 0;
        int isFalse = 0;
        for (int i = 0; i < rayPositions.Count; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + rayPositions[i].x, transform.position.y + rayPositions[i].y), -transform.up, rayLength, groundLayers);
            Debug.DrawRay(new Vector2(transform.position.x + rayPositions[i].x, transform.position.y + rayPositions[i].y), -transform.up * rayLength, Color.red, 0);
            if (hit)
            {
                isTrue++;
            }
            else
            {
                isFalse++;
            }
        }
        if (isTrue == rayPositions.Count)
        {
            return true;
        }
        else if (isFalse == rayPositions.Count)
        {
            return false;
        }
        else
        {
            return isGrounded;
        }
    }

    private void FlipPlayer()
    {
        if (horizontalMovement < 0)
        {
            facingLeft = true;
        }
        else if (horizontalMovement > 0)
        {
            facingLeft = false;
        }
        Vector3 _localScale = transform.localScale;
        if (facingLeft && _localScale.x > 0 || !facingLeft && _localScale.x < 0)
        {
            _localScale.x *= -1;
        }
        transform.localScale = _localScale;
    }

    public void OnDeath()
    {
        transform.position = startingPosition.position;
    }
}
