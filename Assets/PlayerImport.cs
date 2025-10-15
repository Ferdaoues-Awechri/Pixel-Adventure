using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerImport : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw(PAP.axisXinput);

        animator.SetFloat(PAP.moveX, moveX);
        bool isMoving = !Mathf.Approximately(moveX, 0f);
        animator.SetBool(PAP.isMoving, isMoving);

    }

  void FixedUpdate()
{
    float forceX = animator.GetFloat(PAP.forceX);
    float impulseY = animator.GetFloat(PAP.impulseY);

    // Smooth constant velocity (no infinite acceleration)
    rb.velocity = new Vector2(forceX, rb.velocity.y);

    // Jump (if implemented later)
    if (impulseY != 0)
    {
        rb.AddForce(new Vector2(0f, impulseY), ForceMode2D.Impulse);
        animator.SetFloat(PAP.impulseY, 0f);
    }
}


}
