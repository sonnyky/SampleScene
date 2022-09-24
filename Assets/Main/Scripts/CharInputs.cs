using UnityEngine;
using Unity.Netcode;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class CharInputs : NetworkBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
			SetMoveInputServerRpc(move, look, jump, sprint);
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
			SetMoveInputServerRpc(move, look, jump, sprint);
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
			SetMoveInputServerRpc(move, look, jump, sprint);
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
			SetMoveInputServerRpc(move, look, jump, sprint);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		/*
		 Add functionalities to talk to server
		 */
		[ServerRpc(RequireOwnership = false)]
		private void SetMoveInputServerRpc(Vector2 _move, Vector2 _look, bool _jump, bool _sprint, ServerRpcParams serverRpcParams = default)
		{
			//Debug.Log("Send to server");
			move = _move;
			look = _look;
			jump = _jump;
			sprint = _sprint;

		}
	}
}