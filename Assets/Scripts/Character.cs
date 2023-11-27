using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public float m_walkSpeed = 2.0f;
    public float m_turnSpeed = 360.0f;  // degrees per second

    public class CharInput
    {
        public Vector3 m_move;
        public float m_facingAngle;
        public bool m_attack;
        public bool m_jump;
    }

    protected CharacterController m_char;
    protected bool m_canJump = true;
    protected CharInput m_input;
    protected Animator m_anim;
    protected PlayerInput m_playerInput;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_char = GetComponent<CharacterController>();
        m_input = new CharInput();
        m_anim = GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Vector3 forward = Camera.main.transform.forward.normalized;
        Vector3 right = Camera.main.transform.right.normalized;

        // player faces the same direction as camera
        m_input.m_facingAngle = transform.localEulerAngles.y;

        Vector3 move = Vector3.zero;
        {   // read input from the PlayerInput
            if (null != m_playerInput)
            {
                var moveAction = m_playerInput.actions.FindAction("Move");
                if (null != moveAction)
                {
                    Vector2 move2D = moveAction.ReadValue<Vector2>();
                    move.x = move2D.x;
                    move.z = move2D.y;
                }
            }
        }

        {   // Convert move command from camera-space into world-space
            m_input.m_move = move.x * right + move.z * forward;
        }
        {   // set the facing input to the direction of the input
            if (m_input.m_move.sqrMagnitude > 0.001f)
                m_input.m_facingAngle = Mathf.Rad2Deg * Mathf.Atan2(m_input.m_move.x, m_input.m_move.z);
        }

        {   // Space bar for attack
            if (Input.GetKeyDown(KeyCode.Space))
                OnAttackButton();
        }

        {   // Return for jump
            if (Input.GetKeyDown(KeyCode.Return))
                OnJumpButton();
        }


        {   // Move the character based on the input
            m_char.SimpleMove(m_walkSpeed * m_input.m_move);
        }

        {   // Rotate the character based on the input
            Vector3 ang = transform.localEulerAngles;
            float angDiff = m_input.m_facingAngle - ang.y;
            if (angDiff > 180.0f)
                angDiff -= 360.0f;
            if (angDiff <= -180.0f)
                angDiff += 360.0f;
            float turnSpeed = Mathf.Clamp(angDiff, -m_turnSpeed * Time.deltaTime, m_turnSpeed * Time.deltaTime);
#if false
            {   // adjust the turnSpeed based on movement rate
                float throttle = m_input.m_move.magnitude;
                turnSpeed *= throttle;
            }
#endif
            ang.y += turnSpeed;
            transform.localEulerAngles = ang;
        }

        if (null != m_anim)
        {   // animate the character
            Vector3 animMove = transform.InverseTransformVector(m_input.m_move);
            m_anim.SetFloat("FwdBack", animMove.z);
            m_anim.SetFloat("RightLeft", animMove.x);
            m_anim.SetBool("IsWalking", animMove.z > 0.01f);
            m_anim.SetBool("IsRunning", animMove.z > 0.5f);
            if (m_input.m_attack)
                m_anim.SetTrigger("Attack");
            if (m_input.m_jump && m_canJump)
                m_anim.SetTrigger("DoJump");
        }

        // clear the attack input
        m_input.m_attack = false;
        m_input.m_jump = false;
    }

    public void OnAttackButton()
    {
        m_input.m_attack = true;
    }

    public void OnJumpButton()
    {
        m_input.m_jump = true;
    }
}
