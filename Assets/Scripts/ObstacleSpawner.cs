//Código feito pelo e-tutor Pedro Silva em 28/01/2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]

public class ObstacleSpawner : MonoBehaviour
{
    private List<GameObject> obstacles = new List<GameObject>();
    private BoxCollider colisor;
    private GameObject newObject;
    private float chanceOfSpawn = 0.8f; //Essa chance de spawn pode ser ajustada deve ser um valor entre 0 e 1
    private GameController controller1;
    private GameControllerForAndroid controller2;

    private void OnEnable() //Para quando o track for ativo fazer o spawn de objetos
    {//https://answers.unity.com/questions/385639/how-to-spawn-prefabs-with-percent-random.html

        controller1 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>(); //Se não achar esse controller então entrar para o de android
        if(controller1 == null)
        {
            controller2 = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerForAndroid>();
            chanceOfSpawn = controller2.ObstacleSpawnChance();
        }
        else
        {
            chanceOfSpawn = controller1.ObstacleSpawnChance();
        }


        var chancesBool = (Random.value <= chanceOfSpawn); //Valor em porcentagem
        if (chancesBool == true)
        {
            obstacles = FolderFinder.FindingObstacles();
            colisor = GetComponent<BoxCollider>();

            SpawnObstacle();
        }
    }

    private void OnDisable() //Para quando o track for desativado destruir o objeto anterior
    {
        if (newObject)
        {
            Destroy(newObject);
        }

    }


    private void SpawnObstacle()
    {
        float newX = Random.Range(colisor.bounds.center.x - 1.6f, colisor.bounds.center.x + 1.6f);
        float newZ = Random.Range(colisor.bounds.center.z - 1.8f, colisor.bounds.center.z + 1.8f);
        //float newX = Random.Range(colisor.bounds.min.x, colisor.bounds.max.x); //Valor aleatório entre x min e x max do boxcollider
        //float newZ = Random.Range(colisor.bounds.min.z, colisor.bounds.max.z); //Valor aleatório entre z min e z max do boxcollider
        Vector3 newPos = new Vector3(newX, colisor.bounds.max.y, newZ);  //Como o obstáculo irá ficar no topo da caixa, (y max)!!!
                                                                         //Provavelmente é necessário aplicar um offset se o pivot não estiver na base do objeto
        int i = Random.Range(0, obstacles.Count);
        newObject = Instantiate(obstacles[i], newPos, transform.rotation, transform); // Aqui damos spawn no objeto escolhido na posição randomica, com a rotação identidade e o transform do objeto pai.
        int j = obstacles[i].layer;
        if (j == 13)
        {
            newObject.transform.localScale = new Vector3(0.1f / transform.localScale.x, 0.1f / transform.localScale.y, 0.1f / transform.localScale.z);
        }
        else if(j == 14)
        {
            newObject.transform.localScale = new Vector3(1f / transform.localScale.x, 0.8f / transform.localScale.y, 1f / transform.localScale.z);
        }
        else if(j == 15)
        {
            newObject.transform.localScale = new Vector3(1.8f / transform.localScale.x, 1.8f / transform.localScale.y, 1.8f / transform.localScale.z);
            newObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 180f), 0f);
        }
        
        //newObject.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z); //Ignorar a scale do objeto pai
        //newObject.transform.localScale = new Vector3(1.2f / transform.localScale.x, 0.8f / transform.localScale.y, 1.2f / transform.localScale.z);
    }
}
