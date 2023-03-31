using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI successText;
    [SerializeField]
    public TextMeshProUGUI failText;
    private int score = 0;
    private int fail = 0;

    private void Start()
    {
        successText.text = $"success: {0}";
        failText.text = $"fail: {0}";
        EventManager.instance.onPointsChangeEvent += AddPoints;
    }

    public void AddPoints(Shape s, bool isAdd)
    {
        if(isAdd)
        {
            score += 1;
            successText.text = $"success: {score}";
            return;
        }
        if(!isAdd)
        {
            fail += 1;
            failText.text = $"fail: {fail}";
            return;
        }
    }

    
}
