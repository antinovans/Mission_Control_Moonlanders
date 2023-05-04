using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustController : MonoBehaviour
{

    /////////////////////////////////////////////////////////////////////////// Objects

    

    [SerializeField]
    private GameObject[] Boosters;

    [SerializeField]
    private Rigidbody2D landerRB;

    public SpriteRenderer[] fireRenders;

    /////////////////////////////////////////////////////////////////////////// Constants
    [Header("Constants")]
    [SerializeField]
    private float thrustAmt;

    /////////////////////////////////////////////////////////////////////////// Variables

    private bool isBoost1 = false;
    private bool isBoost2 = false;
    private bool isBoost3 = false;
    private bool isBoost4 = false;

    private void Start()
    {
        int fires = Boosters.Length;
        for (int i = 0; i < fires; i++)
        {
            fireRenders[i] = Boosters[i].GetComponent<SpriteRenderer>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        /////////////////////////////////////////////////////////////////////// Get Input
        isBoost1 = false;
        isBoost2 = false;
        isBoost3 = false;
        isBoost4 = false;

        if (Input.GetKey(KeyCode.Q))
        {
            isBoost1 = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            isBoost2 = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            isBoost3 = true;
        }
        if (Input.GetKey(KeyCode.M))
        {
            isBoost4 = true;
        }

    }

    private void FixedUpdate()
    {

        if (isBoost1)
        {
            Thrust(0);
            fireRenders[0].enabled = true;
        }
        else
        {
            fireRenders[0].enabled = false;
        }

        if (isBoost2)
        {
            Thrust(1);
            fireRenders[1].enabled = true;
        }
        else
        {
            fireRenders[1].enabled = false;
        }

        if (isBoost3)
        {
            Thrust(2);
            fireRenders[2].enabled = true;
        }
        else
        {
            fireRenders[2].enabled = false;
        }

        if (isBoost4)
        {
            Thrust(3);
            fireRenders[3].enabled = true;
        }
        else
        {
            fireRenders[3].enabled = false;
        }

    }

    public void Thrust(int thruster)
    {
        landerRB.AddForceAtPosition(Boosters[thruster].transform.TransformDirection(Vector2.left) * thrustAmt, Boosters[thruster].transform.position);
    }


}
