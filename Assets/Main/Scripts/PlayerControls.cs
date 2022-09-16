using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private Vector2 _input_move;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    private float _rotationVelocity;

    private float _targetRotation = 0.0f;

    private GameObject _mainCamera;

    private CharacterController _controller;

    private float targetSpeed = 0.0f;

    private Animator _animator;
    private bool _hasAnimator;
    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input_move = Vector2.zero;
        _hasAnimator = TryGetComponent(out _animator);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_input_move == Vector2.zero)
        {
            targetSpeed = 0.0f;
     
        }
      
        // normalise input direction
        Vector3 inputDirection = new Vector3(_input_move.x, 0.0f, _input_move.y).normalized;
        if (_input_move != Vector2.zero)
        {
            targetSpeed = 2.0f;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move the player
        _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) +
                         new Vector3(0.0f, 0.0f, 0.0f) * Time.deltaTime);



    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
       
    }

    public void SetMoveDirection(Vector2 dir)
    {
        _input_move = dir;
    }

    private void SetWalkAnimation()
    {
        _animator.ResetTrigger("Idle");
        _animator.SetTrigger("Walk");
    }

    private void SetIdleAnimation()
    {
        _animator.ResetTrigger("Walk");
        _animator.SetTrigger("Idle");
    }
}
