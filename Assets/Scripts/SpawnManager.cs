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
    public GameObject zone;
    // Rock card prefab
    public GameObject rockCardPrefab;
    // Draw card prefab
    public GameObject drawThreeCardPrefab;
    // Ground enemy prefab
    public GameObject groundEnemyPrefab;
    // Flying enemy prefab
    public GameObject flyingEnemyPrefab;
    // Reference the Time Manager
    private TimeManager timeManager;

    // Initialise
    public void Start()
    {
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
        string floor = (focusedFloor == Floor.BottomFloor) ? "bottom" : "top";
        Debug.Log("spawning: " + floor);
    }
}