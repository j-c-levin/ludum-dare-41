﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour {
    private float moveSpeed = -0.05f;

    // Loop to move the object along
    public void FixedUpdate()
    {
        // Move the gameobject along
        transform.Translate(new Vector2(moveSpeed, 0));
    }
}
