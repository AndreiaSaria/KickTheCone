using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunlightControl : MonoBehaviour
{ //Controle do sol na cena de jogo
    public float sunTurningSpeed; 
    public Material nightSkybox;
    private Material daySkybox;

    bool isItOn = true; //Aqui botar para false caso o jogo começe de dia

    void Start()
    {
        daySkybox = RenderSettings.skybox; //Recebe o skybox do Unity
    }

    void Update()
    {
        transform.Rotate(Time.deltaTime * sunTurningSpeed, 0, 0, Space.Self);

        if (transform.eulerAngles.x >= 160f && transform.eulerAngles.x <= 333f)  //Esse representa exatamente o momento que o sol se põe
        {
            RenderSettings.skybox = nightSkybox; //Logo trocar para o skybox da noite    
        }
        else
        {
            RenderSettings.skybox = daySkybox;
        }
        if (transform.eulerAngles.x >= 17.2f && transform.eulerAngles.x <= 160f) //Aqui coloquei um ângulo um pouco maior para que a luz só se apague quando o sol iluminar bem a cena.
        {
            isItOn = false;
        }
        else
        {
            isItOn = true;
            
        }
    }

    //Lembro-me que não é muito bom enviar membros diretamente para outras classes, por isso fiz esta função. Um tipo de GETCOMPONENT.
    public bool LightSet() //Envio então este bool para o streetLightControl
    {
        return isItOn;
    }
}
