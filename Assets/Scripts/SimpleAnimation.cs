using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimation : MonoBehaviour
{
    public float m_TurnSpeed = 20f;
    public string m_AnimName = "";
    public GameObject m_ButtonPrototype;

    string m_CurrentAnim = "";
    Animator m_Animator;
    AnimationClip[] m_Clips;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        if (null == m_Animator)
        {
            Debug.LogError("You must have an Animator on your object");
            Destroy(gameObject);
            return;
        }
        m_Clips = m_Animator.runtimeAnimatorController.animationClips;
        Canvas canvas = FindObjectOfType<Canvas>();
        if (null == canvas)
        {
            Debug.LogError("You need to add a Canvas and an EventSystem to your scene");
        }
        else
        {
            if (null == m_ButtonPrototype)
            {
                m_ButtonPrototype = Resources.Load<GameObject>("AnimationButton");
            }
            float yOffset = 0.0f;
            foreach (var clip in m_Clips)
            {
                GameObject uiButton = Instantiate<GameObject>(m_ButtonPrototype, canvas.transform);
                uiButton.name = clip.name;
                uiButton.GetComponentInChildren<Text>().text = clip.name;
                Vector3 pos = uiButton.transform.localPosition;
                pos.y += yOffset;
                uiButton.transform.localPosition = pos;
                yOffset -= 32.0f;
                Button button = uiButton.GetComponent<Button>();
                button.onClick.AddListener(delegate { OnAnimButton(clip.name); });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_AnimName != m_CurrentAnim)
        {   // the user wants to change the animation
            m_Animator.Play(m_AnimName);
            m_CurrentAnim = m_AnimName;
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement.Normalize();

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, m_TurnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimButton(string name)
    {
        m_AnimName = name;
    }
}
