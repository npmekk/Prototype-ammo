using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	public float sensitivity = 1f;
	private float rotationX;
	private float rotationY;

	public bool intro;
	private Animator anim;

	private void Awake()
	{
		GameManager.cam = transform;
		anim = transform.parent.GetComponent<Animator>();
	}

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			anim.SetTrigger("Intro");
		}
		
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
		{
			return;
		}

		if (target)
		{
			transform.position = target.position + offset;
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			rotationX = 0;
			rotationY = 0;
		}

		rotationX += sensitivity * Input.GetAxis("Mouse X");
		rotationY -= sensitivity * Input.GetAxis("Mouse Y");
		rotationY = Mathf.Clamp(rotationY, -90f, 90f);
		transform.eulerAngles = new Vector3(rotationY, rotationX, 0);
	}
}
