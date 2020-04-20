using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todos os movimentos de física tem de estar no FIXEDUPDATE. Assim evitamos erros por mudança de framerate

namespace UnityChanForAndroid
{
    //Ajustar rotação
    // Se o componente não existir no personagem, o código não funciona.
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class UnityChan_For_Infinite_Dodge_Android : MonoBehaviour
    {
        public float animSpeed = 1.5f;
        public float jumpColliderHeight = 0.5f;
        public float forwardSpeed;
        public float moveAroundSpeed = 2.0f;
        public float jumpPower = 3.0f;
        public GameObject foot;
        public GameObject gameController;
        private float speedYBefore;
        private CapsuleCollider col;
        private Rigidbody rb;
        private Animator anim;
        private LayerMask layerMaskAllButPlayer;
        private Vector2 startPos = Vector2.zero;
        private Vector2 touchDirection = Vector2.zero;
        private bool slide; //Como estamos recebendo input de android temos de ter esses bool para controlarmos em fixedupdate
        private bool jump;

        //Para acelerômetro
        private float lowPassValue = 0;

        //Isto é como um dicionário que permite que o unity compare hashs ao invés de strings de caracteres
        //O que acelera o processo de comparação
        static int slideState = Animator.StringToHash("Base Layer.Slide");
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");



        void Start()
        {
            slide = false;
            jump = false;

            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            layerMaskAllButPlayer = ~LayerMask.GetMask("Player");

            speedYBefore = transform.position.y;

            //Para acelerômetro
            lowPassValue = Input.acceleration.x;
        }


        void Update()
        {
            //Acelerômetro
            lowPassValue = LowPassFilterAccelerometer(lowPassValue);

            float LowPassFilterAccelerometer(float prevValue)
            {
                float newValue = Mathf.Lerp(prevValue, Input.acceleration.x, (1.0f / 10.0f) / 1.0f); //Modifiquei o valor de 60 para 10
                return newValue;
            }
            float h = lowPassValue;
          
            anim.SetFloat("Speed", rb.velocity.magnitude); //Enviando Inputs de movimento para o animator
            anim.SetFloat("Direction", h);
            anim.speed = animSpeed; //Velocidade que roda as animações no animator*

            transform.localPosition += transform.right * h * moveAroundSpeed * Time.deltaTime; //Não quero nada além dela movendo a direita e a esquerda, então aqui não rotacionamos


            AndroidTouchesInput();
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

        }
        private void FixedUpdate()
        {
            //rb.velocity = transform.forward * forwardSpeed;  //Velocidade (meio não realística)
            if (rb.velocity.magnitude < 8)
            {
                rb.AddForce(transform.forward * forwardSpeed * (1 + Time.fixedDeltaTime) * 5, ForceMode.Impulse); //velocidade mais realística
            }

            if (jump)
            {
                Jump();
            }
            if (slide)
            {
                Slide();
            }

        }

        private void LateUpdate()
        {
            //periodTime += Time.deltaTime;
            //if (periodTime > periodToCheck)
            //{
            //if (System.Math.Round(transform.position.y, 1) != System.Math.Round(speedYBefore, 1)) //Solução que encontrei em
            //https://answers.unity.com/questions/50391/how-to-round-a-float-to-2-dp.html  
            //if (Mathf.RoundToInt(transform.position.y) != Mathf.RoundToInt(speedYBefore)) //Para evitar erros chatos


            if (Mathf.Abs(rb.worldCenterOfMass.y) > 1f) //Ok tentei mil coisas mas essa é a que funcionou melhor. Como não subimos nada o centro de massa se mantém estável durante a estrada.
                                                        //Mas caso tenhamos de subir fazemos uma variável e conferimos se ela alterou acima de 1f;
            {
                anim.SetBool("SpeedY", true);
                speedYBefore = transform.position.y;
            }
            else { anim.SetBool("SpeedY", false); }
            //}
        }

        private void AndroidTouchesInput()
        {
            if (Input.touchCount > 0)
            {
                Touch theTouch = Input.GetTouch(0);
                switch (theTouch.phase)
                {
                    case TouchPhase.Began:
                        startPos = theTouch.position;
                        break;

                    case TouchPhase.Moved:
                        touchDirection = theTouch.position - startPos;
                        break;

                    case TouchPhase.Ended:
                        if (touchDirection.y > Mathf.Abs(touchDirection.x) && touchDirection.y > 0)
                        {
                            jump = true;
                        }
                        else if (Mathf.Abs(touchDirection.y) > Mathf.Abs(touchDirection.x) && touchDirection.y < 0)
                        {
                            slide = true;
                        }
                        else if (touchDirection.x > Mathf.Abs(touchDirection.y) && touchDirection.x > 0)
                        {
                            transform.Rotate(0, transform.rotation.y + 90f, 0, Space.World); //Transformei em intger pois pelo código as variáveis float podem ter erros que acumulam
                                                                                             //Para ajustar erros de conversão.
                            Vector3 vec = transform.eulerAngles;
                            vec.x = Mathf.Round(vec.x / 90) * 90;
                            vec.y = Mathf.Round(vec.y / 90) * 90;
                            vec.z = Mathf.Round(vec.z / 90) * 90;
                            transform.eulerAngles = vec;
                        }
                        else if (Mathf.Abs(touchDirection.x) > Mathf.Abs(touchDirection.y) && touchDirection.x < 0)
                        {
                            transform.Rotate(0, transform.rotation.y - 90f, 0, Space.World);
                            //Para ajustar erros de conversão.
                            Vector3 vec = transform.eulerAngles;
                            vec.x = Mathf.Round(vec.x / 90) * 90;
                            vec.y = Mathf.Round(vec.y / 90) * 90;
                            vec.z = Mathf.Round(vec.z / 90) * 90;
                            transform.eulerAngles = vec;
                        }
                        
                        break;
                }
            }
        }

        private void Slide()
        {
            //if (Input.GetButton("Slide")) //Botão pré definido em settings>Input
            //if(Input.mouseScrollDelta.y < 0 )
            //{
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == locoState || anim.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpState) //Ela só desliza se estiver se movendo
                {//Se estiver no estado de locomoção ou no estado de pular
                    anim.SetBool("Slide", true); //Deslizar
                }
            //}

            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == slideState)
            {//Macete: Se você estiver fora do estado de transição, você está na animação de deslizar em si

                if (!anim.IsInTransition(0))
                {
                    //Ajuste numero1
                    col.center = new Vector3(0, 0.26f, 0);
                    col.height = 0.37f;
                    col.radius = 0.33f;

                    anim.SetBool("Slide", false);
                    slide = false;
                }

            }

        }

        private void Jump()
        {
            //if (Input.GetButton("Jump"))
            //if(Input.mouseScrollDelta.y > 0)
            //{
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == locoState || anim.GetCurrentAnimatorStateInfo(0).fullPathHash == slideState)
                {//Se estiver no estado de locomoção, no estado parado ou de deslizar
                    anim.SetTrigger("Jump");
                rb.AddForce(Vector3.up * jumpPower * (1 + Time.fixedDeltaTime), ForceMode.Impulse); //Adiciona uma força para cima não esquecer que deve ser de acordo com o FIXED delta time
                col.height = jumpColliderHeight; //Definido por tentativa e erro
                col.center = new Vector3(0, 1.7f, 0);//Definido por tentativa e erro
            }

            //}

            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpState)
            {

                if (!anim.IsInTransition(0))
                {//Macete: Se você estiver fora do estado de transição, você está na animação de deslizar em si

                    anim.ResetTrigger("Jump");


                }
                //Aqui vem a parte legal. Se o Raycast lançado da ponta do pé acertar um collider
                //resetar o collider. Logo quando a personagem se aproxima do solo o collider reseta, o mesmo funciona para uma superfície
                //que a mesma poderia subir.
                //if (Physics.Raycast(foot.transform.position, -Vector3.up, 0.5f, layerMaskAllButPlayer))
                //{
                //    ResetCollider();
                //    anim.SetTrigger("Ground");
                //}

            }
            else
            {// Neste caso, como definimos que o movimento em y reajusta o collider, não precisamos mais do raycast.
                ResetCollider();
                //anim.ResetTrigger("Ground");
                jump = false;
            }
        }

        private void ResetCollider()
        {//Definição de collider feita por tentativa e erro.
            col.height = 1.5f;
            col.center = new Vector3(0, 0.75f, 0);
            col.radius = 0.2f;
        }


        //private void OnTriggerEnter(Collider other)
        //{
        //    gameController.SendMessage("CollidingSetting", other); //enviando o collider acertado

        //}

        private void OnCollisionEnter(Collision collision) //Aqui enviamos mensagem para o GameController e para o animator, assim podemos ajustar a hora em que ela perde.
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Out" || collision.gameObject.tag == "OutOnWater" || collision.gameObject.tag == "Point")
            {
                gameController.SendMessage("CollidingSetting", collision);
            }
        }

        public void TouchAnimation(int t)
        {
            switch (t)
            {
                case 1:
                    anim.SetTrigger("HitObstacle");
                    break;
                case 2:
                    anim.SetTrigger("FallWater");
                    break;
                case 3:
                    anim.SetTrigger("FallOut");
                    break;
            }

        }
    }

}

