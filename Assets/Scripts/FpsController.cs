using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    public float pollingTime;
    private float time;
    private int frameCount;
    private TMPro.TextMeshProUGUI fpsText;

    public enum Limits
    {
        noLimit=0,
        limit30=30,
        limit60=60,
        limit120=120,
        limit240=240,
    }
    public Limits limit;

    private void Awake()
    {
        Application.targetFrameRate = (int)limit;
    }

    void Start()
    {
        fpsText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        WriteFps();
    }

    private void WriteFps()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate.ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}
