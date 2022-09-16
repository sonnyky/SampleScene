using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using StarterAssets;

public class ClientToServerRPC : NetworkBehaviour
{
    private Vector2 _moveInput;
    private StarterAssetsInputs _inputs;
    // Start is called before the first frame update
    void Start()
    {
        _moveInput = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 接続時に生成されたPlayerPrefabsのオブジェクトは、接続したクライアントがオーナー属性を持っています
        // IsOwner判定処理を加えないと、他プレイヤーのオブジェクトも操作してしまうことになります
        if (IsOwner)
        {
            // クライアント側はプレイヤーのキー入力をサーバー側にも伝える
            SetMoveInputServerRPc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (IsServer)
        {
            // サーバー側は移動処理を実行
            Move();
        }
        Debug.Log(_moveInput);
    }

    [Unity.Netcode.ServerRpc]
    private void SetMoveInputServerRPc(float x, float y)
    {
        _moveInput.x = x;
        _moveInput.y = y;
        // 代入した値は、サーバー側のオブジェクトにセットされる
        _inputs.move = _moveInput;
    }

    private void Move()
    {
        // ServerRpcによってクライアント側から変更されている_moveInput
        var moveVector = new Vector3(_moveInput.x, 0, _moveInput.y);
        // 以後、RigidbodyやTransformを変更すると、サーバーに接続している全てのクライアントで反映される

    }
 }
