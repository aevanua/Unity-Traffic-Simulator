using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [Range(0 , 1)]
    
    public float TimeOfDay;
    public float DayDur = 50f;
    public Light Sun;

    public AnimationCurve SunCurve;

    private float sunintensity;

    void Start()
    {
        sunintensity = Sun.intensity;
    }

    void Update()
    {
        TimeOfDay += Time.deltaTime / DayDur;
        if (TimeOfDay >= 1) TimeOfDay -= 1;

        Sun.transform.localRotation = Quaternion.Euler(TimeOfDay * 360f , 180 , 0);
        Sun.intensity = sunintensity * SunCurve.Evaluate(TimeOfDay);
    }
}
