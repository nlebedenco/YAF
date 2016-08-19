using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoPlayAnimations : MonoBehaviour 
{
    public float delay = 3.0f;
    public bool loop = false;
    private float elapsed = 0.0f;
    private int lastPlayedIdx = 0;

    private Animator animator;
    private List<string> animations;

    #region MonoBehaviour

    void Awake()
    {
        animator = GetComponent<Animator>();
        animations = new List<string>();
    }

    void Start()
    {
        for (int i = 0; i < animator.parameterCount; i++)
        {
            AnimatorControllerParameter animCtrlParam = animator.GetParameter(i);
            if (animCtrlParam.type == AnimatorControllerParameterType.Trigger)
                animations.Add(animCtrlParam.name);
        }
        animator.SetBool("AutoReset", loop);
    }

    void Update()
    {
        if ((animations.Count <= 0) || (lastPlayedIdx == -1) || !animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        if (elapsed < delay)
        {
            elapsed += Time.unscaledDeltaTime;
        }
        else
        {
            animator.SetTrigger(animations[lastPlayedIdx]);
            if (++lastPlayedIdx >= animations.Count)
            {
                lastPlayedIdx = (loop) ? 0 : -1;
            }
            elapsed = 0;
        }
    }

    void OnEnable() {
        elapsed = 0.0f;
        lastPlayedIdx = 0;
    }

    #endregion
}
