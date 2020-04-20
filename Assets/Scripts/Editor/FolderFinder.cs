using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //Para AssetDatabase

public class FolderFinder : MonoBehaviour
{
    //Função apenas porque tenho preguiça
    public static List<GameObject> FindingObstacles()
    {
        List<GameObject> obstacles = new List<GameObject>();

        //string[] obstaclenames = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/Prefabs/Obstacle Prefabs" });//Encontrar o nome dos Gameobjects no folder

        //for (int i = 0; i < obstaclenames.Length; i++) //Para a quantidade de obstáculos encontrado
        //{
        //    string TheAssetPath = AssetDatabase.GUIDToAssetPath(obstaclenames[i]); //Pegar o nome que liga a localização
        //    obstacles.Add(AssetDatabase.LoadAssetAtPath<GameObject>(TheAssetPath)); //Fazer load do objeto na lista
        //}
        foreach (GameObject g in Resources.LoadAll("Prefabs/Obstacle Prefabs", typeof(GameObject)))
        {
            obstacles.Add(g);
        }

        return obstacles;
    }

    public static List<GameObject> FindingTracksGameObject()
    {
        List<GameObject> tracks = new List<GameObject>();

        //string[] tracksnames = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/Prefabs/Track Prefabs" });

        //for (int i = 0; i < tracksnames.Length; i++)
        //{
        //    string TheAssetPath = AssetDatabase.GUIDToAssetPath(tracksnames[i]);
        //    tracks.Add(AssetDatabase.LoadAssetAtPath<GameObject>(TheAssetPath));
        //}
        foreach(GameObject g in Resources.LoadAll("Prefabs/Track Prefabs", typeof(GameObject)))
        {
            tracks.Add(g);
        }

        return tracks;

    }

    public static List<Module> FindingTracksModule()
    {
        List<Module> tracks = new List<Module>();

        //string[] tracksnames = AssetDatabase.FindAssets("t:GameObject", new string[] { "Assets/Prefabs/Track Prefabs" });

        //for (int i = 0; i < tracksnames.Length; i++)
        //{
        //    string TheAssetPath = AssetDatabase.GUIDToAssetPath(tracksnames[i]);
        //    tracks.Add(AssetDatabase.LoadAssetAtPath<Module>(TheAssetPath));
        //}

        foreach(Module m in Resources.LoadAll("Prefabs/Track Prefabs", typeof(Module)))
        {
            tracks.Add(m);
        }

        return tracks;
    }
}
