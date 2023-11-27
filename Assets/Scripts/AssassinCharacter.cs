using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinCharacter : Character
{
    protected override void Update()
    {
        base.Update();

        {   // Handle Die
            bool doDie = false;
            if (Input.GetKeyDown(KeyCode.X))
                doDie = true;
            if (doDie)
                m_anim.SetTrigger("Die");
        }

        {   // Handle Special
            bool doSpecial = false;
            if (Input.GetKeyDown(KeyCode.Tab))
                doSpecial = true;
            if (doSpecial)
                m_anim.SetTrigger("Special");
        }

        {   // Handle Wound
            bool doSpecial = false;
            if (Input.GetKeyDown(KeyCode.Z))
                doSpecial = true;
            if (doSpecial)
                m_anim.SetTrigger("Wound");
        }

        {
            bool swordToggle = false;
            if (Input.GetKeyDown(KeyCode.Backspace))
                swordToggle = true;
            if (swordToggle)
            {
                bool swordOut = m_anim.GetBool("IsSwordOut");
                m_anim.SetBool("IsSwordOut", !swordOut);
            }
        }
    }
}
