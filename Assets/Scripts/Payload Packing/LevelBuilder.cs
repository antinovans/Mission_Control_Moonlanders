using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private List<GameObject> collectorsPrefab;
    [SerializeField]
    private List<GameObject> payloadsPrefab;
    [SerializeField]
    private float interval;


    [Header("wall attribute")]
    public float wallThickness;
    public float wallHeight;

    public static LevelBuilder instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BuildWalls();
        BuildCollectors();
        StartCoroutine(InstantiatePayloads());
    }

    void BuildWalls()
    {
        //build left and right wall
        float leftX = PaddleManager.instance.paddlePositions[0].x -
            PaddleManager.instance.paddleScale /2 - wallThickness;
        /*BuildWallFromTwoPos(new Vector2(leftX, wallHeight), new Vector2(leftX, -wallHeight));*/
        float rightX = PaddleManager.instance.paddlePositions[PaddleManager.instance.paddlePositions.Count - 1].x +
            PaddleManager.instance.paddleScale /2 + wallThickness;
        /*BuildWallFromTwoPos(new Vector2(rightX, wallHeight), new Vector2(rightX, -wallHeight));*/
        //building middle walls
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count - 1; i++)
        {
            float mid = (PaddleManager.instance.paddlePositions[i].x 
            + PaddleManager.instance.paddlePositions[i + 1].x) / 2;
           /* BuildWallFromTwoPos(new Vector2(mid, PaddleManager.instance.paddlePositions[i].y - 0.5f), 
            new Vector2(mid, -wallHeight + PaddleManager.instance.paddlePositions[i].y));*/
        }
    }
    private void BuildCollectors()
    {
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count; i++)
        {
            // Debug.Log("bruh");
            Vector2 pos = new Vector2(PaddleManager.instance.paddlePositions[i].x, -wallHeight/2);
            GameObject collector = Instantiate(collectorsPrefab[i % collectorsPrefab.Count], pos, Quaternion.identity);
            /*PayloadCollector pCollector = collector.GetComponent<PayloadCollector>();*/
            /* pCollector?.SetScale(PaddleManager.instance.paddleScale * 0.75f);*/
        }
    }
    IEnumerator InstantiatePayloads()
    {
        float xMin = PaddleManager.instance.paddlePositions[0].x;
        float xMax = PaddleManager.instance.paddlePositions[PaddleManager.instance.paddlePositions.Count - 1].x;
        while(true)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(xMin, xMax), wallHeight, 0);
            Vector2 forceDir = new Vector2(UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 0f)).normalized;
            GameObject payload = Instantiate(payloadsPrefab[UnityEngine.Random.Range(0, payloadsPrefab.Count)],
            pos, Quaternion.identity);
            payload.GetComponent<Payload>().SetSpriteColor(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
            payload.GetComponent<Rigidbody2D>().mass = 0.2f;
            payload.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
            yield return new WaitForSeconds(interval);
        }
    }

    void BuildWallFromTwoPos(Vector2 A, Vector2 B)
    {
        //two points should either align to the same horizontal line
        //or the same vertical line

        //same vertical line
        if(Mathf.Abs(A.x - B.x) < 0.1f)
        {
            float height = Mathf.Abs(A.y - B.y);
            Vector2 midPoint = (A + B)/2;
            Vector3 newScale = new Vector3(wallPrefab.transform.localScale.x * wallThickness, 
            wallPrefab.transform.localScale.y * height, 1);
            GameObject wall = Instantiate(wallPrefab, midPoint, Quaternion.identity, transform);
            wall.transform.localScale = newScale;
            return;
        }
        if(Mathf.Abs(A.y - B.y) < 0.1f)
        {
            float width = Mathf.Abs(A.x - B.x);
            Vector2 midPoint = (A + B)/2;
            Vector3 newScale = new Vector3(wallPrefab.transform.localScale.x * width, 
            wallPrefab.transform.localScale.y * wallThickness, 1);
            GameObject wall = Instantiate(wallPrefab, midPoint, Quaternion.identity, transform);
            wall.transform.localScale = newScale;
            return;
        }
    }
    public void StopCurrentLevel()
    {
        StopCoroutine(InstantiatePayloads());
    }

}
