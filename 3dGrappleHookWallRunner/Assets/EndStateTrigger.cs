using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStateTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SceneLoader.sceneLoader.LoadFirstScene();
        }
    }
}
