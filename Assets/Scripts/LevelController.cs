using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	public float maxSize = 3f;
	public float minSize = 3f;
	public Transform[] points;
	public GameObject cubePrefab;

	private List<GameObject> allCubes = new List<GameObject>();

	private void Start()
	{
		GenerateLevel();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			foreach (var cube in allCubes)
			{
				Destroy(cube);
			}

			GenerateLevel();
		}
	}

	private void GenerateLevel()
	{
		if (points.Length < 2)
		{
			Debug.LogWarning("Add more points for level generation!");
		}

		var lastPos = Vector3.zero;
		var segmentDirection = Vector3.zero;
		var segmentLength = 0f;

		for (int i = 0; i < points.Length - 1; i++)
		{
			Refresh(i);

			while (segmentLength > minSize)
			{
				SpawnCube(i);
			}

			if (segmentLength > 0)
			{
				SpawnCube(i);
			}
		}

		void SpawnCube(int point)
		{
			var cubeSize = Random.Range(Mathf.Min(minSize, segmentLength), Mathf.Min(maxSize, segmentLength));
			var cubePos = lastPos + (segmentDirection * cubeSize / 2f);
			var cubeRot = Random.rotation;
			var cube = Instantiate(cubePrefab, cubePos, cubeRot, transform);

			allCubes.Add(cube);
			cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
			lastPos = cubePos + (segmentDirection * cubeSize / 2f);
			segmentLength = (points[point + 1].position - lastPos).magnitude;
		}

		void Refresh(int point)
		{
			lastPos = points[point].position;
			var segmentDirectionRaw = points[point + 1].position - points[point].position;
			segmentDirection = segmentDirectionRaw.normalized;
			segmentLength = segmentDirectionRaw.magnitude;
		}
	}
}
