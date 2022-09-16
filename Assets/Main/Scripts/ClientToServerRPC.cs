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
        // �ڑ����ɐ������ꂽPlayerPrefabs�̃I�u�W�F�N�g�́A�ڑ������N���C�A���g���I�[�i�[�����������Ă��܂�
        // IsOwner���菈���������Ȃ��ƁA���v���C���[�̃I�u�W�F�N�g�����삵�Ă��܂����ƂɂȂ�܂�
        if (IsOwner)
        {
            // �N���C�A���g���̓v���C���[�̃L�[���͂��T�[�o�[���ɂ��`����
            SetMoveInputServerRPc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        if (IsServer)
        {
            // �T�[�o�[���͈ړ����������s
            Move();
        }
        Debug.Log(_moveInput);
    }

    [Unity.Netcode.ServerRpc]
    private void SetMoveInputServerRPc(float x, float y)
    {
        _moveInput.x = x;
        _moveInput.y = y;
        // ��������l�́A�T�[�o�[���̃I�u�W�F�N�g�ɃZ�b�g�����
        _inputs.move = _moveInput;
    }

    private void Move()
    {
        // ServerRpc�ɂ���ăN���C�A���g������ύX����Ă���_moveInput
        var moveVector = new Vector3(_moveInput.x, 0, _moveInput.y);
        // �Ȍ�ARigidbody��Transform��ύX����ƁA�T�[�o�[�ɐڑ����Ă���S�ẴN���C�A���g�Ŕ��f�����

    }
 }
