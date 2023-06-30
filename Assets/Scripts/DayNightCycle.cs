using System.Collections;
using System.Collections.Generic;
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
    public delegate void TimeAction();
    public static event TimeAction OnDay;
    public static event TimeAction OnNight;

    private bool isDay;

    private void Update()
    {
        float currentLightIntensity = lightIntensityCurve.Evaluate(Time.time);
        light2D.intensity = currentLightIntensity;
     
        if(useColor)
            light2D.color = new Color(redCurve.Evaluate(Time.time), greenCurve.Evaluate(Time.time), blueCurve.Evaluate(Time.time));

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
