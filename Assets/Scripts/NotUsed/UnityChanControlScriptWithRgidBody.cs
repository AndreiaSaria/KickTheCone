
//Comentários japonês foram removidos e o script quase que completamente refeito.
using UnityEngine;

namespace UnityChan
{
    // Se o componente não existir no personagem, o código não funciona.
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class UnityChanControlScriptWithRgidBody : MonoBehaviour
    {
        public float animSpeed = 1.5f;

        public float jumpColliderHeight = 0.5f;

        public float forwardSpeed = 7.0f;

        public float backwardSpeed = 2.0f;

        public float rotateSpeed = 2.0f;

        public float jumpPower = 3.0f;

        public GameObject foot;

        public GameObject knee;

        private float speedYBefore;

        private CapsuleCollider col;

        private Rigidbody rb;

        private Vector3 velocity;

        private Animator anim;

        private LayerMask layerMaskAllButPlayer;

        //Isto é como um dicionário que permite que o unity compare hashs ao invés de strings de caracteres
        //O que acelera o processo de comparação
        static int slideState = Animator.StringToHash("Base Layer.Slide");
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");



        void Start()
        {

            anim = GetComponent<Animator>();
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            layerMaskAllButPlayer = ~LayerMask.GetMask("Player");

            speedYBefore = transform.position.y;

        }


        void FixedUpdate() // update a cada frame de física
        {

            float h = Input.GetAxis("Horizontal");   //Definindo Inputs		
            float v = Input.GetAxis("Vertical");
            anim.SetFloat("Speed", velocity.magnitude); //Enviando Inputs de movimento para o animator
            anim.SetFloat("Direction", h);
            anim.speed = animSpeed; //Velocidade que roda as animações no animator


            velocity = new Vector3(0, 0, v); //A velocidade é no vetor z, mas pode rotacionar em relação a Y*
            velocity = transform.TransformDirection(velocity);

            if (v > 0.1)
            {
                velocity *= forwardSpeed;  	// Velocidade = Velocidade * FowardSpeed
            }
            else if (v < -0.1)
            {
                velocity *= backwardSpeed;
            }

            transform.localPosition += velocity * Time.fixedDeltaTime; //A velocidade segue pelo Delta time da física
            transform.Rotate(0, h * rotateSpeed, 0); //Rotacionando em relação a Y*

            Debug.DrawLine(foot.GetComponent<Transform>().position, (foot.GetComponent<Transform>().position -Vector3.up * 0.2f), Color.blue);
            Debug.DrawLine(knee.GetComponent<Transform>().position, (knee.GetComponent<Transform>().position - Vector3.up * 0.2f), Color.cyan);

            Jump();

            Slide();

            if (Mathf.RoundToInt(transform.position.y) != Mathf.RoundToInt(speedYBefore))
            {
                anim.SetBool("SpeedY", true);
                speedYBefore = transform.position.y;
            }
            else { anim.SetBool("SpeedY", false); }

        }

        private void Slide()
        {
            if (Input.GetButton("Slide")) //Botão pré definido em settings>Input
            {
                if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == locoState || anim.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpState) //Ela só desliza se estiver se movendo
                {//Se estiver no estado de locomoção ou no estado de pular
                    anim.SetBool("Slide", true); //Deslizar
                }
            }
            
            if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == slideState )
            {//Macete: Se você estiver fora do estado de transição, você está na animação de deslizar em si

                if (!anim.IsInTransition(0))
                {
                    //Ajuste numero1
                    col.center = new Vector3(0, 0.26f, 0);
                    col.height = 0.37f;
                    col.radius = 0.33f;

                    //Ajuste numero2
                    //col.center = new Vector3(0, 0.18f, 0); //Ajustando o centro do colisor
                    //col.direction = 2; //Mudando a direção do colisor para a direção z
                    //col.height = 1f; //Ajuste fino
                    //Debug.DrawRay(transform.TransformPoint(col.center), Vector3.up * 1.2f, Color.white); // Mostrando o Ray
                    if (Physics.Raycast(transform.TransformPoint(col.center), Vector3.up, 1.2f, layerMaskAllButPlayer)) // Se colidir acima
                    {
                        anim.SetBool("CollidingOnTop", true);
                    }
                    else
                    {
                        anim.SetBool("CollidingOnTop", false);
                    }
                    anim.SetBool("Slide", false);
                }
                else
                {

                    if (!anim.GetBool("CollidingOnTop"))
                    {
                        ResetCollider();
                    }
                }
            }

        }



        private void Jump()
        {
            //Debug.Log(foot.GetComponent<Transform>().position.y);
            //Debug.DrawRay(foot.GetComponent<Transform>().position, -Vector3.up * 0.2f, Color.red);
            //Debug.DrawRay(knee.GetComponent<Transform>().position, -Vector3.up*0.2f, Color.blue) ;
            if (Input.GetButtonDown("Jump"))
            {
                if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == locoState || anim.GetCurrentAnimatorStateInfo(0).fullPathHash == idleState || anim.GetCurrentAnimatorStateInfo(0).fullPathHash == slideState) 
                {//Se estiver no estado de locomoção, no estado parado ou de deslizar
                    anim.SetTrigger("Jump");
                }

            }

            if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == jumpState){

                

                if (!anim.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse); //Adiciona uma força para cima
                    col.height = jumpColliderHeight; //Definido por tentativa e erro
                    col.center = new Vector3(0, 1.7f, 0);//Definido por tentativa e erro
                    anim.ResetTrigger("Jump");
                }
                //Aqui vem a parte legal. Se o Raycast lançado da ponta do pé ou o Raycast lançado do joelho acertarem um collider
                //resetar o collider. Logo quando a personagem se aproxima do solo o collider reseta, o mesmo funciona para uma superfície
                //que a mesma poderia subir.
                if (Physics.Raycast(foot.GetComponent<Transform>().position, -Vector3.up, 0.1f, layerMaskAllButPlayer) ||
                    Physics.Raycast(knee.GetComponent<Transform>().position, -Vector3.up, 0.1f, layerMaskAllButPlayer))
                {
                    ResetCollider();
                    anim.SetTrigger("Ground");
                }

            }
            else
            {//Colocado aqui para caso esteja em transição.
                ResetCollider();
            }
        }


        private void ResetCollider()
        {//Definição de collider feita por tentativa e erro.
            col.height = 1.5f;
            col.center = new Vector3(0, 0.75f, 0);
            col.radius = 0.2f;
        }

    }
}