﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.AI; //Para NavmeshBuilder
//Código refeito em 03/03/2020 com base no código Creator enviado pelo e-tutor. Atenção para raycast.
//Código refeito novamente em 26/03/2020, aqui ajustamos o problema do raycast e retornamos o track em que ela toca para outro script

public class GroundCreator : MonoBehaviour
{
    //public int maxRenderedTracks = 9;
    public int tracksBeforePlayer = 3; //Quantidade de tracks que terão atrás do player.
    public float distanceForRendering; // Distância do player até o proximo track 
    public bool mushroombuild;

    //Module é um outro script que serve para nos ajudar dizendo a direção para onde os tracks vão, inclusive marcando o começo e o final do track.

    private List<Module> tracks = new List<Module>();
    private List<Module> renderedTracks = new List<Module>(); //Não se esqueça de que caso o list seja private/protected fazer um iniciador!!!;
    private Module trackTrick;
    private GameObject player; //Para a localização do player
    private Transform associateNextTrackHere; //Onde colocaremos o próximo track
    private Direction? forbidenDirection = null;

    void Start()
    {
        
        tracks = FolderFinder.FindingTracksModule();//Coisa de gente preguiçosa (código para achar os tracks na pasta)

        player = GameObject.FindWithTag("Player"); //Encontrando o player, pois podem ser modelos diferentes

        trackTrick = GameObject.Instantiate(tracks[Random.Range(0,tracks.Count)], new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 1.5f), player.transform.rotation);

        renderedTracks.Add(trackTrick);
        associateNextTrackHere = player.transform;

    }


    private void Update()//antes era fixedupdate, não acredito que seja necessário pois não fazemos operações físicas
    {

        if (distanceForRendering > Vector3.Distance(player.transform.position, renderedTracks[renderedTracks.Count - 1].transform.position))
        {
            int i = 0;
            do
            {
                i = Random.Range(0, tracks.Count);

                if (trackTrick != null)
                {
                    if (trackTrick.Direction == Direction.LEFT)
                    {
                        forbidenDirection = Direction.LEFT;
                    }
                    else if (trackTrick.Direction == Direction.RIGHT)
                    {
                        forbidenDirection = Direction.RIGHT;
                    }

                }
            } while (tracks[i].Direction == forbidenDirection);

            associateNextTrackHere = renderedTracks[renderedTracks.Count - 1].End; //O track sempre tem de ser colocado após o ultimo renderizado. Lembrando que em C# as listas/vetores começam em 0

            trackTrick = GameObject.Instantiate(tracks[i], associateNextTrackHere.transform.position, associateNextTrackHere.transform.rotation);
            //if (mushroombuild)
            //{
            //    NavMeshBuilder.BuildNavMesh(); //ACTIVATE FOR MUSHROOM
            //}


            renderedTracks.Add(trackTrick); //Adicionando ao manager de tracks
        }

        if (PlayerRayCheck() != null && PlayerRayCheck() == renderedTracks[tracksBeforePlayer])
        {
            GameObject.Destroy(renderedTracks[0].gameObject);
            renderedTracks.RemoveAt(0);
        }
            //Código para caso queiramos pelo numero de tracks a renderizar
            //if (maxRenderedTracks == renderedTracks.Count)
            //{
            //    GameObject.Destroy(renderedTracks[0].gameObject);
            //    renderedTracks.RemoveAt(0);
            //}


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

  

    public Module RenderedTracksGetLast()
    {
        if (renderedTracks.Count > 1){
            return renderedTracks[renderedTracks.Count - 1];
        }
        else
        {
            return renderedTracks[0];
        }

    }

    //private Transform FindByName(GameObject theStandingGameObject,string whatTofind)//Esta função serve para acharmos o gameObject que criei dentro de todos os prefabs de track para encaixar o fim / obstáculo.
    //{
    //    Transform answer = null; //No caso de não acharmos o nome
    //    foreach (Transform t in theStandingGameObject.GetComponentsInChildren<Transform>()) //Pegando qual o transform do End (o que seria o fim do prefab) ou o transform do obstacleplacement
    //    {
    //        if (t.gameObject.name.ToString() == whatTofind)
    //        {
    //            answer = t;
    //        }

    //    }

    //    return answer;
    //}
}

