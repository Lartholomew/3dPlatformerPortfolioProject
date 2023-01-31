using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class Stopwatch : MonoBehaviour
{
    [HideInInspector]
    public float currentTime;
    bool running;

    [SerializeField] TMP_Text timerText;


    private void OnEnable()
    {
        FinishLine.OnFinishLinePassed += StopStopWatch;
    }

    private void OnDisable()
    {
        FinishLine.OnFinishLinePassed -= StopStopWatch;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0.50f;
        StartStopWatch();
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
            currentTime += Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void StartStopWatch()
    {
        running = true;
    }

    public void StopStopWatch()
    {
        running = false;
    }

}
