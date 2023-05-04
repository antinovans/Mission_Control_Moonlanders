using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePath : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawnTarget;
    [SerializeField]
    private GameObject PipSpawner;
    [SerializeField]
    private GameObject Pip;

    private Rigidbody2D SpawnRB;

    [SerializeField]
    private float spawnerSpeedMax;
    [SerializeField]
    private float pipSpeed;
    [SerializeField]
    private float pipFrequency;
    [SerializeField]
    private float travelForce;



    private float timer;


    void Awake()
    {
        SpawnTarget.transform.localPosition = new Vector2(Random.Range(-400, 400), 0);
        SpawnRB = PipSpawner.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

    //===============================>> Timer to spawn Pip

        timer += Time.deltaTime;
        if (timer >= pipFrequency)
        {
            SpawnPip();
            timer = 0;
        }
    //===============================>> Apply Forces to Move Spawner

        SpawnRB.AddForce(Vector3.Normalize(SpawnTarget.transform.position - PipSpawner.transform.position) * travelForce);

    //===============================>> Limit Spawner Speed

        if (SpawnRB.velocity.magnitude > spawnerSpeedMax)
        {
            SpawnRB.velocity = SpawnRB.velocity.normalized * spawnerSpeedMax;
        }
    }


    private void SpawnPip()
    {
       GameObject pipInstance =  Instantiate(Pip, PipSpawner.transform.position, Quaternion.identity);
       pipInstance.GetComponent<Rigidbody2D>().velocity = Vector2.down * pipSpeed;
        
    }
}
