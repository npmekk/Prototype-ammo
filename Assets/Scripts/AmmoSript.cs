using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSript : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            GameManager.ammoAmount += 5;
            Destroy(gameObject);
        }
    }
}
