using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawnTarget : MonoBehaviour
{
    
    private int TargetPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Spawner")
        {
            TargetPos = Random.Range(-400, 400);
            gameObject.transform.localPosition = new Vector3(TargetPos, 0, 0);
        }
    }
}
