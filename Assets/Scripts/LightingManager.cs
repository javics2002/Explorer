// https://www.youtube.com/watch?v=m9hj9PdO328
// https://www.youtube.com/watch?v=mPS_nRwh_dM

using UnityEngine;
using System;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] Light sun;
    [SerializeField] LightingPreset preset;

    [SerializeField, Range(0, 90)] float angle;
    [SerializeField] float sunOffset;

    [SerializeField] bool useVirtualTime;
    [SerializeField, Range(0, 1)] float virtualTime;

    DateTime time;

    private void Start()
    {
        time = DateTime.Now;
    }

    private void Update()
    {
        if (preset == null)
            return;

        time.AddSeconds(Time.deltaTime);
        UpdateLighting();
    }


    private void UpdateLighting()
    {
        //Porcentaje del día, siendo 0 las 0:00:00 y 0,5 las 12:00:00
        float timePercent = useVirtualTime ? virtualTime : time.Hour / 24.0f + time.Minute / (24.0f * 60) + time.Second / (24.0f * 3600);
        Debug.Log("Porcentaje del dia: " + timePercent);

        //Set ambient and fog
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (sun != null) {
            sun.color = preset.DirectionalColor.Evaluate(timePercent);

            //Las 15:00:00 son la hora en el que el sol está más alto, y sunHeight es 1.
            //-1 es la mitad de la noche y 0 el amanecer y atardecer.
            float sunHeight = Mathf.Sin(2 * Mathf.PI * timePercent - 2.36f);
            Debug.Log("Altura del sol: " + sunHeight);
            sun.transform.localRotation = Quaternion.Euler(new Vector3(sunHeight * (90.0f - angle), timePercent * 360f - sunOffset, 0));
        }
    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (sun != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
            sun = RenderSettings.sun;
        //Search scene for light that fits criteria (directional)
        else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights) {
                if (light.type == LightType.Directional) {
                    sun = light;
                    return;
                }
            }
        }
    }
}