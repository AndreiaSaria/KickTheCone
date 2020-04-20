using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MushroonRun
{

    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class MushroomRun : MonoBehaviour
    {
        private Animation animationMushroom;
        private SphereCollider colliderMushroom;
        private Rigidbody rb;
        private LayerMask allButMushroom;

        private Vector3 directionFaced;
        public float speed = 2;
        public float moveAroundSpeed = 2;

        void Start()
        {
            animationMushroom = GetComponent<Animation>();
            colliderMushroom = GetComponent<SphereCollider>();
            rb = GetComponent<Rigidbody>();

            directionFaced = new Vector3(0, 0, 1);
            allButMushroom = ~LayerMask.GetMask("Mushroom");

        }

        void FixedUpdate()
        {

            //transform.localPosition += directionFaced * speed * Time.deltaTime;
            rb.velocity = transform.forward * speed;


            animationMushroom.Play("Run");

            if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(1, 0.8f, 1)), 1.5f, allButMushroom, QueryTriggerInteraction.Ignore)) //ignorando o meu trigger para virar
            {
                transform.localPosition += (-transform.right) * moveAroundSpeed * Time.deltaTime;
                rb.velocity = transform.forward * speed;
            }
            else if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(-1, 0.8f, 1)), 1.5f, allButMushroom, QueryTriggerInteraction.Ignore))
            {
                transform.localPosition += transform.right * moveAroundSpeed * Time.deltaTime;
                rb.velocity = transform.forward * speed;
            }
            else if (Physics.Raycast(transform.position, Vector3.forward, 1f, allButMushroom, QueryTriggerInteraction.Ignore))
            {
                transform.localPosition += transform.right * moveAroundSpeed * Time.deltaTime;
                rb.velocity = transform.forward * speed;
            }



            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(1, 0.8f, 1)), Color.green);
            Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(-1, 0.8f, 1)), Color.green);

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "TurnL")
            {
                transform.Rotate(0, Mathf.RoundToInt(transform.rotation.y - 90f), 0, Space.World);

            }
            else if(other.tag == "TurnR")
            {
                transform.Rotate(0, Mathf.RoundToInt(transform.rotation.y + 90f), 0, Space.World);
            }
        }
    }
}

