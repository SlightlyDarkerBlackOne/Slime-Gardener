using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    private enum State
    {
        Idle,
        Normal,
        Rolling,
    }

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float dashSpeed = 2;
    [SerializeField]
    private float rollSpeed = 19;
    private float rollSpeedOngoing;
    [Space(20)]
    [SerializeField]
    private LayerMask dashLayerMask;

    private Rigidbody2D rb;
    private Vector3 moveDir;
    private Vector3 lastMoveDir;
    private Vector3 rollDir;

    private Animator anim;
    private State state;

    public float startTimeBtwTrail;
    private float timeBtwTrail;
    public GameObject trailEffect;

    private float dashTime;
    public float startDashTime;

    public float speedBonusModifier = 1;
    private float startMoveBonusCooldown;
    public float MoveBonusCooldown { get; private set; }
    public float moveBonusDuration;
    public bool grantMoveBonus;

    private bool playerMoving;
    private bool isDashButtonDown;
    public bool playerFrozen = false;

    #region Singleton
    public static PlayerController2D Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        //anim = transform.Find("Animation").GetComponent<Animator>();
        state = State.Normal;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        Move();
        //SetAnimations();
        //TrailEffect();
        SetMoveSpeedForADuration();

        if (Input.GetKeyDown(KeyCode.F))
        {
            isDashButtonDown = true;
        }
        if (dashTime >= 0)
        {
            isDashButtonDown = false;
        }
        dashTime -= Time.deltaTime;

        if (playerFrozen)
        {
            state = State.Idle;
            playerMoving = false;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Idle:
                rb.velocity = Vector2.zero;
                break;
            case State.Normal:
                rb.velocity = moveDir * moveSpeed;
                Dash();
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
        }
    }
    private void Move()
    {
        switch (state)
        {
            case State.Idle:
                if (!playerFrozen)
                    state = State.Normal;
                break;
            case State.Normal:
                float moveX = 0f;
                float moveY = 0f;

                if (Input.GetKey(KeyCode.W))
                {
                    moveY = +1f;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    moveY = -1f;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    moveX = -1f;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    moveX = +1f;
                }
                if (moveX != 0 || moveY != 0)
                {
                    playerMoving = true;
                    lastMoveDir = moveDir;
                }
                else
                {
                    playerMoving = false;
                }
                moveDir = new Vector3(moveX, moveY).normalized;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rollDir = lastMoveDir;
                    rollSpeedOngoing = rollSpeed;
                    if (dashTime <= 0)
                    {
                        SFXManager.Instance.PlaySound(SFXManager.Instance.dash);
                        state = State.Rolling;
                    }
                }
                break;
            case State.Rolling:
                Roll();
                break;
        }
    }
    private void Roll()
    {
        float rollSpeedDropMultiplier = 5f;
        rollSpeedOngoing -= rollSpeedOngoing * rollSpeedDropMultiplier * Time.deltaTime;
        float rollSpeedMinimum = 10f;
        if (rollSpeedOngoing < rollSpeedMinimum)
        {
            state = State.Normal;
        }
    }
    private void Dash()
    {
        if (isDashButtonDown && dashTime <= 0)
        {
            dashTime = startDashTime;
            Vector3 dashPosition = transform.position + lastMoveDir * dashSpeed;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDir,
                    dashSpeed, dashLayerMask);
            if (raycastHit2D.collider != null)
            {
                dashPosition = raycastHit2D.point;
            }

            rb.MovePosition(dashPosition);

         
            SFXManager.Instance.PlaySound(SFXManager.Instance.dash);
            isDashButtonDown = false;
        }
    }
    private void SetAnimations()
    {
        anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        anim.SetBool("PlayerMoving", playerMoving);
        anim.SetFloat("LastMoveX", lastMoveDir.x);
        anim.SetFloat("LastMoveY", lastMoveDir.y);
    }
    private void TrailEffect()
    {
        if (playerMoving)
        {
            if (timeBtwTrail <= 0)
            {
                GameObject effect = Instantiate(trailEffect, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
                timeBtwTrail = startTimeBtwTrail;
            }
            else
            {
                timeBtwTrail -= Time.deltaTime; ;
            }
        }
    }
    private void SetMoveSpeedForADuration()
    {
        if (grantMoveBonus)
        {
            if (SpeedNotOnCooldown())
            {
                MoveBonusCooldown = startMoveBonusCooldown;
                moveSpeed *= speedBonusModifier;
            }
            if (moveBonusDuration <= 0)
            {
                moveSpeed /= speedBonusModifier;
                grantMoveBonus = false;
            }
            moveBonusDuration -= Time.deltaTime;
        }
        MoveBonusCooldown -= Time.deltaTime;
    }
    public bool SpeedNotOnCooldown()
    {
        if (MoveBonusCooldown <= 0)
            return true;
        else return false;
    }
    public void FrezePlayer()
    {
        playerFrozen = true;
    }
    public void UnFreezePlayer()
    {
        playerFrozen = false;
    }
    public void IncreaseMoveSpeed()
    {
        moveSpeed += moveSpeed / 10;
    }
    public void SetMoveSpeedBonuses(float speedModifier, float duration, float cooldown)
    {
        grantMoveBonus = true;
        speedBonusModifier = speedModifier;
        moveBonusDuration = duration;
        startMoveBonusCooldown = cooldown;
    }

}