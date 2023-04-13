using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject billboard;
    [SerializeField]
    private GameObject billboardL;
    [SerializeField]
    private GameObject billboardR;
    [SerializeField]
    private List<GameObject> collectorsPrefab;
    [SerializeField]
    private List<GameObject> payloadsPrefab;
    [SerializeField]
    private GameObject canvas;
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
        BuildBillboard();
        StartCoroutine(InstantiatePayloads());
    }

    void BuildWalls()
    {
        GameObject parent = new GameObject("Walls");
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
        lWallUp.transform.SetParent(parent.transform);
        lWallDown.transform.SetParent(parent.transform);


        float rightX = PaddleManager.instance.paddlePositions[PaddleManager.instance.paddlePositions.Count - 1].x + 
            PaddleManager.instance.paddleWidth /2 + wallThickness;
        GameObject rWallUp = Instantiate(wallPrefab, new Vector3(rightX, wallHeight/2, 1), Quaternion.identity);
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(rWallUp.transform, _id);
        GameObject rWallDown = Instantiate(wallPrefab, new Vector3(rightX, -wallHeight/2, 1), Quaternion.identity);
        _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(rWallDown.transform, _id);
        rWallUp.transform.SetParent(parent.transform);
        rWallDown.transform.SetParent(parent.transform);


        //building middle walls
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count - 1; i++)
        {
            float mid = (PaddleManager.instance.paddlePositions[i].x 
            + PaddleManager.instance.paddlePositions[i + 1].x) / 2;
            GameObject wall =  Instantiate(wallPrefab, new Vector3(mid, -wallHeight *9/16, 1), Quaternion.identity);
            wall.transform.SetParent(parent.transform);
        }
    }
    private void BuildCollectors()
    {
        GameObject parent = new GameObject("Collectors");
        for(int i = 0; i < PaddleManager.instance.paddlePositions.Count; i++)
        {
            Vector2 pos = new Vector2(PaddleManager.instance.paddlePositions[i].x, -wallHeight/2);
            GameObject collector = Instantiate(collectorsPrefab[i % collectorsPrefab.Count], pos, Quaternion.identity);
            PayloadCollector pCollector = collector.GetComponent<PayloadCollector>();
            pCollector?.SetScale(PaddleManager.instance.paddleWidth * 0.5f);
            collector.transform.SetParent(parent.transform);
        }
    }
    private void BuildBillboard() {
        GameObject parent = new GameObject("boarder");
        //build middle Billboard
        float posX = (PaddleManager.instance.paddlePositions[0].x + 
        PaddleManager.instance.paddlePositions[PaddleManager.instance.paddlePositions.Count - 1].x)/2;
        GameObject board =  Instantiate(billboard, new Vector3(posX, -8, 1), Quaternion.identity);
        int _id = MultipleCameraController.instance.GetDictSize();
        MultipleCameraController.instance.AddTransformToList(board.transform, _id);

        board.transform.SetParent(parent.transform);

        Vector2 midStats = GetPrefabStats(billboard);
        Vector2 leftStats = GetPrefabStats(billboardL);
        Vector2 rightStats = GetPrefabStats(billboardR);

        canvas.GetComponent<RectTransform>().position = board.transform.position;

        //instantiate billboard left and right wings
        for(int i = 0; i < PaddleManager.instance.playerNum/2; i++) {
            Vector3 posL = new Vector3(posX - midStats.x/2 - (2 * i + 1) * leftStats.x/2, -8, 1);
            GameObject left =  Instantiate(billboardL, posL, Quaternion.identity);
            left.transform.SetParent(parent.transform);

            Vector3 posR = new Vector3(posX + midStats.x/2 + (2 * i + 1) * rightStats.x/2, -8, 1);
            GameObject right =  Instantiate(billboardR, posR, Quaternion.identity);
            right.transform.SetParent(parent.transform);
        }
    }
    public Vector2 GetPrefabStats(GameObject prefab) 
    {
        float width = (float)prefab.GetComponent<SpriteRenderer>().sprite.textureRect.width / (float)LevelBuilder.PIXEL_PER_UNIT;
        float height = (float)prefab.GetComponent<SpriteRenderer>().sprite.textureRect.height / (float)LevelBuilder.PIXEL_PER_UNIT;
        return new Vector2(width, height);
    }
    public Vector2 GetWallStats()
    {
        return GetPrefabStats(wallPrefab);
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
            GameObject payload = Instantiate(payloadsPrefab[UnityEngine.Random.Range(0, PaddleManager.instance.paddlePositions.Count)],
            pos, Quaternion.identity);
            // payload.GetComponent<Payload>().SetSpriteColor(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
            payload.GetComponent<Rigidbody2D>().mass = 0.2f;
            payload.GetComponent<Rigidbody2D>().AddForce(forceDir, ForceMode2D.Impulse);
            yield return new WaitForSeconds(interval);
        }
    }
    public void StopCurrentLevel()
    {
        StopCoroutine(InstantiatePayloads());
    }

}
