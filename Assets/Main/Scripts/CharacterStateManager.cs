using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterStateManager : NetworkBehaviour
{
    // character state: 0: idle, 1: walk, 2: run, 3: jump start
    // 4: jump (in the air), 5: land normally, 6: land walking, 7: land running
    private int currentState = 0;

    private Animator _animator;
    private bool _hasAnimator;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private bool _grounded;

    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = TryGetComponent(out _animator);
        AssignAnimationIDs();
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);
    }

    public void IdleWalkRun(int animIDSpeed, float animationBlend, int animIDMotionSpeed, float mag)
    {
        // check what the parameters mean, ie. which animation should run
        // animatonBlend = 0 is idle, state = 0
        // 2 <= animationBlend <3  is walking, state = 1 
        // animationBlend >= 5 is running, state = 2

        int targetState = 0;
        if (animationBlend == 0f) targetState = 0;
        if (animationBlend >= 0.2f && animationBlend < 5f) targetState = 1;
        if (animationBlend >= 5f) targetState = 2;
        // check if current state is already the same as tha target state
        // if not then set the currenState to the new targetState and run animations accordingly
        if (currentState != targetState)
        {
            Debug.Log("animationBlend: " + animationBlend + " and target state is : " + targetState + " while currentState: " + currentState);

            currentState = targetState;
            switch (currentState)
            {
                case 0:
                    _animator.SetBool("Idle", true);
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("Run", false);
                    break;
                case 1:
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("Walk", true);
                    _animator.SetBool("Run", false);
                    break;
                case 2:
                    _animator.SetBool("Idle", false);
                    _animator.SetBool("Walk", false);
                    _animator.SetBool("Run", true);
                    break;
            }

        }
        else { return; } // we ignore consecutive requests to set the target state. This is necessary since this method is called in Update
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    public void GroundedCheck(bool isGrounded)
    {
        _grounded = isGrounded;
        _animator.SetBool("Grounded", isGrounded);
    }

    // TODO : do we really need this?
    private void ResetAllAnimationFlags()
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Walk", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Grounded", false);
        _animator.SetBool("FreeFall", false);
    }
}
