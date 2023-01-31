using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// TODO: end state ui needs to get the current level par times and compare to the current time on stopwatch to display the correct medal
public class EndStateUI : MonoBehaviour
{
    [SerializeField] GameObject endStateUI;

    [SerializeField] TMP_Text stopWatchText;
    [SerializeField] TMP_Text endStateStopWatchText;
    [SerializeField] Image medalDisplay;

    [SerializeField] Sprite[] medalSprites;
    // 0 - Bronze
    // 1 - silver
    // 3 - gold

    Stopwatch stopwatch;


    private void OnEnable()
    {
        FinishLine.OnFinishLinePassed += DisplayEndStateUI;
    }

    private void OnDisable()
    {
        FinishLine.OnFinishLinePassed -= DisplayEndStateUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        stopwatch = FindObjectOfType<Stopwatch>();

    }


    void DisplayEndStateUI()
    {
        endStateUI.SetActive(true);
        endStateStopWatchText = stopWatchText;
    }

}
