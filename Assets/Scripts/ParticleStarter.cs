using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticleStarter : MonoBehaviour
{
    private ParticleSystem myParticleEmitter;
    public Collider collidertouched;
    void Awake()
    {
        myParticleEmitter = GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider collidertouched)
    {
        if(collidertouched.gameObject.tag == "Player")
        {
            myParticleEmitter.Play(true);
        }
    }
}
