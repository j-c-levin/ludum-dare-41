using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Runner : MonoBehaviour
{
    public Sprite runnerStandingSprite;
    public Sprite runnerDuckingSprite;
    public float jumpSpeed = 10;
    public float gravity = 1;
    private Rigidbody2D runner;
    private SpriteRenderer runnerSprite;
    private BoxCollider2D runnerCollider;
    private Vector2 standingColliderSize = new Vector2(0.60f, 1.2f);
    private Vector2 duckingColliderSize = new Vector2(0.60f, 0.6f);
    private float translationBetweenSizes = 0.4f;

    public void Start()
    {
        // Store the reference to the rigidbody
        runner = GetComponent<Rigidbody2D>();
        // Store the reference to the runner sprite
        runnerSprite = GetComponent<SpriteRenderer>();
        // Store the reference to the runner collider
        runnerCollider = GetComponent<BoxCollider2D>();
        // Set the initial velocity
        runner.velocity = new Vector2(0, 0);
        // Set the initial sprite
        runnerSprite.sprite = runnerStandingSprite;
        // Set the initial collider
        runnerCollider.size = standingColliderSize;
    }

    public void FixedUpdate()
    {
        if (Input.GetKeyDown("space"))
        {
            Jump();
        }
        if (Input.GetKeyDown("down"))
        {
            Duck();
        }
        float newVerticalVelocity = runner.velocity.y - gravity;
        runner.velocity = new Vector2(0, newVerticalVelocity);
    }

    public void Jump()
    {
        runner.velocity = new Vector2(0, jumpSpeed);
        if (runnerSprite.sprite == runnerDuckingSprite)
        {
            runnerSprite.sprite = runnerStandingSprite;
            runnerCollider.size = standingColliderSize;
            runner.transform.Translate(new Vector2(0, translationBetweenSizes));
        }
    }

    public void Duck()
    {
        if (runnerSprite.sprite == runnerStandingSprite)
        {
            runnerSprite.sprite = runnerDuckingSprite;
            runnerCollider.size = duckingColliderSize;
            runner.transform.Translate(new Vector2(0, -translationBetweenSizes));
        }
    }
}
