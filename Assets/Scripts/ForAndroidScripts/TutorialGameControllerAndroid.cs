using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChanForAndroid;
//using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


//Observação You can't have a transform without a game object; Transform is a component that's attached to a GameObject. All GameObjects have a Transform, which is the only component that's not optional. So you can do "var newTrans = new GameObject().transform;"
//https://forum.unity.com/threads/making-a-new-transform-in-code.49277/


//Isso aqui é uma sopa dos meus códigos. Eu os juntei para poder configurar melhor o tutorial.
public class TutorialGameControllerAndroid : MonoBehaviour
{
    private GameObject player; //Para a localização do player
    private GameObject initialTransform; //Transform do inicio do tutorial
    private GameObject checkPoint; 
    private Coroutine routineGoing; //Para controlar a coroutine
    private AudioSource audioSource;
    private bool music;

    public Transform[] targets1; //Checkpoints1
    public Transform[] targets2; //Checkpoints2

    public UnityChan_For_Infinite_Dodge_Android playerScript; //Script que recebe do player para ajustar a animação
    public GameObject waterSplash; //Partículas
    public float pushBackForce = 200f; //Força que a empurra para trás
    public float cameraPushBack = 2f; //Distância da câmera após a morte
    public int frameRate;
    public AudioClip[] coneHit;
    public AudioClip[] coneTouch;
    
    public GameObject canvas; //Foi aqui que coloquei a música ingame
    public GameObject pauseButton;
    public GameObject gameOverMenu;
    public AudioClip[] inGameMusic;
    public TMP_Text coneKick;


    void Start()
    {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Time.timeScale = 1; //Para evitar bugs.
        gameOverMenu.SetActive(false);

        initialTransform = new GameObject(); //temos de inicializar um gameobject já que um transform não pode existir sem um
        checkPoint = new GameObject();

        Application.targetFrameRate = frameRate; //Seria para melhor performance
        player = GameObject.FindWithTag("Player"); //Encontrando o player, pois podem ser modelos diferentes

        initialTransform.transform.position = player.transform.position; //Temos de ajustar parâmetro por parâmetro já que se igualar o transform ele modifica de acordo com o anterior.
        initialTransform.transform.rotation = player.transform.rotation;
        initialTransform.transform.localScale = player.transform.localScale;
        checkPoint.transform.position = initialTransform.transform.position;
        checkPoint.transform.rotation = initialTransform.transform.rotation;
        checkPoint.transform.localScale = initialTransform.transform.localScale;

        audioSource = canvas.GetComponent<AudioSource>();
        music = true;
        InGameSound();
    }

    void Update()
    {
        //Para ajustar a velocidade do player, aqui definimos o quão rápido ela deve aumentar.
        if (Time.timeSinceLevelLoad > 1)
        {
            if (playerScript.forwardSpeed < 4)
            {
                playerScript.forwardSpeed = playerScript.forwardSpeed + (Time.timeSinceLevelLoad / 100);
            }
        }

        //Parte do texto do tutorial que muda de acordo com os checkpoints em que o player passou
        if (checkPoint.transform.position.z == initialTransform.transform.position.z)
        {
            coneKick.text = "Here we use accelerometer, rotate your wrist right.";
        }
        else if (checkPoint.transform.position.z == targets1[0].position.z)
        {
            coneKick.text = "Swipe up to jump";
        }
        else if (checkPoint.transform.position.z == targets1[1].position.z)
        {
            coneKick.text = "Rotate your wrist left";
        }
        else if (checkPoint.transform.position.z == targets1[2].position.z)
        {
            coneKick.text = "Swipe right to do the curve";
        }
        else if (checkPoint.transform.position.x == targets2[0].position.x)
        {
            coneKick.text = "Swipe down to slide";
        }
        else if (checkPoint.transform.position.x == targets2[1].position.x)
        {
            coneKick.text = "Slide to kick cones and win points";
        }
        else if (checkPoint.transform.position.x == targets2[2].position.x) //Se passar do ultimo ponto voltar ao anterior 
        {
            checkPoint.transform.position = targets2[1].position;
            this.RestartCoroutine(WaitMenu(), ref routineGoing);
        }
        

        if (PlayerRayCheck().name == "Track2" || PlayerRayCheck().name == "Track6") //Esses são os tracks que estão indo no sentido de x
        {
            foreach(Transform t in targets2)
            {
                if(player.transform.position.x >= t.position.x)
                {
                    checkPoint.transform.position = t.position;
                    checkPoint.transform.rotation = t.rotation;
                }
            }
        }
        else
        {
            foreach (Transform t in targets1) //Já estes estão indo no sentido de z
            {
                if (player.transform.position.z >= t.position.z)
                {
                    checkPoint.transform.position = t.position;
                    checkPoint.transform.rotation = t.rotation;
                }
            }
        }



        WrongWay();
        Falling();
    }
   
    public void Falling() //O player decidiu voltar ao primeiro track renderizado e cair.
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody>().worldCenterOfMass.y) > 4) //Aqui de novo sabemos que meus tracks se mantêm a uma certa altura, se cair além disso ela saiu do track; 
        {
            this.RestartCoroutine(WaitMenu(), ref routineGoing);
        }
    }

    public void WrongWay() //O player está indo no sentido errado.
    {
        if (PlayerRayCheck() != null && player.transform.forward.z == (-PlayerRayCheck().Start.forward.z))
        {
            this.RestartCoroutine(WaitMenu(), ref routineGoing);
        }
    }


    public void CollidingSetting(Collision collision)
    {
        //Aqui estão os casos de colisão. Estão aqui para tanto limpar o código da Unity chan como para aplicarmos a outros players.


        if (collision.collider.gameObject.tag == "Out")
        {
            playerScript.enabled = false; //Para parar de receber input e se mover.
            playerScript.SendMessage("TouchAnimation", 3); // Case 3 caiu fora.
            
            this.RestartCoroutine(WaitMenu(), ref routineGoing); // Start and stopCoroutine não funcionavam para meu propósito então procurei na internet e achei esse código adicional para o Unity.
            //https://forum.unity.com/threads/how-to-cancel-and-restart-a-coroutine.435493/

        }
        else if (collision.collider.gameObject.tag == "OutOnWater")
        {
            playerScript.enabled = false;
            Instantiate(waterSplash, player.transform); //Neste caso iniciamos as partículas de água.
                                                        //Com elas o som se inicia

            playerScript.SendMessage("TouchAnimation", 2); //Case 2 cair na água

            this.RestartCoroutine(WaitMenu(), ref routineGoing);

        }
        else if (collision.collider.gameObject.tag == "Enemy")
        {
            playerScript.enabled = false;
            player.transform.rotation.SetLookRotation(collision.GetContact(collision.contactCount - 1).point.normalized); //Fazer com que o player olhe para o ponto de colisão. (não funciona tããão bem)

            if (!Physics.Raycast(player.transform.position, Vector3.back, 2)) //Para evitar dela entrar em superfícies. Confere se tem algo atrás dela antes de fazer o impulso.
            {
                player.GetComponent<Rigidbody>().AddForce(-collision.GetContact(collision.contactCount - 1).point.normalized * pushBackForce, ForceMode.Impulse);
            }
            
            playerScript.SendMessage("TouchAnimation", 1); //Ativando a animação de cair, case 1 encostar no enemy

            this.RestartCoroutine(WaitMenu(), ref routineGoing);

        }
        else if (collision.collider.gameObject.tag == "Point")
        {
            AnimatorClipInfo[] info = player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0); //Aqui pegamos a informação do animator do player, o current animator clip só envia arrays então recebemos como um.
            if (info[0].clip.name == "SLIDE00") //Mas sabemos que apenas o primeiro termo do array é a posição de animação do momento. Aqui conferimos se é o slide.
            {
                collision.collider.gameObject.GetComponent<Rigidbody>().AddForce(collision.GetContact(collision.contactCount - 1).point.normalized * 10, ForceMode.Impulse);
                //Se for, fazemos o objeto com que ela colidiu sair voando e adicionamos score.

                PlayerPrefs.SetString("TutorialDone", "Done");// Se completou o tutorial é salvo nos playerprefs que já foi feito.
                this.enabled = false;
                coneKick.text = "Tutorial Done";

                ThePause();


                if (collision.collider.gameObject.GetComponent<AudioSource>().enabled)
                {
                    int i = Random.Range(0, coneHit.Length);
                    collision.collider.gameObject.GetComponent<AudioSource>().clip = coneHit[i];
                    collision.collider.gameObject.GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                this.RestartCoroutine(WaitMenu(), ref routineGoing);
                //Além de adicionar o som ao objeto
                if (collision.collider.gameObject.GetComponent<AudioSource>().enabled)
                {
                    int i = Random.Range(0, coneTouch.Length);
                    collision.collider.gameObject.GetComponent<AudioSource>().clip = coneTouch[i];
                    collision.collider.gameObject.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    IEnumerator WaitMenu() //Espera para apresentar o score novamente na parte de baixo da tela
    {
        yield return new WaitForSeconds(1);
        player.transform.position = checkPoint.transform.position;
        player.transform.rotation = checkPoint.transform.rotation;
        playerScript.enabled = true;

        playerScript.GetComponent<Animator>().Play("Idle"); //Para retomar a animação já que ao perder não temos um retorno de animação.
    }

    public Module PlayerRayCheck() //Para que isso? Para podermos controlar quantos prefabs ficariam atrás do player
    {
        LayerMask layerMaskGround = LayerMask.GetMask("Ground"); //Fazendo uma layermask somente encontrar os prefabs do chão no raycast

        RaycastHit hitOnGround;//Criando um raycasthit para conseguir retornar os objetos que são tocados pelo raycast

        if (Physics.Raycast(new Vector3(player.transform.position.x, 0.5f, player.transform.position.z)
            , -Vector3.up, out hitOnGround, layerMaskGround)) //O raycast, ele tem esse número 0.5 no vetor inicial para que não saia dos pés do mesmo, um pouquinho acima somente.
        {
            var theObject = hitOnGround.collider.transform.root.gameObject;
            //Não sabia que existia esse root. Solução encontrada em :https://answers.unity.com/questions/33552/gameobjectparent.html

            if (theObject != null)
            {
                GameObject whereSheIs = theObject;
                return whereSheIs.GetComponent<Module>();
            }
            else return null;
        }
        else
        {
            return null;
        }
    }

    public void InGameSound()
    {
        if (music)
        {
            int i = Random.Range(0, inGameMusic.Length);
            audioSource.clip = inGameMusic[i];
            audioSource.Play();
            Invoke("InGameSound", audioSource.clip.length); //OK ISSO É LOUCO, mas eu chamo esta mesma função no momento em que o audio acabar de tocar.
            //achei em https://www.youtube.com/watch?v=sOLfxVbUrAc;
        }
        else if (music == false)
        {
            audioSource.Stop();
        }
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
            gameOverMenu.SetActive(true); //Ao pausar o menu é ativo.
            Time.timeScale = 0;
        }
        else
        {
            gameOverMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
