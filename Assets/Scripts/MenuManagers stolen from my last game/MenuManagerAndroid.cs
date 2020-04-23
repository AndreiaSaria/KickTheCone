//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Audio; //Biblioteca necessária para usar funções de audio

//O MenuManager inicia a musica e confere se a mesma já foi iniciada anteriormente para não termos uma musica de background duplicada
public class MenuManagerAndroid : MonoBehaviour
{
    //Declarações públicas para facilitar
    //public GameObject music;
    public GameObject configCanvas; //Para ativar/desativar de acordo com o config
    public GameObject mainCanvas; //Para ativar/desativar de acordo com o config
    public GameObject quitButton;
    public Text highScoreText;
    public Button resetHighScore;
    public Button resetTutorial;
    //public AudioMixer mixer;

    public void Start() //Ao iniciar na cena
    {

        highScoreText.gameObject.SetActive(false);
        highScoreText.gameObject.transform.parent.gameObject.SetActive(false);
        resetHighScore.gameObject.SetActive(false);
        quitButton.SetActive(true);

        mainCanvas.SetActive(true);
        configCanvas.SetActive(false);

    }

    public void Play()//Função associada no botão play
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }

    }

    public void Quit()//Função associada no botão quit.
    {
        Application.Quit();
    }

    public void HighScore()
    {
        if (quitButton.activeSelf == true) //Se o botão quit estiver ativo, desativar e mostrar o highscore no lugar dele
        {
            quitButton.SetActive(false);
            highScoreText.gameObject.SetActive(true);
            highScoreText.gameObject.transform.parent.gameObject.SetActive(true);
            highScoreText.text = "The highest score is " + PlayerPrefs.GetFloat("Score", 0).ToString();
            resetHighScore.gameObject.SetActive(true);
        }
        else //Se não, ativar o botão quit.
        {
            resetHighScore.gameObject.SetActive(false);
            highScoreText.gameObject.SetActive(false);
            highScoreText.gameObject.transform.parent.gameObject.SetActive(false);
            quitButton.SetActive(true);
        }

    }

    public void Config()
    {
        if (mainCanvas.activeSelf)
        {
            mainCanvas.SetActive(false);
            configCanvas.SetActive(true);
        }
        else
        {
            mainCanvas.SetActive(true);
            configCanvas.SetActive(false);
        }

    }
    
    public void ResetHighScore() //função para resetar highscore
    {
        PlayerPrefs.DeleteKey("Score");
        HighScore();
    }

    public void ResetTutorial()
    {
        if (PlayerPrefs.HasKey("TutorialDone"))
        {
            PlayerPrefs.DeleteKey("TutorialDone");
            resetTutorial.gameObject.SetActive(false);
        }
    }

}

