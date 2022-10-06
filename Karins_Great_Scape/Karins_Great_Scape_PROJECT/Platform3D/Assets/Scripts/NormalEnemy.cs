using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    public Transform point0, point1;
    public Animator anim;
    public LayerMask groundMask;

    bool going, isDying, isGrounded, isJumping;
    RaycastHit hitWalls;
    Rigidbody control;

    void Start()
    {
        control = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDying)
        {
            Movement();
            Jump();
        }
    }

    void Movement()
    {
        if (!going)
        {
            Vector3 endPos = point0.position;
            endPos.y = transform.position.y;
            transform.LookAt(endPos);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 1.5f * Time.deltaTime);
            if (transform.position == endPos) going = true;
        }
        else
        {
            Vector3 endPos = point1.position;
            endPos.y = transform.position.y;
            transform.LookAt(endPos);
            transform.position = Vector3.MoveTowards(transform.position, endPos, 1.5f * Time.deltaTime);
            if (transform.position == endPos) going = false;
        }
    }

    void Jump()
    {
        isGrounded = Physics.OverlapSphere(transform.position, 0.2f, groundMask).Length > 0;
        Vector3 rayPos = transform.position;
        rayPos.y += 0.1f;

        if (!isJumping && Physics.Raycast(rayPos, transform.forward, out hitWalls, 1, groundMask))
        {
            control.velocity = new Vector3(control.velocity.x, 0, control.velocity.z);
            control.AddForce(Vector3.up * 9, ForceMode.Impulse);
            isJumping = true;
        }

        if (isJumping && control.velocity.y < 0) isJumping = false;
    }

    public void GetDamage()
    {
        anim.SetTrigger("Death");
        isDying = true;
    }
}
