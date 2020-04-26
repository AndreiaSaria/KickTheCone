using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script com partes do UnityChanForAndroidDodge
//Apenas para a cutscene
public class UnityChanControlScene : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    private float speedYBefore;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        speedYBefore = transform.position.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        anim.SetFloat("Speed", rb.velocity.magnitude); //Enviando Inputs de movimento para o animator
        anim.speed = 1.2f; //Velocidade que roda as animações no animator

        if (Mathf.Abs(rb.worldCenterOfMass.y) > 1f) 
        {
            anim.SetBool("SpeedY", true);
            speedYBefore = transform.position.y;
        }
        else { anim.SetBool("SpeedY", false); }


        rb.AddForce(transform.forward * 2 * (1 + Time.fixedDeltaTime) * 8, ForceMode.Impulse); //velocidade mais realística
    }
}
