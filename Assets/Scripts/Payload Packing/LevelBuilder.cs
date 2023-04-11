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
    public static int PIXEL_PER_UNIT = 32; 


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
        float wallWidth = (int)wallPrefab.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        float wallHeight = (int)wallPrefab.GetComponent<SpriteRenderer>().sprite.textureRect.height / (float)LevelBuilder.PIXEL_PER_UNIT;
        //build left and right wall
        float leftX = PaddleManager.instance.paddlePositions[0].x - 
            PaddleManager.instance.paddleWidth /2 - wallWidth/2;
        GameObject lWallUp =  Instantiate(wallPrefab, new Vector3(leftX, wallHeight/2, 1), Quaternion.identity);
        int _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(lWallUp.transform, _id);
        GameObject lWallDown =  Instantiate(wallPrefab, new Vector3(leftX, -wallHeight/2, 1), Quaternion.identity);
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(lWallDown.transform, _id);


        float rightX = PaddleManager.instance.paddlePositions[PaddleManager.instance.paddlePositions.Count - 1].x + 
            PaddleManager.instance.paddleWidth /2 + wallThickness/2;
        GameObject rWallUp = Instantiate(wallPrefab, new Vector3(rightX, wallHeight/2, 1), Quaternion.identity);
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(rWallUp.transform, _id);
        GameObject rWallDown = Instantiate(wallPrefab, new Vector3(rightX, -wallHeight/2, 1), Quaternion.identity);
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(rWallDown.transform, _id);


        //building middle walls
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count - 1; i++)
        {
            float mid = (PaddleManager.instance.paddlePositions[i].x 
            + PaddleManager.instance.paddlePositions[i + 1].x) / 2;
            Instantiate(wallPrefab, new Vector3(mid, -wallHeight/2, 1), Quaternion.identity);
        }
    }

    public Vector2 GetWallStats()
    {
        float wallWidth = (float)wallPrefab.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        float wallHeight = (float)wallPrefab.GetComponent<SpriteRenderer>().sprite.textureRect.height / (float)LevelBuilder.PIXEL_PER_UNIT;
        return new Vector2(wallWidth, wallHeight);
    }
    private void BuildCollectors()
    {
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count; i++)
        {
            // Debug.Log("bruh");
            Vector2 pos = new Vector2(PaddleManager.instance.paddlePositions[i].x, -wallHeight/2);
            GameObject collector = Instantiate(collectorsPrefab[i % collectorsPrefab.Count], pos, Quaternion.identity);
            PayloadCollector pCollector = collector.GetComponent<PayloadCollector>();
            pCollector?.SetScale(PaddleManager.instance.paddleWidth * 0.5f);
            pCollector?.SetSpriteColor(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 0.2f, 0.4f));
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
            // payload.GetComponent<Payload>().SetSpriteColor(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
            payload.GetComponent<Rigidbody2D>().mass = 0.2f;
            payload.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
            yield return new WaitForSeconds(interval);
        }
    }

    // GameObject BuildWallFromTwoPos(Vector2 A, Vector2 B)
    // {
    //     //two points should either align to the same horizontal line
    //     //or the same vertical line

    //     //same vertical line
    //     if(Mathf.Abs(A.x - B.x) < 0.1f)
    //     {
    //         float height = Mathf.Abs(A.y - B.y);
    //         Vector2 midPoint = (A + B)/2;
    //         GameObject wall = Instantiate(wallPrefab, midPoint, Quaternion.identity, transform);
    //         Utils.SetLengthAndHeightInWorldPosition(wall, wallThickness, height, false);
    //         return wall;
    //     }
    //     if(Mathf.Abs(A.y - B.y) < 0.1f)
    //     {
    //         float width = Mathf.Abs(A.x - B.x);
    //         Vector2 midPoint = (A + B)/2;
    //         GameObject wall = Instantiate(wallPrefab, midPoint, Quaternion.identity, transform);
    //         Utils.SetLengthAndHeightInWorldPosition(wall, width, wallThickness, false);
    //         return wall;
    //     }
    //     return null;
    // }
    public void StopCurrentLevel()
    {
        StopCoroutine(InstantiatePayloads());
    }

}
