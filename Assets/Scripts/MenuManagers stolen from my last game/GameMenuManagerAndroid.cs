using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Sei que poderia ter colocado as strings em um vetor String[] e controlado um random dentro do mesmo. Mas, bem, fiz assim.

public class GameMenuManagerAndroid : MonoBehaviour
{
    public GameObject canvas; //Foi aqui que coloquei a música ingame
    public GameObject endGameMenu;
    public GameObject pauseButton;
    public AudioClip[] inGameMusic;
    public Text youLoseOrWin;
    public Text coneKick;
    private AudioSource audioSource;
    private int score;
    private Coroutine theCoroutine; //Para ter certeza de estar chamando a mesma coroutine ao invés de instances do mesmo
    private bool music;

    void Start()
    {
        Time.timeScale = 1;

        audioSource = canvas.GetComponent<AudioSource>();
        music = true;
        InGameSound();

        score = 0;
        endGameMenu.SetActive(false);
        pauseButton.SetActive(true);
        Score(1); 
    }

    public void InGameSound()
    {
        if(music)
        {
            int i = Random.Range(0, inGameMusic.Length);
            audioSource.clip = inGameMusic[i];
            audioSource.Play();
            Invoke("InGameSound", audioSource.clip.length); //OK ISSO É LOUCO, mas eu chamo esta mesma função no momento em que o audio acabar de tocar.
            //achei em https://www.youtube.com/watch?v=sOLfxVbUrAc;
        }
        else if(music == false)
        {
            audioSource.Stop();
        }
    }

    public void SetActivity(int caso) //Quando o player perde será encaminhada esta função
    {
        endGameMenu.SetActive(true); //Que ativa o menu de endgame e escolhe dependendo da forma que o player perdeu o texto mais coerente.
        pauseButton.SetActive(false);
        Score(1);
        music = false;
        InGameSound();
        //A música do EndGameMenu é ativada ao iniciá-lo
        if (PlayerPrefs.GetFloat("Score", 0) < score) //Além de não esquecer de guardar se for highscore
        {
            PlayerPrefs.SetFloat("Score", score);
            PlayerPrefs.Save();
            youLoseOrWin.text = "New High Score: " + score;
        }
        else
        {
            switch (caso)
            {
                case 1:
                    switch (Random.Range(1, 5))
                    {
                        case 1:
                            youLoseOrWin.text = "OUT";
                            break;
                        case 2:
                            youLoseOrWin.text = "Oof";
                            break;
                        case 3:
                            youLoseOrWin.text = "Can you even stop falling?";
                            break;
                        case 4:
                            youLoseOrWin.text = "That hurts";
                            break;
                    }
                    break;
                case 2:
                    switch (Random.Range(1, 4))
                    {
                        case 1:
                            youLoseOrWin.text = "Blublublub";
                            break;
                        case 2:
                            youLoseOrWin.text = "So, is the water wet?";
                            break;
                        case 3:
                            youLoseOrWin.text = "I don't know if she can swim";
                            break;
                    }
                    break;
                case 3:
                    switch (Random.Range(1, 4))
                    {
                        case 1:
                            youLoseOrWin.text = "Enemy hurdle ahe- Oh.";
                            break;
                        case 2:
                            youLoseOrWin.text = "That's not for kicking";
                            break;
                        case 3:
                            youLoseOrWin.text = "pof";
                            break;
                    }
                    break;
                case 4:
                    switch (Random.Range(1, 4))
                    {
                        case 1:
                            youLoseOrWin.text = "Are you happy with FALLING?";
                            break;
                        case 2:
                            youLoseOrWin.text = "How does it feel to fall FOREVER?";
                            break;
                        case 3:
                            youLoseOrWin.text = "Ever dreamed about falling FOREVER?";
                            break;
                    }
                    break;
            } 
        }
        
        
    }

    public void Score(int caso)
    {
        
        switch (caso){
            case 1:
                if (theCoroutine != null)
                {
                    StopCoroutine(theCoroutine);
                }
                //StopCoroutine(WaitText()); //Isto não funciona pois podemos ter várias instâncias do WaitText rodando simultaneamente. 
                //Por isso usei o theCoroutine para controlar e ser apenas UMA instância.
                coneKick.text = "";
                break;
            case 2:
                if (theCoroutine != null)
                {
                    StopCoroutine(theCoroutine);
                }

                score ++; //Ela só recebe pontos se acertar o cone enquanto desliza
                switch (Random.Range(1, 5))
                {
                    case 1:
                        coneKick.text = "Cone Kick!";
                        break;
                    case 2:
                        coneKick.text = "PEW";
                        break;
                    case 3:
                        coneKick.text = "Look at it go";
                        break;
                    case 4:
                        coneKick.text = "Taking the skies";
                        break;
                }
                theCoroutine = StartCoroutine(WaitText()); //Aqui queremos que o Score volte a ser mostrado na parte de baixo da tela ao final de x segundos, então enviamos para a corotina.
                break;
            case 3:
                if (theCoroutine != null)
                {
                    StopCoroutine(theCoroutine);
                }

                switch (Random.Range(1, 5))
                {
                    case 1:
                        coneKick.text = "Meh";
                        break;
                    case 2:
                        coneKick.text = "Kinda boring";
                        break;
                    case 3:
                        coneKick.text = "**Slide before kicking it**";
                        break;
                    case 4:
                        coneKick.text = "paft";
                        break;
                }
                theCoroutine = StartCoroutine(WaitText());
                break;
            case 4:
                coneKick.text = "Kicked " + score;
                break;
            case 5: //Caso de Pause
                if (theCoroutine != null)
                {
                    StopCoroutine(theCoroutine);
                }
                coneKick.text = "Game Paused";
                break;
            case 6:
                if(theCoroutine != null)
                {
                    StopCoroutine(theCoroutine);
                }

                coneKick.text = "WRONG WAY";

                theCoroutine = StartCoroutine(WaitText());
                break;
            
        }
    }

    IEnumerator WaitText() //Espera para apresentar o score novamente na parte de baixo da tela
    {
        yield return new WaitForSeconds(4);
        Score(4);

    }


    public void Play()//Função associada no botão play
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); //Poderia ter feito a mesma coisa que fiz em update para cá, e deixar só um botão de quit. Mas fiz assim.

    }

    public void ThePause()
    {
        if (Time.timeScale != 0)
        {
            endGameMenu.SetActive(true);
            youLoseOrWin.text = "";
            Time.timeScale = 0;
            Score(5);
        }
        else
        {
            endGameMenu.SetActive(false);
            Time.timeScale = 1;
            Score(1);
        }
    }


}
