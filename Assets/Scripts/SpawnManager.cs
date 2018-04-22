using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
public class SpawnManager : MonoBehaviour
{
    // Does this spawn manager focus on the top or bottom floor
    public enum Floor
    {
        TopFloor,
        BottomFloor
    }
    public Floor focusedFloor;

    // Zone prefab
    public GameObject zonePrefab;
    // Rock card prefab
    public GameObject rockCardPrefab;
    // Ground enemy prefab
    public GameObject enemyPrefab;
    // Array to randomly pick something to spawn
    private GameObject[] spawnables;
    // Reference the Time Manager
    private TimeManager timeManager;
    // Spawning positions for zones
    private float zoneXSpawn = 30;
    private float bottomFloorYSpawn = 0;
    private float topFloorYSpawn = 3.60f;
    // Spawn track heights
    private float bottomTrackSpawn = -1;
    private float topTrackSpawn = 0;

    // Initialise
    public void Start()
    {
        // Set spawnables array
        spawnables = new GameObject[] { rockCardPrefab, enemyPrefab };
        // Set reference
        timeManager = GetComponent<TimeManager>();
        // Set up handler
        /* 
			Cannot use a ternary here, trust me, I tried. 
			
			Reason being is that delegates are null until they have a listener, which is why before you use one you must check if it's null. 
			
			So trying to do something like TimeManager.TickDelegate d = (isBottomFloor) ? bottomFloorTickDelegate : etc. fails because the delegates are null, thus d is null and cannot be subscribed to.

			The more you know :)
		*/
        if (focusedFloor == Floor.BottomFloor)
        {
            timeManager.bottomFloorTickDelegate += tickDelegateHandler;
        }
        else
        {
            timeManager.topFloorTickDelegate += tickDelegateHandler;
        }
    }

    // An event handler that when the timer manager ticks it spawns
    private void tickDelegateHandler()
    {
        // Pick thing to spawn
        GameObject objectToSpawn = spawnables[Random.Range(0, spawnables.Length)];
        // Pick the height to spawn the zone based on whether it's top or bottom floor
        float floorSpawnHeight = (focusedFloor == Floor.BottomFloor) ? bottomFloorYSpawn : topFloorYSpawn;
        // Spawn the zone at the right height
        GameObject newZone = Instantiate(zonePrefab, new Vector2(zoneXSpawn, floorSpawnHeight), Quaternion.identity);
        // Spawn the object that is to be interacted with
        GameObject newObject = Instantiate(objectToSpawn, Vector2.zero, Quaternion.identity);
        // Parent the object to the zone
        newObject.transform.SetParent(newZone.transform);
        // Reset the scale of the object
        newObject.transform.localScale = Vector2.one;
        // Randomly pick a track for the object
        float trackHeight = (Random.Range(0, 1f) < 0.5f) ? bottomTrackSpawn : topTrackSpawn;
        // Reset the postion of the object at the correct track
        newObject.transform.localPosition = new Vector2(0, trackHeight);
    }
}