using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public Transform arm;
	public LineRenderer web;
	public float walkSpeed = 1f;
	public float jumpHeight = 1f;
	public float jumpGravity = 1f;

	private Rigidbody rb;
	private Vector3 endPoint;
	private Vector3 spawnPoint;
	private ConfigurableJoint joint;

	private int grounded; // 0 - unset, 1 - grounded, 2 - in air
	private float gravity;
	private float defaultDrag;

	private void Awake()
	{
		spawnPoint = transform.position;
		rb = GetComponent<Rigidbody>();
	}
	
	private void Start()
	{
		web.positionCount = 0;
		defaultDrag = rb.drag;
		SetGrounded(2);
	}

	private void Update()
    {
		var transPos = transform.position;
		if (Input.GetButtonDown("Fire1"))
		{
			RaycastHit hit;
			int layerMask = 1 << 9;
			if (Physics.Raycast(transPos, GameManager.cam.forward + new Vector3(0, 0.05f, 0), out hit, Mathf.Infinity, layerMask))
			{
				endPoint = hit.point - (GameManager.cam.forward.normalized * 0.05f);
				ToggleJoint(true);

				var connectedRigidbody = hit.collider.GetComponent<Rigidbody>();
				if (connectedRigidbody)
				{
					joint.connectedBody = connectedRigidbody;
					joint.connectedAnchor = connectedRigidbody.transform.InverseTransformPoint(endPoint);
				}
			}
		}

		if (endPoint != Vector3.zero)
		{
			if (Input.GetButton("Fire1"))
			{
				SetGrounded(2);
				web.SetPosition(0, arm.position);
				web.SetPosition(1, endPoint);
			}
			else
			{
				endPoint = Vector3.zero;
				ToggleJoint(false);
			}
		}

		if (Input.GetButtonDown("Jump") && grounded == 1)
		{
			rb.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.VelocityChange);
		}

		if (transPos.y < -50f && endPoint == Vector3.zero)
		{
			Respawn();
			GameManager.gm.ResetScore();
		}
	}

	private void FixedUpdate()
    {
		rb.AddForce(Vector3.down * gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);

		var rotation = GameManager.cam.rotation.eulerAngles;
		var forward = Quaternion.Euler(0, rotation.y, rotation.z);
		var direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		if (direction != Vector3.zero && grounded == 1)
		{
			rb.AddForce(forward * direction * walkSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
		} 
	}

	private void SetGrounded(int state)
    {
		if (grounded == state)
		{
			return;
		}

		grounded = state;
		if (grounded == 1)
		{
			gravity = 1f;
			rb.drag = defaultDrag;
		}
		else
		{
			gravity = jumpGravity;
			rb.drag = .1f;
		}
	}

	private void Respawn()
    {
		rb.velocity = Vector3.zero;
		transform.position = spawnPoint;
		web.positionCount = 0;
		SetGrounded(2);
	}

	private void ToggleJoint(bool enable)
	{
		if (enable)
		{
			web.positionCount = 2;
			joint = gameObject.AddComponent<ConfigurableJoint>();
			joint.enableCollision = true;
			joint.connectedAnchor = Vector3.zero;
			joint.anchor = new Vector3(0, 0.5f, 0);
			joint.autoConfigureConnectedAnchor = false;
			joint.linearLimitSpring = new SoftJointLimitSpring { spring = 100f, damper = 100f };
			joint.linearLimit = new SoftJointLimit { limit = 5f, bounciness = 0, contactDistance = 0 };
			joint.xMotion = ConfigurableJointMotion.Limited;
			joint.yMotion = ConfigurableJointMotion.Limited;
			joint.zMotion = ConfigurableJointMotion.Limited;
		}
		else if (joint)
		{
			web.positionCount = 0;
			Destroy(joint);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Start"))
		{
			GameManager.gm.started = false;
			GameManager.gm.ResetScore();
		}
		else if (other.gameObject.CompareTag("Finish"))
		{
			GameManager.gm.started = false;
			GameManager.gm.Completed();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Start"))
		{
			GameManager.gm.started = true;
			GameManager.gm.ResetScore();
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.layer == 9)
		{
			SetGrounded(1);
		}
	}

	private void OnCollisionStay(Collision other)
	{
		if (other.gameObject.layer == 9)
		{
			SetGrounded(1);
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.layer == 9)
		{
			SetGrounded(2);
		}
	}
}
