using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    public float turnSpeed = 20f;

    protected Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        {
            bool doJump = false;
            if (Input.GetKeyDown(KeyCode.Space))
                doJump = true;
            if (doJump)
                m_Animator.SetTrigger("DoJump");
        }

        {
            bool doAttack = false;
            if (Input.GetKeyDown(KeyCode.Return))
                doAttack = true;
            if (doAttack)
                m_Animator.SetTrigger("Attack");
        }
    }

    protected virtual void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isRunning = false;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            isRunning = true;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        m_Animator.SetBool("IsRunning", isRunning);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(desiredForward);
    }
}