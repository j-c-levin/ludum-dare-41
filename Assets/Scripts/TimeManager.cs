using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // The event delegate for ticks on the top floor
    public delegate void TopFloorTickDelegate();
    // The event listener for ticks for the top floor
    public TopFloorTickDelegate topFloorTickDelegate;
    // The event delegate for ticks on the bottom floor
    public delegate void BottomFloorTickDelegate();
    // The event listener for ticks for the bottom floor
    public BottomFloorTickDelegate bottomFloorTickDelegate;
    // The range of time in which a tick can occur, i.e. +/- 1 second either way
    private float rangeOfPossibleTicks = 1f;
    // The beginning average time per tick at the start of the game
    private float averageTickTime = 2f;
    // Commented for now just to get stuff working
    // // How long should each level be before stepping up to the next
    // private float durationOfLevel;
    // // How much shorter should the average time per tick get per step
    // private float stepAmountOfNextLevel;
    // The current top floor time to the next tick
    private float topFloorTimeToNextTick = 0;
    // The current top floor time since last tick
    private float topFloorTimeSinceLastTick = 0;
    // The current bottom floor time to the next tick
    private float bottomFloorTimeToNextTick = 0;
    // The current bottom floor time since last tick
    private float bottomFloorTimeSinceLastTick = 0;

    // Initialise everything
    public void Start()
    {
        // Set time to next ticks
        topFloorTimeToNextTick = getRandomTickTIme();
		bottomFloorTimeToNextTick = getRandomTickTIme();
    }

    // Update loop which manages ticks
    public void Update()
    {
		
    }

	private void updateTickTime() 
	{
		
	}

    private float getRandomTickTIme()
    {
        float minTime = (averageTickTime - rangeOfPossibleTicks);
        float maxTime = (averageTickTime + rangeOfPossibleTicks);
        return Random.Range(minTime, maxTime);
    }
}