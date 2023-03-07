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

    [SerializeField] ParLevelTimes parLevelTimes;

    private void OnEnable() // add the display end state ui as a listener to the endstate event
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
        if (stopwatch == null)
            Debug.LogWarning("WARNING!!! Missing stopwatch script in scene!");
    }

    /// <summary>
    /// Method called when OnFinishLinePassed event is triggered activates ui and displays the current time and medal
    /// </summary>
    public void DisplayEndStateUI()
    {
        endStateUI.SetActive(true);
        endStateStopWatchText.text = stopWatchText.text;
        stopWatchText.gameObject.SetActive(false);
        DisplayMedal();
    }

    /// <summary>
    /// check the current time against the level par time, level par time is set in inspector with a levelpartimes object
    /// </summary>
    void DisplayMedal()
    {
        if(stopwatch.currentTime > parLevelTimes.silverPar && stopwatch.currentTime <= parLevelTimes.bronzePar)
        {
            medalDisplay.sprite = medalSprites[0];
        }
        else if(stopwatch.currentTime > parLevelTimes.goldPar && stopwatch.currentTime <= parLevelTimes.silverPar)
        {
            medalDisplay.sprite = medalSprites[1];
        }
        else if(stopwatch.currentTime <= parLevelTimes.goldPar)
        {
            medalDisplay.sprite = medalSprites[2];
        }
        else if(stopwatch.currentTime > parLevelTimes.bronzePar)
        {
            medalDisplay.gameObject.SetActive(false);
        }

    }

}
