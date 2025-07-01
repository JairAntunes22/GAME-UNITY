using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Animator anim;
    [SerializeField] private TrailRenderer tr;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private int totalJumps = 2;

    [Header("Dash Settings")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    [Header("Wall Slide/Jump Settings")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingDuration = 0.4f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDist = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Knockback Settings")]
    [SerializeField] private float KBForce = 10f;
    [SerializeField] private float KBTime = 0.5f;
    public float KBCount;
    public bool isKnockRight;

    [Header("Special Dash Settings")]
    [SerializeField] private int specialDashCost = 30;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private float slowFactor = 0.5f;

    [Header("Special Attack Settings")]
    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private Transform sliceSpawnPoint;
    [SerializeField] private int sliceEnergyCost = 25;
    [SerializeField] private float sliceCooldown = 2f;
    [SerializeField] private SliceCooldownUI sliceCooldownUI;
    private float lastSliceTime = -999f;
    private bool isSlowed = false;

    [SerializeField] private PlayerEnergy energy; // Referência ao componente de energia

    private int jumpLess;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingCounter;
    private bool isDashing;
    private bool canDash = true;
    private float horizontal;

    private bool isGroundedCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpLess = totalJumps;

        if (energy == null)
            energy = GetComponent<PlayerEnergy>(); // tenta pegar automaticamente se não atribuído
    }

    void Update()
    {
        if (isDashing) return;

        GetInput();
        HandleFlip();
        HandleAnimations();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(SpecialDash());
        }

        WallSlide();
        WallJump();
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        Move();
        GroundCheck();
        KnockbackLogic();
    }

    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (CanJump())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpLess--;
            }
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            anim.Play("AttackC", -1);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            anim.Play("AttackB", -1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
{
    // Verifica cooldown
    if (Time.time < lastSliceTime + sliceCooldown)
    {
        Debug.Log("Slice em cooldown!");
        return;
    }

    // Verifica energia
    if (energy != null && energy.GetCurrentEnergy() >= sliceEnergyCost)
    {
        bool usedEnergy = energy.UseEnergy(sliceEnergyCost);
        if (usedEnergy)
        {
            Debug.Log("Energia consumida, executando slice.");
            anim.Play("AttackA", -1);
            StartCoroutine(DelayedSliceLaunch());

            // Inicia visual de cooldown se estiver atribuído
            if (sliceCooldownUI != null)
                sliceCooldownUI.StartCooldown(sliceCooldown);
        }
        else
        {
            Debug.Log("Falha ao consumir energia.");
        }
    }
    else
    {
        Debug.Log("Energia insuficiente para usar Slice!");
    }
}



    }

    private void LaunchSlice()
    {
        if (Time.time < lastSliceTime + sliceCooldown)
            return;

        GameObject slice = Instantiate(slicePrefab, sliceSpawnPoint.position, Quaternion.identity);
        float dir = isFacingRight ? -1f : 1f;
        slice.GetComponent<Slice>().SetDirection(dir);

        lastSliceTime = Time.time;
    }



    void Move()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
    }

    public void ApplyKnockback(bool knockFromRight)
    {
        KBCount = KBTime;
        isKnockRight = knockFromRight;
    }
    public void TakeDamage()
    {
        anim.SetTrigger("TakeDamage");
    }



    void GroundCheck()
    {
        isGroundedCheck = Physics2D.OverlapCircle(groundCheck.position, groundDist, groundLayer);
        if (isGroundedCheck && rb.linearVelocity.y <= 0)
        {
            jumpLess = totalJumps;
        }
    }

    bool CanJump()
    {
        return jumpLess > 0;
    }

    void WallSlide()
    {
        if (IsWalled() && !isGroundedCheck && Mathf.Abs(horizontal) > 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlidingSpeed);
            anim.SetBool("isWallSliding", true);

            // Resetar os pulos ao tocar na parede
            jumpLess = totalJumps;
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("isWallSliding", false);
        }
    }

    void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            float direction = -transform.localScale.x;
            rb.linearVelocity = new Vector2(direction * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != direction)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    bool IsWalled()
    {
        bool wall = Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
        Debug.Log("Is touching wall? " + wall);
        return wall;
    }

    void HandleFlip()
    {
        if (!isWallJumping)
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
    }

    void HandleAnimations()
    {
        anim.SetFloat("HorizontalAnim", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("VerticalAnim", rb.linearVelocity.y);
        anim.SetBool("groundCheck", isGroundedCheck);
    }

    void KnockbackLogic()
    {
        if (KBCount > 0)
        {
            rb.linearVelocity = new Vector2(isKnockRight ? -KBForce : KBForce, KBForce);
            KBCount -= Time.deltaTime;
        }
    }

    private IEnumerator SpecialDash()
    {
        canDash = false;

        if (energy != null && energy.UseEnergy(specialDashCost))
        {
            isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;

            // IGNORAR COLISÃO COM INIMIGOS DURANTE O DASH
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Inimigo"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), true);

            rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
            tr.emitting = true;

            yield return new WaitForSeconds(dashingTime);

            tr.emitting = false;
            rb.gravityScale = originalGravity;
            isDashing = false;

            // VOLTAR A DETECTAR COLISÃO COM INIMIGOS
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Inimigo"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), false);
        }
        else
        {
            yield return ApplySlow();
        }

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    private IEnumerator ApplySlow()
    {
        if (isSlowed) yield break;

        isSlowed = true;
        float originalSpeed = moveSpeed;
        moveSpeed *= slowFactor;

        Debug.Log("Personagem exausto: movimento reduzido!");

        yield return new WaitForSeconds(slowDuration);

        moveSpeed = originalSpeed;
        isSlowed = false;
    }

    private IEnumerator DelayedSliceLaunch()
    {
        float delay = 0.2f; // tempo em segundos (ajuste como quiser)
        yield return new WaitForSeconds(delay);

        LaunchSlice();
    }


    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundDist);
        }

        if (wallCheck != null)
        {
            Gizmos.DrawWireSphere(wallCheck.position, 0.2f);
        }
    }
}
