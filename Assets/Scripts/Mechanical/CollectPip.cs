using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectPip : MonoBehaviour
{
    public Slider NavSlider;

    public int PipValue;
    public float navDegFreq;
    public float degrade;
    private float timer;
    public float textWait = 5;
    public TMP_Text success;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= navDegFreq)
        {
            NavSlider.value -= degrade;
            timer = 0;
        }

        if (NavSlider.value >= NavSlider.maxValue)
        {
            print("success!!!!!");
            success.enabled = true;
            StartCoroutine(TextHangOut());
            NavSlider.value = 6;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Pip")
        {
            print("increase!");
            NavSlider.value += PipValue;
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator TextHangOut()
    {
        yield return new WaitForSeconds(textWait);
        success.enabled = false;

    }
}
