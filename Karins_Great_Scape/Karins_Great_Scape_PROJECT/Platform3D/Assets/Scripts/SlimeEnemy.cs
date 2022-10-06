using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    Rigidbody control;
    Vector3 moveDir;
    float speed = 1;
    int currentPathPoint;
    float animTimer;
    float animValue;
    bool animState;

    public List<Transform> pathPoints;
    public bool isJumping, haveHelmet;
    public Animator anim;

    void Start()
    {
        control = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Animation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        Vector3 finalPos = pathPoints[currentPathPoint].position;
        finalPos.y = transform.position.y;
        transform.LookAt(finalPos);

        moveDir = new Vector3(0, moveDir.y, speed);
        moveDir = transform.TransformDirection(moveDir);
        control.MovePosition(control.position + moveDir * Time.deltaTime);

        float dist = Vector3.Distance(transform.position, finalPos);
        if (dist <= 1)
        {
            currentPathPoint++;
            if (currentPathPoint >= pathPoints.Count) currentPathPoint = 0;
        }
    }

    void Animation()
    {
        if (!animState) animValue += Time.deltaTime;
        else animValue -= Time.deltaTime;
        anim.SetFloat("FlyingSlimeAnimation", animValue);
        if (animValue >= 1.5f) animState = true;
        if(animValue <= 1f) animState = false;

    }
}
