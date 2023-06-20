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
   
    private void Update()
    {
        light2D.intensity = lightIntensityCurve.Evaluate(Time.time);
        if(useColor)
            light2D.color = new Color(redCurve.Evaluate(Time.time), greenCurve.Evaluate(Time.time), blueCurve.Evaluate(Time.time));
    }
}
