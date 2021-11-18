using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : TestAnimation
{
    // Update is called once per frame
    protected override void Update()
    {
        {   // Handle Die
            bool doDie = false;
            if (Input.GetKeyDown(KeyCode.X))
                doDie = true;
            if (doDie)
                m_Animator.SetTrigger("Die");
        }

        {   // Handle Special
            bool doSpecial = false;
            if (Input.GetKeyDown(KeyCode.Tab))
                doSpecial = true;
            if (doSpecial)
                m_Animator.SetTrigger("Special");
        }

        {   // Handle Wound
            bool doSpecial = false;
            if (Input.GetKeyDown(KeyCode.Z))
                doSpecial = true;
            if (doSpecial)
                m_Animator.SetTrigger("Wound");
        }

        {
            bool swordToggle = false;
            if (Input.GetKeyDown(KeyCode.Backspace))
                swordToggle = true;
            if (swordToggle)
            {
                bool swordOut = m_Animator.GetBool("IsSwordOut");
                m_Animator.SetBool("IsSwordOut", !swordOut);
            }
        }

        base.Update();
    }
}
