using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este código fica em cada gameobject com a luz.

public class StreetLightControl : MonoBehaviour
{
    private Light lightHere;
    private SunlightControl control;
    private SunlightControlForAndroid control2;

    void OnEnable() //No momento em que aparecer na cena.
    {
        lightHere = GetComponent<Light>();

        control = GameObject.FindWithTag("Sunlight").GetComponent<SunlightControl>();
        if (control == null)
        {
            control2 = GameObject.FindWithTag("Sunlight").GetComponent<SunlightControlForAndroid>();
            lightHere.enabled = control2.LightSet();
        }
        else
        {
            lightHere.enabled = control.LightSet(); //Conferir se é para estar ligado ou não
        }
        
        //Lembro-me que não é muito bom enviar membros diretamente para outras classes, por isso fiz esta função. Um tipo de GETCOMPONENT.
    }
    

}
