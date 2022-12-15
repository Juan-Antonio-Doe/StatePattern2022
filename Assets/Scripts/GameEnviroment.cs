using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class GameEnviroment {

    private static GameEnviroment instance;

    List<GameObject> checkpoints = new List<GameObject>();
    List<GameObject> safepoints = new List<GameObject>();
    
    // Getters
    public List<GameObject> Checkpoints { get => checkpoints; }
    public List<GameObject> Safepoints { get => safepoints; }

    public static GameEnviroment Singleton {
        get {
            if (instance == null) {
                instance = new GameEnviroment();
                instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();
                instance.Safepoints.AddRange(GameObject.FindGameObjectsWithTag("Safepoint"));
                //instance.safepoints = instance.safepoints.OrderBy(safepoint => safepoint.name).ToList();
            }
            return instance;
        }
    }

    public GameObject GetRandomCheckpointLocation() {
        return checkpoints[Random.Range(0, checkpoints.Count)];
    }
}
