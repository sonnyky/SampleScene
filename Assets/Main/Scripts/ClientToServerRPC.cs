using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class ClientToServerRPC : NetworkBehaviour
{
    private Vector2 _moveInput;
    private Animator _animator;
    private bool _hasAnimator;
    private float _animationBlend;
    private int _animIDSpeed;

    private PlayerControls _controller;

    private int state;

    // Start is called before the first frame update
    void Start()
    {
        state = 0; // idle
        _controller = GetComponent<PlayerControls>();
        _animIDSpeed = Animator.StringToHash("Speed");
        _moveInput = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        _hasAnimator = TryGetComponent(out _animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // �ڑ����ɐ������ꂽPlayerPrefabs�̃I�u�W�F�N�g�́A�ڑ������N���C�A���g���I�[�i�[�����������Ă��܂�
        // IsOwner���菈���������Ȃ��ƁA���v���C���[�̃I�u�W�F�N�g�����삵�Ă��܂����ƂɂȂ�܂�
        if (IsOwner && IsClient)
        {
            // �N���C�A���g���̓v���C���[�̃L�[���͂��T�[�o�[���ɂ��`����
            SetMoveInputServerRPc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetMoveInputServerRPc(float x, float y, ServerRpcParams serverRpcParams = default)
    {
        _moveInput.x = x;
        _moveInput.y = y;
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            if (x != 0 || y != 0)
            {
                
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") && state == 0)
                {
                    _animator.ResetTrigger("Idle");
                    _animator.SetTrigger("Walk");
                    Debug.Log("Walking animation: " + clientId);
                    state = 1;
                }
            }
            else
            {
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && state == 1)
                {
                    _animator.ResetTrigger("Walk");
                    _animator.SetTrigger("Idle");
                    Debug.Log("Idle animation: " + clientId);
                    state = 0;
                }
            }
          
            _controller.SetMoveDirection(_moveInput);
        }
    }
 }
