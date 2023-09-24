using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private AnimationCurve lightIntensityCurve;
    [SerializeField] private bool useColor = true;
    [SerializeField] private AnimationCurve redCurve;
    [SerializeField] private AnimationCurve greenCurve;
    [SerializeField] private AnimationCurve blueCurve;
    [SerializeField] private Light2D light2D;
    [SerializeField] private float startFromTime;

    private float startTime;

    public delegate void TimeAction();
    public event TimeAction OnDay;
    public event TimeAction OnNight;

    private bool isDay = true;
    private void Start()
    {
        float oneHourDuration = lightIntensityCurve[lightIntensityCurve.length - 1].time / 24;
        startTime = oneHourDuration * startFromTime;


    }

    private void Update()
    {
        float currentTime = startTime + Time.time;
        float currentLightIntensity = lightIntensityCurve.Evaluate(currentTime);
        light2D.intensity = currentLightIntensity;
     
        if(useColor)
            light2D.color = new Color(redCurve.Evaluate(currentTime), greenCurve.Evaluate(currentTime), blueCurve.Evaluate(currentTime));

        if(currentLightIntensity > 0.4 && !isDay)
        {
            if(OnDay != null)
                OnDay();
            Debug.Log("isDay");
            isDay = true;
        }
        else if(currentLightIntensity < 0.4 && isDay)
        {
            if (OnNight != null)
                OnNight();
            Debug.Log("isNight");
            isDay = false;
        }
    }
}
