using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    public float jumpSpeed;

    [SerializeField]
    private float JumpHorizontalSpeed;

    [SerializeField]
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float doubleJumpMultiplier = 0.7f;


    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private Animator animator;

    private bool isJumping;
    private bool isGrounded;
    private bool isSliding;
    private Vector3 slopeSlideVelocity;

    private float timeInAir = 0f;
    [SerializeField]
    private float minAirTimeForSlide = 0.5f; // medio segundo en el aire antes de permitir slide

    private bool hasDoubleJumped = false;

    public static PlayerMovement instance;


    //para el knockback cuando se hace daño
    public bool isKnocking;
    public float knockbackLength = .5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;

    public GameObject[] playerPieces;

    //para poder acceder a las propiedades del script desde otro
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        animator = GetComponent<Animator>();

       characterController = GetComponent<CharacterController>();
       originalStepOffset = characterController.stepOffset;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!isKnocking)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                inputMagnitude /= 2;
            }
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //se mueve acorde a la cámara
            movementDirection.Normalize(); //evitar que se mueva más rápido en diagonal

            ySpeed += Physics.gravity.y * 1.65f * Time.deltaTime;

            SetSlopeSlideVelocity();

            if (slopeSlideVelocity == Vector3.zero)
            {
                isSliding = false;
            }

            if (characterController.isGrounded)
            {
                timeInAir = 0f;
                lastGroundedTime = Time.time;
            }
            else
            {
                timeInAir += Time.deltaTime;
            }


            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonPressedTime = Time.time;
            }

            if (Time.time - lastGroundedTime <= jumpButtonGracePeriod) // Está en el suelo
            {

                if (slopeSlideVelocity != Vector3.zero) //Se está deslizando 
                {
                    isSliding = true;
                }

                characterController.stepOffset = originalStepOffset;

                if (!isSliding)
                {
                    ySpeed = -0.5f;
                }

                animator.SetBool("IsGrounded", true);
                isGrounded = true;
                //animator.SetBool("IsJumping", false);
                isJumping = false;
                animator.SetBool("IsFalling", false);


                hasDoubleJumped = false;

                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && !isSliding)  //comprobar condiciones para hacer saltar o no
                {
                    ySpeed = jumpSpeed;
                    // animator.SetBool("IsJumping", true);
                    animator.SetTrigger("JumpTrigger");
                    isJumping = true;
                    jumpButtonPressedTime = null;
                    lastGroundedTime = null;

                }
            }
            else
            {
                characterController.stepOffset = 0;
                animator.SetBool("IsGrounded", false);
                isGrounded = false;

                if ((isJumping && ySpeed < 0) || ySpeed < -2) //si está cayendo por salto o cambio de superficie
                {
                    if (timeInAir >= minAirTimeForSlide || isJumping)
                    {
                        animator.SetBool("IsFalling", true);
                    }
                }


                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && !hasDoubleJumped) //doble salto
                {
                    ySpeed = jumpSpeed * doubleJumpMultiplier;
                    // animator.SetBool("IsJumping", true);
                    animator.SetFloat("JumpSpeedMultiplier", 0.3f); // solo afecta a la animación Jumping
                    animator.Play("Jumping", 0, 0.05f); // reinicia desde un poco adelante
                    Invoke(nameof(ResetAnimatorSpeed), 0.3f); //  restaurar velocidad normal

                    isJumping = true;
                    hasDoubleJumped = true;
                    jumpButtonPressedTime = null;
                }
            }





            if (movementDirection != Vector3.zero) //se está moviendo
            {
                animator.SetBool("IsMoving", true);
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            if (!isGrounded && !isSliding)
            {
                Vector3 velocity = movementDirection * inputMagnitude * JumpHorizontalSpeed;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);
            }

            if (isSliding)
            {
                Vector3 velocity = slopeSlideVelocity;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);
            }
        }

        if (isKnocking)
        {
            knockbackCounter -= Time.deltaTime;

            Vector3 knockDirection = -transform.forward * knockbackPower.x + Vector3.up * knockbackPower.y;
            characterController.Move(knockDirection * Time.deltaTime);

            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }
        }
    }

    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= characterController.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, ySpeed,0), hitInfo.normal);
                return;
            }
        }

        if (isSliding) //Parar gradualmente la animación
        {
            slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * 3;

            if(slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }
        slopeSlideVelocity = Vector3.zero;
    }

    private void OnAnimatorMove()
    {
        if (isGrounded && !isSliding)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
       
    }
    private void ResetAnimatorSpeed()
    {
        animator.SetFloat("JumpSpeedMultiplier", 1f);
    }


    public void Knockback()
    {
        isKnocking = true;
        knockbackCounter = knockbackLength;
        Debug.Log("Knocked back");
    }
}

