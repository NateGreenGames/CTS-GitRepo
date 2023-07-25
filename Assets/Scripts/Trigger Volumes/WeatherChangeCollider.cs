using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChangeCollider : MonoBehaviour
{
    [Header("Drag the wave system here if you intend to change wind speed or angle")]
    public GameObject waveSystem;
    [Header("Type the name of the weather preset you wish to change to (Case Sensitive)")]
    public string weatherName;
    [Header("Type the name of the tag for the collider you are looking for (Case Sensitive)")]
    public string colliderTag;
    [Header("Check the boxes if you wish to change wind speed and angle")]
    public bool changeWindSpeed;
    public float userWindSpeed;
    public bool changeWindAngle;
    public float userWindAngle;

    public bool changeHour;
    public int userHour;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == colliderTag && changeWindSpeed == true) waveSystem.GetComponent<Crest.ShapeFFT>()._windSpeed = userWindSpeed;
        if (other.transform.tag == colliderTag && changeWindAngle == true) waveSystem.GetComponent<Crest.ShapeFFT>()._waveDirectionHeadingAngle = userWindAngle;
        if (other.transform.tag == colliderTag && changeHour == true) Enviro.EnviroManager.instance.Time.hours = userHour;
        //Checks to see if the thing that entered the collider is the player boat.
        if (other.transform.tag == colliderTag && weatherName != null)
        {
            
            //This Line checks to see what the current Weather is and returns it's name in Debug Log
            Enviro.EnviroWeatherType weatherType = Enviro.EnviroManager.instance.Weather.targetWeatherType;
            Debug.Log(weatherType);
            //If the weather isn't already Foggy it transitions to foggy, otherwise it will change back to cloudy 2
            Enviro.EnviroManager.instance.Weather.ChangeWeather(weatherName);
            Destroy(this);

        }


        

    }
}
