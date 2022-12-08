using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonShooter.Entities.Player
{
	public class PlayerMotor : MonoBehaviour
	{
		private static readonly int speedID = Animator.StringToHash("Speed");

		private InputAction MoveAction => moveActionReference.action;
		
		[SerializeField] private InputActionReference moveActionReference;
		
		[Header("Components")]
		[SerializeField] private new Transform camera;
		[SerializeField] private Animator animator;

		private new Rigidbody rigidbody;
		private PlayerEntity player;
		private Vector2 input;

		private void OnEnable()
		{
			rigidbody = gameObject.GetComponentInParent<Rigidbody>();
			player = gameObject.GetComponentInParent<PlayerEntity>();

			MoveAction.performed += OnMovePerformed;
			MoveAction.canceled += OnMoveCancelled;
		}

		private void OnDisable()
		{
			MoveAction.performed += OnMovePerformed;
			MoveAction.canceled += OnMoveCancelled;
		}

		private void FixedUpdate()
		{
			// Movement Handling
			rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, player.Stats.Speed);
			
			if(input.normalized.magnitude != 0)
				rigidbody.AddForce((camera.forward * input.y + camera.right * input.x) * player.Stats.Speed, ForceMode.Impulse);
			
			// Rendering Handling
			if(animator)
				animator.SetFloat(speedID, Mathf.Clamp01(rigidbody.velocity.magnitude));
		}

		private void OnMovePerformed(InputAction.CallbackContext _context)
		{
			input = _context.ReadValue<Vector2>();
		}

		private void OnMoveCancelled(InputAction.CallbackContext _context)
		{
			input = Vector2.zero;
		}
	}
}