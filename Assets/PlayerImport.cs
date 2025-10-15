using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerImport : MonoBehaviour
{
public Animator animator;

    public float groundCheckDistance = 0.1f;
    public float wallRaycastDistance = 0.6f;
    public ContactFilter2D groundCheckFilter;

    private Rigidbody2D rb;
    private Collider2D collider2d;
    private List<RaycastHit2D> groundHits = new List<RaycastHit2D>();
    private List<RaycastHit2D> wallHits = new List<RaycastHit2D>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw(PAP.axisXinput);
        animator.SetFloat(PAP.moveX, moveX);

        bool isMoving = !Mathf.Approximately(moveX, 0f);
        animator.SetBool(PAP.isMoving, isMoving);

        // Check & Trigger for On Ground
        bool lastOnGround = animator.GetBool(PAP.isOnGround);
        bool newOnGround = CheckIfOnGround();
        animator.SetBool(PAP.isOnGround, newOnGround);

        if (lastOnGround == false && newOnGround == true)
        {
            animator.SetTrigger(PAP.landedOnGround);
        }

        // Check & Trigger for On Wall
        bool onWall = CheckIfOnWall();
        animator.SetBool(PAP.isOnWall, onWall);

        // Jump
        bool isJumpKeyPressed = Input.GetButtonDown(PAP.jumpKeyName);
        if (isJumpKeyPressed)
        {
            animator.SetTrigger(PAP.JumpTriggerName);
        }
        else
        {
            animator.ResetTrigger(PAP.JumpTriggerName);
        }
    }

    void FixedUpdate()
    {
        float forceX = animator.GetFloat(PAP.forceX);
        float impulseY = animator.GetFloat(PAP.impulseY);
        float impulseX = animator.GetFloat(PAP.impulseX);

        // Horizontal movement
        rb.velocity = new Vector2(forceX, rb.velocity.y);

        // Jump / Impulse
        if (impulseY != 0 || impulseX != 0)
        {
            float xDirectionSign = Mathf.Sign(transform.localScale.x);
            Vector2 impulseVector = new Vector2(xDirectionSign * impulseX, impulseY);
            rb.AddForce(impulseVector, ForceMode2D.Impulse);
            animator.SetFloat(PAP.impulseY, 0f);
            animator.SetFloat(PAP.impulseX, 0f);
        }

        // Stop velocity
        bool isStopVelocity = animator.GetBool(PAP.stopVelocity);
        if (isStopVelocity)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool(PAP.stopVelocity, false);
        }

        animator.SetFloat(PAP.velocityY, rb.velocity.y);
    }

    bool CheckIfOnGround()
    {
        collider2d.Cast(Vector2.down, groundCheckFilter, groundHits, groundCheckDistance);
        return groundHits.Count > 0;
    }

    bool CheckIfOnWall()
    {
        Vector2 localScale = transform.localScale;
        collider2d.Raycast(Mathf.Sign(localScale.x) * Vector2.right, groundCheckFilter, wallHits, wallRaycastDistance);
        return wallHits.Count > 0;
    }
}
