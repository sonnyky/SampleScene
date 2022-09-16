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
        // 接続時に生成されたPlayerPrefabsのオブジェクトは、接続したクライアントがオーナー属性を持っています
        // IsOwner判定処理を加えないと、他プレイヤーのオブジェクトも操作してしまうことになります
        if (IsOwner && IsClient)
        {
            // クライアント側はプレイヤーのキー入力をサーバー側にも伝える
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
