using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour {
    public Transform Player;
    public Module lastInstanciated;
    public float distanceThreshold = 20;

    public int MaxActiveModules = 8;
    public List<Module> TempModules = new List<Module> ();

    private Module[] floorPrefabs;
    private float rotationAngle;
    private Direction? forbidenDirection = null;

    private void Start () {
        floorPrefabs = Resources.LoadAll<Module> ("Platforms/");
        rotationAngle = 0;
    }

    private void Update () {

        if (Vector3.Distance (lastInstanciated.transform.position, Player.transform.position) < distanceThreshold) {
            int i = 0;

            do {
                i = Random.Range (0, floorPrefabs.Length);
            } while (floorPrefabs[i].Direction == forbidenDirection);

            Vector3 spawnPosition = lastInstanciated != null ? lastInstanciated.End.position : Vector3.zero;
            Module newModule = GameObject.Instantiate (floorPrefabs[i], spawnPosition, Quaternion.Euler (-90, 0, 0));
            newModule.transform.position -= newModule.Start.localPosition;

            if (lastInstanciated != null) {
                if (lastInstanciated.Direction == Direction.LEFT) {
                    rotationAngle -= 90;
                    forbidenDirection = Direction.LEFT;
                } else if (lastInstanciated.Direction == Direction.RIGHT) {
                    rotationAngle += 90;
                    forbidenDirection = Direction.RIGHT;
                }
                newModule.transform.RotateAround (newModule.Start.position, new Vector3 (0, 1, 0), rotationAngle);
            }

            TempModules.Add (newModule);
            lastInstanciated = newModule;
        }

        DestroyOldModules ();

    }
    private void DestroyOldModules () {
        if (TempModules.Count >= MaxActiveModules) {
            GameObject.Destroy (TempModules[0].gameObject);
            TempModules.RemoveAt (0);
        }
    }

    private void SpawnNewModule () {
        int i = 0;

        do {
            i = Random.Range (0, floorPrefabs.Length);
        } while (floorPrefabs[i].Direction == forbidenDirection);

        Vector3 spawnPosition = lastInstanciated != null ? lastInstanciated.End.position : Vector3.zero;
        Module newModule = GameObject.Instantiate (floorPrefabs[i], spawnPosition, Quaternion.Euler (-90, 0, 0));
        newModule.transform.position -= newModule.Start.localPosition;

        if (lastInstanciated != null) {
            if (lastInstanciated.Direction == Direction.LEFT) {
                rotationAngle -= 90;
                forbidenDirection = Direction.LEFT;
            } else if (lastInstanciated.Direction == Direction.RIGHT) {
                rotationAngle += 90;
                forbidenDirection = Direction.RIGHT;
            }
            newModule.transform.RotateAround (newModule.Start.position, new Vector3 (0, 1, 0), rotationAngle);
        }

        TempModules.Add (newModule);
        lastInstanciated = newModule;
    }
}