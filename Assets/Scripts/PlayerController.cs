using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    public Skeleton skeleton;

    public TouchController touchController;
    public TimeController timeController;

    public GameObject projectile;
    public ParticleSystem jumpParticle;
    public ParticleSystem oilLeakParticle;
    public ParticleSystem chargeParticle;

    public SwappableTileEffector tileEffector;

    public float maxHealth;
    public float currentHealth;
    public float maxAmmo;
    public float currentAmmo;

    public float itemPickupRange;
    public LayerMask itemLayer;

    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    public LayerMask absoluteGroundLayer;

    public bool isWallSliding;
    public float wallSlideGravity;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float wallCheckRadius;
    public LayerMask wallLayer;

    public Vector2 wallJumpDirection;
    public float walljumpForce;

    public float moveAcceleration;
    public float maxMoveSpeed;

    public float jumpForce;
    public float jumpDefaultCount;
    private float jumpCounter;

    public float shootForce;
    private BoxCollider2D collider;
    public float slideForce;
    public float slideDuration;
    public float slideTimer;
    public Vector2 slideColliderOffset;
    public Vector2 slideColliderScale;
    private Vector2 colliderDefaultOffset;
    private Vector2 colliderDefaultSize;
    private Vector2 defaultScale;

    private Vector2 lookDirection;

    public Rigidbody2D rb;
    public LineRenderer lr;
    private float gravityDefaultScale;
    private float defaultDrag;

    public Transform shootingPoint;
    public bool invertedAiming;
    public float aimLineLength;
    private Vector2 aimDirection;
    private Vector2 dragStartPosition;
    public bool focusMode;
    public float deathYHeight;
    public bool isDead;
    private Vector3 startPosition;


    void Awake()
    {
        skeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        animationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        collider = GetComponent<BoxCollider2D>();

        currentHealth = maxHealth;
        currentAmmo = maxAmmo;

        gravityDefaultScale = rb.gravityScale;
        defaultScale = transform.localScale;
        defaultDrag = rb.drag;
        startPosition = transform.position;

        slideTimer = slideDuration;
        colliderDefaultOffset = collider.offset;
        colliderDefaultSize = collider.size;

        stateMachine.ChangeState(new IdleState(this));
    }

    private void Start()
    {
    }


    private void Update()
    {
            stateMachine.Update();
            HandleFocusModeInput();

            if (focusMode && !isDead)
            {
                if (Input.GetMouseButton(0))
                {
                    ExecuteFocusMode();
                }
                if (Input.GetMouseButtonUp(0))
                {


                    if (aimDirection.magnitude > 0)
                    {
                        Shoot(aimDirection.normalized);
                        currentAmmo--;
                        if (currentAmmo <= 1)
                        {
                            oilLeakParticle.Play();
                            tileEffector.enabled = false;
                        }
                    }

                StopFocusMode();

                skeletonAnimation.state.SetAnimation(0, "Haduken Fire", false);
                    skeletonAnimation.state.AddAnimation(0, "Haduken End", false, 0.433f);
                    skeletonAnimation.state.AddAnimation(0, "Running", true, 0.633f);
                    
                }
            }
            DetectItems();
            HandleHealth();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AmmoPickup"))
        {
            FillAmmo(0.1f);
            if(currentAmmo >= 1)
            {
                oilLeakParticle.Stop();
                tileEffector.enabled = true;
            }
            Destroy(other.gameObject.gameObject);
        }

        if(other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallCheckLeft.position, wallCheckRadius);
        Gizmos.DrawWireSphere(wallCheckRight.position, wallCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, itemPickupRange);
        Gizmos.DrawWireSphere(shootingPoint.position, 0.2f);
    }
    void DetectItems()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, itemPickupRange, itemLayer);
        foreach (Collider2D collider in colliders)
        {
            if(collider.CompareTag("AmmoPickup"))
            {
                collider.GetComponent<ItemPickup>().target = transform;
            }
        }
    }

    IEnumerator JumpOff()
    {
        int layerIndex = LayerMask.NameToLayer("Platform");
        Physics2D.IgnoreLayerCollision(gameObject.layer, layerIndex, true);
        yield return new WaitForSeconds(.5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, layerIndex, false);
        SoundController.Instance.Play("Fart");
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null
            || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, absoluteGroundLayer);
    }

    public Vector2 CheckWall()
    {
        if (Physics2D.OverlapCircle(wallCheckLeft.position, wallCheckRadius, wallLayer))
        {
            SetLookDirection(Vector2.right);
            return -Vector2.right;
        }

        if (Physics2D.OverlapCircle(wallCheckRight.position, wallCheckRadius, wallLayer))
        {
            SetLookDirection(-Vector2.right);
            return Vector2.right;
        }

        return Vector2.zero;
    }

    void HandleHealth()
    {
        if ((currentHealth <= 0 || transform.position.y < deathYHeight) && !isDead)
        {
            stateMachine.ChangeState(new DeadState(this));
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    void Die()
    {

    }

    public void FillAmmo(float amount)
    {
        if (currentAmmo + amount > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        else
        {
            currentAmmo += amount;
        }
    }

    public void HandleFocusModeInput()
    {
        if (touchController.GetLongPress() && GameController.Instance.gameState == GameController.GameState.Ingame)
        {
            StartFocusMode();
        }
    }

    public void HandleJumpOffInput()
    {
        if (touchController.GetSwipeDown())
        {
            StartCoroutine(JumpOff());
        }
    }

    public void HandleJumpInput()
    {
        if (touchController.GetTap())
        {
            Jump();
        }
    }

    public void HandleWallJumpInput()
    {
        if (touchController.GetTap())
        {
            WallJump();
        }
    }


    public void HandSlideInput()
    {
        if (touchController.GetSwipeRight())
        {
            SoundManager.Instance.Play("dash");
            collider.offset = slideColliderOffset;
            collider.size = slideColliderScale;
            stateMachine.ChangeState(new SlidingState(this));
            skeletonAnimation.state.SetAnimation(0, "Slide Start", false);
            skeletonAnimation.state.AddAnimation(0, "Slide Mid", true, 0.267f);
        }
    }

    void Shoot(Vector2 direction)
    {
        SoundManager.Instance.Play("cannon_shot");
        GameObject newProjectile = Instantiate(projectile, shootingPoint.position, transform.rotation);
        newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * shootForce);
    }

    public void Move(Vector2 direction)
    {
        SetLookDirection(direction);
        rb.AddForce(lookDirection * moveAcceleration);
        LimitMoveSpeed();
    }

    public void MoveMaxSpeed(Vector2 direction)
    {
        SetLookDirection(direction);
        rb.velocity = new Vector2(maxMoveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (jumpCounter > 0)
        {
            if(jumpCounter > 1)
            {
                SoundManager.Instance.Play("jumping", 1, 0.8f);
            } else
            {
                SoundManager.Instance.Play("jumping", 1, 1f);
            }
            
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpParticle.Play();
            jumpCounter--;
            if(!CheckGround())
            {
                skeletonAnimation.state.SetAnimation(0, "Double Jump", false);
                skeletonAnimation.state.AddAnimation(0, "Falling Animation", false, 0.2f);
            }
            else
            {
                skeletonAnimation.state.SetAnimation(0, "Jump", false);
                skeletonAnimation.state.AddAnimation(0, "Falling Animation", false, 0.2f);
            }

        }
    }

    public void ResetJumpCounter()
    {
        jumpCounter = jumpDefaultCount;
    }

    public void ResetCollider()
    {
        collider.offset = colliderDefaultOffset;
        collider.size = colliderDefaultSize;
    }

    void WallJump()
    {
        rb.AddForce(new Vector2(wallJumpDirection.x * lookDirection.x, wallJumpDirection.y) * walljumpForce, ForceMode2D.Impulse);
        jumpParticle.Play();
    }

    public void SetLookDirection(Vector2 direction)
    {
        lookDirection = direction;
        if (lookDirection == Vector2.right)
        {

        }
        else if (lookDirection == -Vector2.right)
        {

        }
    }

    public void StartFocusMode()
    {
        if (currentAmmo >= 1)
        {
            SoundManager.Instance.PlayLoop("charge");
            skeletonAnimation.state.SetAnimation(0, "Haduken Start", false);
            skeletonAnimation.state.AddAnimation(0, "Haduken Charging", true, 0.5f);
            chargeParticle.Play();
            focusMode = true;
            lr.enabled = true;
            dragStartPosition = Input.mousePosition;
            timeController.StartSlowMotion();
        }
    }

    public void ExecuteFocusMode()
    {
        aimDirection = invertedAiming ? (dragStartPosition - (Vector2)Input.mousePosition) : -(dragStartPosition - (Vector2)Input.mousePosition);
        DrawLine(shootingPoint.position, ((Vector2)shootingPoint.position + (aimDirection.normalized * aimLineLength)));
    }

    public void StopFocusMode()
    {
        chargeParticle.Stop();
        timeController.StopSlowMotion();
        focusMode = false;
        lr.enabled = false;
        SoundManager.Instance.Stop("charge");
    }

    public void StartWallSlide()
    {
        if (CheckGround())
        {
            rb.velocity = new Vector2(0, rb.velocity.x / 2);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        rb.gravityScale = wallSlideGravity;
        rb.drag = 0;
    }

    public void StopWallSlide()
    {
        rb.gravityScale = gravityDefaultScale;
        rb.drag = defaultDrag;
    }

    void LimitMoveSpeed()
    {
        float xVelocity = Mathf.Min(Mathf.Abs(rb.velocity.x), maxMoveSpeed) * Mathf.Sign(rb.velocity.x);
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        skeletonAnimation.ClearState();
        transform.position = startPosition;
        isDead = false;
        stateMachine.ChangeState(new GroundedState(this));
    }

    public bool isYVelocityZero()
    {
        return rb.velocity.y == 0;
    }
}

public class IdleState : State
{
    public string id { get; set; } = "IdleState";
    PlayerController owner;

    public IdleState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {

    }

    public void Execute()
    {

    }

    public void Exit()
    {
        SoundManager.Instance.Stop("footsteps");
    }
}

public class GroundedState : State
{
    public string id { get; set; } = "GroundedState";
    PlayerController owner;

    public GroundedState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.ResetJumpCounter();
        SoundManager.Instance.Play("landing");
        SoundManager.Instance.PlayLoop("footsteps");
        owner.skeletonAnimation.state.SetAnimation(0, "Running", true);
    }

    public void Execute()
    {
        owner.MoveMaxSpeed(Vector2.right);
        owner.HandleJumpInput();
        owner.HandSlideInput();
        owner.HandleJumpOffInput();

        if (owner.focusMode)
        {
            SoundManager.Instance.Get("footsteps").volume = 0;
        } else
        {
            SoundManager.Instance.Get("footsteps").volume = 1;
        }

        if (!owner.CheckGround())
        {
            owner.stateMachine.ChangeState(new JumpingState(owner));
        }

        Vector2 detectedWall = owner.CheckWall();
        if (owner.CheckWall() != Vector2.zero)
        {
            owner.SetLookDirection(-detectedWall);
            owner.stateMachine.ChangeState(new WallSlidingState(owner));
        }
    }

    public void Exit()
    {
        SoundManager.Instance.Stop("footsteps");
    }
}

public class JumpingState : State
{
    public string id { get; set; } = "JumpingState";
    PlayerController owner;

    public JumpingState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        owner.MoveMaxSpeed(Vector2.right);
        owner.HandleJumpInput();

        if (owner.CheckGround() && owner.isYVelocityZero() && !owner.focusMode)
        {
            owner.stateMachine.ChangeState(new GroundedState(owner));
        }

        if (owner.CheckWall() != Vector2.zero)
        {
            owner.stateMachine.ChangeState(new WallSlidingState(owner));
        }
    }

    public void Exit()
    {

    }
}

public class WallSlidingState : State
{
    public string id { get; set; } = "WallSlidingState";
    PlayerController owner;

    public WallSlidingState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        owner.StartWallSlide();
    }

    public void Execute()
    {
        owner.HandleWallJumpInput();

        if (owner.CheckWall() == Vector2.zero)
        {
            if (owner.CheckGround())
            {
                owner.stateMachine.ChangeState(new GroundedState(owner));
            }
            else
            {
                owner.stateMachine.ChangeState(new WallJumpingState(owner));
            }
        }
    }

    public void Exit()
    {
        owner.StopWallSlide();
    }
}

public class WallJumpingState : State
{
    public string id { get; set; } = "WallJumpingState";
    PlayerController owner;

    public WallJumpingState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        if (owner.CheckGround())
        {
            owner.stateMachine.ChangeState(new GroundedState(owner));
        }

        if (owner.CheckWall() != Vector2.zero)
        {
            owner.stateMachine.ChangeState(new WallSlidingState(owner));
        }
    }

    public void Exit()
    {

    }
}

public class SlidingState : State
{
    public string id { get; set; } = "SlidingState";
    PlayerController owner;
    float timer;

    public SlidingState(PlayerController owner)
    {
        this.owner = owner;
    }

    public void Enter()
    {
        timer = owner.slideDuration;
    }

    public void Execute()
    {
        timer -= Time.deltaTime;
        owner.Move(Vector2.right * owner.slideForce);
        if (timer < 0)
        {
            if (owner.CheckGround() && !owner.focusMode)
            {
                owner.stateMachine.ChangeState(new GroundedState(owner));
            }
        }

        if (!owner.CheckGround() && owner.CheckWall() == Vector2.zero)
        {
            owner.stateMachine.ChangeState(new JumpingState(owner));
        }
        else if (owner.CheckWall() != Vector2.zero)
        {
            owner.stateMachine.ChangeState(new WallSlidingState(owner));
        }
    }

    public void Exit()
    {
        owner.ResetCollider();
    }
}

public class DeadState : State
{
    public string id { get; set; } = "DeadState";
    PlayerController owner;
    float timer;

    public DeadState(PlayerController owner)
    {
        this.owner = owner;
        owner.isDead = true;
    }

    public void Enter()
    {
        timer = 2;
        owner.touchController.enabled = false;

        SoundManager.Instance.FadeOut("gameplay_Bgm");
        SoundManager.Instance.Play("death");

        owner.StopFocusMode();

        owner.skeletonAnimation.ClearState();
        owner.skeletonAnimation.state.SetAnimation(0, "Death Animation", false);
    }

    public void Execute()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            GameController.Instance.EndGame();
        }
    }

    public void Exit()
    {

    }
}