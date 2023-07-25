using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public string[] weatherPresets; // List of weather presets created in Enviro 3. 
    public int weatherIndex;

    [Header("Transition times")]
    /*
    [Tooltip("This is the time it takes for the current weather to change to the new weather preset")]
    public float weatherTransitionTime = 60f;
    */
    [Tooltip("This is the time in between each weather change")]
    public float weatherChangeTime = 300f;

    private WindFollowBehavior wind;
    private CrestManager crestManager;

    private float fogDensitySmoothTime = 60.0f;
    private float windDirectionSmoothTime = 60.0f;

    private float fogDensityTarget = 0.0f;
    private float fogDensityVelocity = 0.0f;

    private void Start()
    {
        crestManager = GameManager.gm.crestReference;
        wind = GameObject.FindGameObjectWithTag("Wind").GetComponent<WindFollowBehavior>();

        // Set the fog mode to Exponential Squared
        RenderSettings.fogMode = FogMode.ExponentialSquared;

        weatherIndex = 0;
        InvokeRepeating("ChangeWeather", 0f, weatherChangeTime);
        InvokeRepeating("RandomWindDirection", 0f, 30f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            ChangeWeather();
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            RandomWindDirection();
        }

        // Update fog density based on the target value and smooth time
        float currentFogDensity = RenderSettings.fogDensity;
        float newFogDensity = Mathf.SmoothDamp(currentFogDensity, fogDensityTarget, ref fogDensityVelocity, fogDensitySmoothTime);
        RenderSettings.fogDensity = newFogDensity;

        // Turn off fog when the fog density target hits 0
        if (fogDensityTarget == 0.0f && currentFogDensity == 0.0f)
        {
            RenderSettings.fog = false;
        }
    }

    public void ChangeWeather()
    {
        // Save the current weather index
        int prevWeatherIndex = weatherIndex;

        // Generate a new weather index
        if (weatherIndex == 0) weatherIndex = Random.Range(1, 5);
        else weatherIndex = Random.Range(0, 6); // 7 is snow, not sure if we want snow

        // If the new weather index is the same as the previous one, do nothing
        if (weatherIndex == prevWeatherIndex) return;

        // Change the weather preset
        Enviro.EnviroManager.instance.Weather.ChangeWeather(weatherPresets[weatherIndex]);
        Debug.Log($"Weather Changed to: {weatherPresets[weatherIndex]}");

        /*
        if (weatherIndex == 6 || weatherIndex == 5 || weatherIndex == 4)
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = 0f;
            fogDensityTarget = 0.009f;
        }
        else
        {
            // Decrease the fog density back to 0 over 5 seconds when weather index changes to another number
            fogDensityTarget = 0.0f;
        }
        */
        /*
        // If the weather index is currently 4, 5, or 6, set the fog density to the target value
        if (weatherIndex == 4 || weatherIndex == 5 || weatherIndex == 6)
        {
            RenderSettings.fogDensity = fogDensityTarget;
        }
        */
    }

    public IEnumerator ChangeWindDirectionOverTime(float newDegrees, float time)
    {
        float startTime = Time.time;
        float curDegrees = GameManager.gm.crestReference.wavesReference._waveDirectionHeadingAngle;
        yield return new WaitForSeconds(5f);
        while (Time.time < startTime + time)
        {
            float t = (Time.time - startTime) / time;
            float smoothedDegrees = Mathf.Lerp(curDegrees, newDegrees, t);
            crestManager.ChangeWindSpeed(smoothedDegrees);
            yield return null;
        }
        wind.ChangeAlphaOne();
    }

    public void RandomWindDirection()
    {
        float degrees = Random.Range(0, 360);
        wind.ChangeAlphaZero();
        
        StartCoroutine(ChangeWindDirectionOverTime(degrees, windDirectionSmoothTime));
        
        Debug.Log($"Wind Direction Changing to: {degrees} from {crestManager.wavesReference._waveDirectionHeadingAngle}");
    }
}


