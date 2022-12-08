using System;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace ThirdPersonShooter.Entities.Player
{
	public class CameraMotor : MonoBehaviour
	{
		private InputAction LookAction => lookActionReference.action;
		
		[SerializeField, Range(0, 1)] private float sensitivity = 1.0f;

		[Header("Components")] 
		[SerializeField] private InputActionReference lookActionReference;
		[SerializeField] private new Camera camera;
		[SerializeField, Range(0, 1)] private float alignmentDamping = 1f;

		[Header("Collision")] 
		[SerializeField] private float collisionRadius = 0.5f;
		[SerializeField] private float distance = 3f;
		[SerializeField] private Transform render;
		[SerializeField] private LayerMask collisionLayers;
		
		private Vector3 dampingVelocity;

		private void OnValidate()
		{
			camera.transform.localPosition = Vector3.back * distance;
		}

		private void OnDrawGizmosSelected()
		{
			Matrix4x4 defaultMat = Gizmos.matrix;

			Gizmos.matrix = camera.transform.localToWorldMatrix;
			Gizmos.color = new Color(0, 0.8f, 0, 0.8f);
			Gizmos.DrawWireSphere(Vector3.zero, collisionRadius);

			Gizmos.matrix = defaultMat;
		}

		private void OnEnable()
		{
			camera.transform.localPosition = Vector3.back * distance;
			LookAction.performed += OnLookPerformed;

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}

		private void OnDisable()
		{
			camera.transform.localPosition = Vector3.back * distance;
			LookAction.performed -= OnLookPerformed;

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		private void FixedUpdate()
		{
			CameraCollision();
		}

		private void CameraCollision()
		{
			if(Physics.Raycast(transform.position, -camera.transform.forward, out RaycastHit hit, distance, collisionLayers))
			{
				camera.transform.position = hit.point + camera.transform.forward * collisionRadius;
			}
			else
			{
				camera.transform.localPosition = Vector3.back * distance;
			}
		}

		private void OnLookPerformed(InputAction.CallbackContext _context)
		{
			if(!GameManager.IsValid())
			{
				transform.Rotate(Vector3.up, _context.ReadValue<float>() * sensitivity);
				render.transform.rotation = transform.rotation;
				return;
			}
			
			if(!GameManager.Instance.IsPaused)
				transform.Rotate(Vector3.up, _context.ReadValue<float>() * sensitivity);
			render.forward = Vector3.SmoothDamp(render.forward, transform.forward, ref dampingVelocity, alignmentDamping);
			render.transform.rotation = transform.rotation;
		}

	}
}