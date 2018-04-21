using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
	private float moveSpeed = -0.2f;
    private Camera mainCamera;
    private float respawnPosition;

    public void Start()
    {
        // Store reference to main camera
        mainCamera = Camera.main;
        // Work out at what position the zone should resapwn itself
        respawnPosition = mainCamera.ViewportToWorldPoint(Vector2.zero).x * 2;
    }

    public void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0));
        if (transform.position.x <= respawnPosition)
        {
            transform.position = new Vector2(respawnPosition * -1, 0);
        }
    }
}
