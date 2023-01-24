using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public delegate void FinishLineAction();
    public static event FinishLineAction OnFinishLinePassed;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            OnFinishLinePassed();
        }
    }
}
