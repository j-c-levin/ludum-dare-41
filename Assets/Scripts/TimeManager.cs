using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // The event delegate for ticks
    public delegate void TickDelegate();
    // The event listener for ticks for the top floor
    public TickDelegate topFloorTickDelegate;
    // The event listener for ticks for the bottom floor
    public TickDelegate bottomFloorTickDelegate;
    // The range of time in which a tick can occur, i.e. +/- 1 second either way
    private float rangeOfPossibleTicks = 1f;
    // The beginning average time per tick at the start of the game
    private float averageTickTime = 3f;
    // How long should each level be before stepping up to the next
    private float durationOfLevel = 10f;
    private float timeTillNextLevel = 10f;
    // How much shorter should the average time per tick get per step
    private float stepAmountOfNextLevel = 0.2f;
    // The fastest speed the game should get
    private float minTickDuration = 1.5f;
    // The current top floor time to the next tick
    private float topFloorTimeToNextTick = 0;
    // The current bottom floor time to the next tick
    private float bottomFloorTimeToNextTick = 0;

    // Update loop which manages ticks
    public void Update()
    {
        updateTickTimer(ref topFloorTimeToNextTick, topFloorTickDelegate);
        updateTickTimer(ref bottomFloorTimeToNextTick, bottomFloorTickDelegate);
        updateLevelTime();
    }

    private void updateLevelTime()
    {
        timeTillNextLevel -= Time.deltaTime;
        if (timeTillNextLevel <= 0)
        {
            averageTickTime -= stepAmountOfNextLevel;
            averageTickTime = Mathf.Clamp(averageTickTime, minTickDuration, 100);
            timeTillNextLevel = durationOfLevel;
        }
    }

    private void updateTickTimer(ref float timeTillTick, TickDelegate tickDelegate)
    {
        // Update the time until the next tick
        timeTillTick -= Time.deltaTime;
        // Check if a tick has occured
        if (timeTillTick <= 0)
        {
            // Fire the event
            if (tickDelegate != null)
            {
                tickDelegate();
            }
            // Reset the tick timer
            timeTillTick = getRandomTickTIme();
        }
    }

    private float getRandomTickTIme()
    {
        float minTime = (averageTickTime - rangeOfPossibleTicks);
        float maxTime = (averageTickTime + rangeOfPossibleTicks);
        return Random.Range(minTime, maxTime);
    }
}