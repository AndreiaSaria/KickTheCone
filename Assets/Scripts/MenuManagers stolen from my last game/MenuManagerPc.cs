using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.Audio; //Biblioteca necessária para usar funções de audio

//O MenuManager inicia a musica e confere se a mesma já foi iniciada anteriormente para não termos uma musica de background duplicada
public class MenuManagerPc : MonoBehaviour {

    public static bool mouseOrKeyboard; // Valor que será usado em player para saber se é pelo mouse ou teclado função dele lá em baixo
    //Declarações públicas para facilitar
    //public GameObject music;
    public GameObject configCanvas; //Para ativar/desativar de acordo com o config
    public GameObject mainCanvas; //Para ativar/desativar de acordo com o config
    public GameObject quitButton;
    public Text highScoreText;
    public Button resetHighScore;
    //public AudioMixer mixer;

    public void Start() //Ao iniciar na cena
    {
        //if (GameObject.FindGameObjectWithTag("Music") == false) //Confere se existe algum gameObject de musica (a musica de background)
        //{
        //    Instantiate(music); // Se não existir, iniciar a mesma.
        //}
        Camera.main.orthographic = true;
        highScoreText.gameObject.SetActive(false);
        highScoreText.gameObject.transform.parent.gameObject.SetActive(false);
        resetHighScore.gameObject.SetActive(false);
        quitButton.SetActive(true);

        mainCanvas.SetActive(true);
        configCanvas.SetActive(false);

    }

    public void Play()//Função associada no botão play
    {
        SceneManager.LoadScene(1);

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
        if (Camera.main.orthographic == true & mainCanvas.activeSelf)
        {
            mainCanvas.SetActive(false);
            configCanvas.SetActive(true);
        }
        else if (Camera.main.orthographic == true & mainCanvas.activeSelf == false)
        {
            mainCanvas.SetActive(true);
            configCanvas.SetActive(false);
        }
        else if (Camera.main.orthographic == false & configCanvas.activeSelf == false)
        {
            configCanvas.SetActive(true);
        }
        else if (Camera.main.orthographic == false & configCanvas.activeSelf)
        {
            configCanvas.SetActive(false);
        }
    }

    public void MushWatch()
    {
        if (mainCanvas.activeSelf)
        {
            Camera.main.orthographic = false;
            mainCanvas.SetActive(false);
        }
        else if (configCanvas.activeSelf & Camera.main.orthographic)
        {
            Camera.main.orthographic = false;
            configCanvas.SetActive(false);
        }
        else if(Camera.main.orthographic == false)
        {
            Camera.main.orthographic = true;
            configCanvas.SetActive(false);
            mainCanvas.SetActive(true);
        }

    }

    public void ResetHighScore() //função para resetar highscore
    {
        PlayerPrefs.DeleteKey("Score");
        HighScore();
 
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) //Se apertar esc
        {   
                Quit();         
        }
        //MouseOrKeyboard();
        //Pew();
    }


    //public void MouseOrKeyboard() //Fiz assim pois o botão só retorna um valor ao ser apertado, não importa quantas vezes foi apertado
    //{// Eu chamo essa função no update 
    //    //podería te-lo colocado como variável publica mas fiz assim.
    //    bool setting = GameObject.Find("MouseKey").GetComponent<Toggle>().isOn; //Pesquiso o botão
    //    mouseOrKeyboard = setting; // E defino se ele está on como true e false quando off

    //}

    //public void Pew()
    //{
    //    bool setting = GameObject.Find("PewToggle").GetComponent<Toggle>().isOn;
    //    if (setting)
    //    {
    //        mixer.SetFloat("PewVolume", 0f);
    //    }
    //    else
    //    {
    //        mixer.SetFloat("PewVolume", -80f);
    //    }
    //}
}
