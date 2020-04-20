using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityChan;

public class GameController : MonoBehaviour
{
    private GameObject player; //Para a localização do player
    private Camera cameraPlayer; //Para ajustar a câmera ao player perder
    Coroutine routineGoing; //Para conferir a coroutine rodando
    private int routineNumber; //Para definir o tipo de forma em que o personagem perdeu
    private bool fall;
    public UnityChan_For_Infinite_Dodge playerScript; //Script que recebe do player para ajustar a animação
    public GameObject waterSplash; //Partículas
    public GameMenuManagerPC menu;
    public GroundCreator groundStanding;
    public float pushBackForce = 200f; //Força que a empurra para trás
    public float cameraPushBack = 2f; //Distância da câmera após a morte
    public int frameRate;
    public AudioClip[] coneHit;
    public AudioClip[] coneTouch;


    void Start()
    {
        Application.targetFrameRate = frameRate; //Seria para melhor performance
        player = GameObject.FindWithTag("Player"); //Encontrando o player, pois podem ser modelos diferentes
        cameraPlayer = player.GetComponentInChildren<Camera>(); //A câmera está como filha do player
        fall = false;
    }

    void Update()
    { 
        //Para ajustar a velocidade do player, aqui definimos o quão rápido ela deve aumentar.
        if (Time.timeSinceLevelLoad > 1)
        {
            if (playerScript.forwardSpeed < 6)
            {
                playerScript.forwardSpeed = playerScript.forwardSpeed + (Time.timeSinceLevelLoad / 100);
            }
        }

        WrongWay();
        Falling();
    }

    public void Falling() //O player decidiu voltar ao primeiro track renderizado e cair.
    {
        if (!fall && Mathf.Abs(player.GetComponent<Rigidbody>().worldCenterOfMass.y) > 4) //Aqui de novo sabemos que meus tracks se mantêm a uma certa altura, se cair além disso ela saiu do track; 
        {
            //Debug.Log("Looks like you fell a rabbit hole huh");
            menu.SetActivity(4);
            fall = true;
        }
    }

    public void WrongWay() //O player está indo no sentido errado.
    {
        if (groundStanding.PlayerRayCheck() != null && player.transform.forward.z == (-groundStanding.PlayerRayCheck().Start.forward.z))
        {
            //Debug.Log("Facing Wrong Way");
            menu.Score(6);
        }
    }

    public void CollidingSetting(Collision collision)
    {
        //Aqui estão os casos de colisão. Estão aqui para tanto limpar o código da Unity chan como para aplicarmos a outros players.


        if (collision.collider.gameObject.tag == "Out")
        {
            playerScript.enabled = false; //Para parar de receber input e se mover.
            playerScript.SendMessage("TouchAnimation", 3); // Case 3 caiu fora.

            //Ajuste da câmera. Ela tem de estar na mesma posição x do player, subir um pouco(y), e se mover para trás um pouco(z). Além disso ela tem de rotacionar para olhar para o player!
            cameraPlayer.transform.localPosition = new Vector3(cameraPlayer.transform.localPosition.x, cameraPlayer.transform.localPosition.y + cameraPushBack, cameraPlayer.transform.localPosition.z - cameraPushBack);
            cameraPlayer.transform.LookAt(player.transform);

            routineNumber = 1; //Número de atividade que será recebida pelo código de Menu. Lá temos um Case sendo o 1 - ela caiu fora
            this.RestartCoroutine(WaitMenu(), ref routineGoing); // Start and stopCoroutine não funcionavam para meu propósito então procurei na internet e achei esse código adicional para o Unity.
            //https://forum.unity.com/threads/how-to-cancel-and-restart-a-coroutine.435493/

        }
        else if (collision.collider.gameObject.tag == "OutOnWater")
        {
            playerScript.enabled = false;
            Instantiate(waterSplash, player.transform); //Neste caso iniciamos as partículas de água.
            //Com elas o som se inicia
            
            playerScript.SendMessage("TouchAnimation", 2); //Case 2 cair na água
            cameraPlayer.transform.localPosition = new Vector3(cameraPlayer.transform.localPosition.x, cameraPlayer.transform.localPosition.y + cameraPushBack, cameraPlayer.transform.localPosition.z - cameraPushBack);
            cameraPlayer.transform.LookAt(player.transform);


            routineNumber = 2;
            this.RestartCoroutine(WaitMenu(), ref routineGoing); 

        }
        else if(collision.collider.gameObject.tag == "Enemy")
        {
            playerScript.enabled = false; 
            player.transform.rotation.SetLookRotation(collision.GetContact(collision.contactCount - 1).point.normalized); //Fazer com que o player olhe para o ponto de colisão. (não funciona tããão bem)
                                                                                                                          
            if (!Physics.Raycast(player.transform.position, Vector3.back, 2)) //Para evitar dela entrar em superfícies. Confere se tem algo atrás dela antes de fazer o impulso.
            {
                player.GetComponent<Rigidbody>().AddForce(-collision.GetContact(collision.contactCount - 1).point.normalized * pushBackForce, ForceMode.Impulse);
            }
 
            cameraPlayer.transform.localPosition = new Vector3(cameraPlayer.transform.localPosition.x, cameraPlayer.transform.localPosition.y + cameraPushBack, cameraPlayer.transform.localPosition.z - cameraPushBack);
            cameraPlayer.transform.LookAt(player.transform);

            playerScript.SendMessage("TouchAnimation", 1); //Ativando a animação de cair, case 1 encostar no enemy

            routineNumber = 3;
            this.RestartCoroutine(WaitMenu(), ref routineGoing);

        }
        else if(collision.collider.gameObject.tag == "Point")
        {
            AnimatorClipInfo[] info = player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0); //Aqui pegamos a informação do animator do player, o current animator clip só envia arrays então recebemos como um.
            if (info[0].clip.name == "SLIDE00") //Mas sabemos que apenas o primeiro termo do array é a posição de animação do momento. Aqui conferimos se é o slide.
            {
                collision.collider.gameObject.GetComponent<Rigidbody>().AddForce(collision.GetContact(collision.contactCount - 1).point.normalized * 10, ForceMode.Impulse);
                //Se for, fazemos o objeto com que ela colidiu sair voando e adicionamos score.
                menu.Score(2);
                //Além de adicionar o som ao objeto
                if (collision.collider.gameObject.GetComponent<AudioSource>().enabled)
                {
                    int i = Random.Range(0, coneHit.Length);
                    collision.collider.gameObject.GetComponent<AudioSource>().clip = coneHit[i];
                    collision.collider.gameObject.GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                //Caso contrário temos mensagens no score para explicar ao player que é melhor deslizar.
                menu.Score(3);
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

    IEnumerator WaitMenu() //Para que a animação de cair/ bater se complete antes do menu aparecer.
    {
        
        yield return new WaitForSeconds(2);
        menu.SetActivity(routineNumber);
        
    }
}
