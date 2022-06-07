using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    TextMeshProUGUI clock;
    [SerializeField] LightingManager lightingManager;

    void Start()
    {
        clock = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float time = lightingManager.GetTime();

        //Convertimos el tiempo en rango 0 a 1 en horas y minutos del dia
        byte hour = (byte) (time * 24), 
            minute = (byte) (time * 24 * 60 % 60);

        //Los convertimos a string para escribirlos;
        string hourString = hour.ToString(),
            minuteString = minute.ToString();

        //Les damos formato de hora
        Add0(ref hourString);
        Add0(ref minuteString);
        
        clock.SetText(hourString + ":" + minuteString);
    }

    //Añade un 0 delante del caracter, si la hora solo tiene un digito
    void Add0(ref string s)
    {
        if(s.Length == 1)
            s = "0" + s;
    }
}
