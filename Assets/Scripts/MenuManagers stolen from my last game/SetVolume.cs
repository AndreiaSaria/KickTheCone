using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; //Biblioteca necessária para usar funções de audio
//Poderia ter colocado dentro de MenuManager mas achei melhor assim
//IMPORTANTE: Expor o valor para script no audio mixer. Selecionar o grupo e com o direito do mouse expor o valor para a alteração de volume
public class SetVolume : MonoBehaviour {

    public AudioMixer mixer;

    public void SetLevelMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20); //Porque o valor do volume é dado em decibéis
    }

    public void SetLevelFx(float sliderValue)
    {
        mixer.SetFloat("FxVol", Mathf.Log10(sliderValue) * 20); //Porque o valor do volume é dado em decibéis
    }

    public void SetLevelMusic(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20); //Porque o valor do volume é dado em decibéis
    }

}
