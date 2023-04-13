using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PaddleManager : MonoBehaviour
{
    public static PaddleManager instance;
    // public GameObject paddlePrefab;
    public int playerNum;
    public List<KeyCode> playerBinds;
    public List<Vector2> paddlePositions;
    [Header("Paddle attributes")]
    public float paddleWidth = 3; //default is 3.0f
    public float paddleMass = 2.0f; //default is 2.0f
    public float angularDrag = 1.0f;
    public float paddleOffset = 0.2f;
    public float rotationSpeed = 100f;
    
    private void Awake() {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        paddlePositions = new List<Vector2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        paddleOffset = LevelBuilder.instance.GetWallStats().x;
        InitializePaddles();
    }
    private void InitializePaddles()
    {
        Assert.IsNotNull(playerBinds);
        Vector2 origin = Vector2.zero;
        // Assert.IsNotNull(paddlePrefab);
        GameObject parent = new GameObject("players");
        for(int i = 0; i < playerNum; i++)
        {
            // int shouldOffset = i == 0 ? 0 : 1;
            Vector2 paddlePosition = origin + new Vector2((2*i + 1) * (paddleWidth/2) + i * paddleOffset, 0);
            paddlePositions.Add(paddlePosition);
            Paddle paddle = PaddleBuilder.instance.BuildPadle(i, paddlePosition, paddleWidth);
            paddle.SetLeftRotateKey(playerBinds[i*2]);
            paddle.SetRightRotateKey(playerBinds[i*2 + 1]);
            paddle.SetAngularDrag(angularDrag);
            paddle.SetMass(paddleMass);
            paddle.SetRotationSpeed(rotationSpeed);
            paddle.transform.SetParent(parent.transform);
        }
    }
}
