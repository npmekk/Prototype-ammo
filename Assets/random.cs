using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starto : MonoBehaviour
{
    public GameObject prefab;

    // Instantiate the Prefab somewhere between -10.0 and 10.0 on the x-z plane
    void Start()
    {
        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
        Instantiate(prefab, position, Quaternion.identity);
    }
}