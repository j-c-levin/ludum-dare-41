using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private bool testMode = false;
    // private float moveSpeed = -0.1f;
    private float moveSpeed = -0.7f;
    private Camera mainCamera;
    private float respawnPosition;

    // Initialisation
    public void Start()
    {
        // Store reference to main camera
        mainCamera = Camera.main;
        // Work out at what position the zone should resapwn itself
        respawnPosition = mainCamera.ViewportToWorldPoint(Vector2.zero).x * 2;
        // Move it 
        transform.position = new Vector2(respawnPosition * -1, transform.position.y);
    }

    // Loop to move the object along
    public void FixedUpdate()
    {
        // Don't run anything if testing is enabled
        if (testMode)
        {
            return;
        }
        // Move the gameobject along
        transform.Translate(new Vector2(moveSpeed, 0));
        // If the gameobject is past the left of the screen, destroy it
        if (transform.position.x <= respawnPosition)
        {
            Destroy(this.gameObject);
        }
    }
}
