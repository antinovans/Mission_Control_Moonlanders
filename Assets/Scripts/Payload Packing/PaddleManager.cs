using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PaddleManager : MonoBehaviour
{
    public static PaddleManager instance;
    public GameObject paddlePrefab;
    public int playerNum;
    public List<KeyCode> playerBinds;
    public List<Vector2> paddlePositions;
    [Header("Paddle attributes")]
    public int paddleScale = 3; //default is 3.0f
    public float paddleMass = 2.0f; //default is 2.0f
    public float angularDrag = 1.0f;
    public float paddleOffset = 0.2f;
    private float _paddleLength = 1.0f;
    
    private void Awake() {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        paddlePositions = new List<Vector2>();
        InitializePaddles();
    }
    // Start is called before the first frame update
    void Start()
    {
    }
    private void InitializePaddles()
    {
        Assert.IsNotNull(playerBinds);
        Vector2 origin = Vector2.zero;
        Assert.IsNotNull(paddlePrefab);
        for(int i = 0; i < playerNum; i++)
        {
            // int shouldOffset = i == 0 ? 0 : 1;
            Vector2 paddlePosition = origin + new Vector2(i * paddleScale * _paddleLength + 
            paddleScale * _paddleLength - i * paddleOffset, 0);
            paddlePositions.Add(paddlePosition);
            GameObject paddle = Instantiate(paddlePrefab, paddlePosition, Quaternion.identity);
            if(paddle.TryGetComponent<Paddle> (out Paddle p))
            {
                p.SetSpriteColor(Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f));
                p.SetScale(paddleScale);
                p.SetLeftRotateKey(playerBinds[i*2]);
                p.SetRightRotateKey(playerBinds[i*2 + 1]);
            }
            if(paddle.TryGetComponent<Rigidbody2D> (out Rigidbody2D rb))
            {
                rb.angularDrag = angularDrag;
                rb.mass = paddleMass;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
