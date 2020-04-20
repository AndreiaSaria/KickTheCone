using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Jump does not work.

public class MushroomRunNavMesh : MonoBehaviour
{
    public GroundCreator instanceGround;
    public int delayCheck;
    //public AudioClip walk;
    public AudioClip damage;
    private float check = 0;
    private Animation animationMushroom;
    private SphereCollider colliderMushroom;
    private Rigidbody rb;
    private NavMeshAgent mushroomNav;
    private Vector3 theTransform;


    void Start()
    {
        animationMushroom = GetComponent<Animation>();
        colliderMushroom = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        mushroomNav = GetComponent<NavMeshAgent>();
        theTransform = this.transform.position;
        animationMushroom.Play("Run");
    }

    void Update()
    {
        Module lastTrack = instanceGround.RenderedTracksGetLast();

        mushroomNav.SetDestination(lastTrack.End.transform.position);

        if (!animationMushroom.isPlaying)
        {
            animationMushroom.Play("Run");
            GetComponent<AudioSource>().Play();
        }

        check += Time.deltaTime;

        if(Mathf.Floor(check) == delayCheck)
        {
            
            if (Mathf.RoundToInt(theTransform.x) == Mathf.RoundToInt(this.transform.position.x) & Mathf.RoundToInt(theTransform.z) == Mathf.RoundToInt(this.transform.position.z))
            {
                if(instanceGround.PlayerRayCheck() != null)
                {
                    this.transform.position = instanceGround.PlayerRayCheck().End.position ;
                    Debug.Log("Entrei");
                }
                
            }
            theTransform = this.transform.position;
            check = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Point")
        {
            animationMushroom.Play("Death");
            GetComponent<AudioSource>().PlayOneShot(damage);
            //Debug.Log("I took damage");
        }

    }

}

