using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Runner : MonoBehaviour
{
    // Useful references
    private Rigidbody2D runner;
    private SpriteRenderer runnerSprite;
    private BoxCollider2D runnerCollider;
    public Sprite runnerStandingSprite;
    public Sprite runnerDuckingSprite;
    // Correctly sizes the box collider based on state
    private Vector2 standingColliderSize = new Vector2(0.60f, 1.2f);
    private Vector2 duckingColliderSize = new Vector2(0.60f, 0.6f);
    // Animation variables
    // How far to shift the runner object down or up based on how it grows and shrinks
    private float translationBetweenSizes = 0.3f;
    private float singleJumpHeight = 0.85f;
    private float jumpDuration = 0.3f;
    private enum RunnerState
    {
        Crouching,
        Standing
    }
    private RunnerState currentState;
    // Have a reference to the ducking animation so that it can be cancelled mid way if a second card is played;
    private Coroutine duckingAnimation;

    public void Start()
    {
        SetupReferences();
        SetInitialValues();
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
    }

    public void Jump()
    {
        SingleJumpSequence();
        if (runnerSprite.sprite == runnerDuckingSprite)
        {
            runnerSprite.sprite = runnerStandingSprite;
            runnerCollider.size = standingColliderSize;
            runner.transform.Translate(new Vector2(0, translationBetweenSizes));
        }
    }

    public void Duck()
    {
        if (duckingAnimation != null)
        {
            StopCoroutine(duckingAnimation);
        }
        duckingAnimation = StartCoroutine("DuckingRoutine");
    }

    IEnumerator DuckingRoutine()
    {
        ChangeRunnerState(RunnerState.Crouching);
        // The jump animation is made of an upwards and downwards arc, each with its own jump duration the ducking animation mimics this by ducking for an equal length of time
        yield return new WaitForSeconds(jumpDuration * 2);
        ChangeRunnerState(RunnerState.Standing);
    }

    private void ChangeRunnerState(RunnerState newState)
    {
        // Only change the sprite if the state is not in the new state
        if (currentState == newState)
        {
            return;
        }
        currentState = newState;
        runnerSprite.sprite = (newState == RunnerState.Standing) ? runnerStandingSprite : runnerDuckingSprite;
        runnerCollider.size = (newState == RunnerState.Standing) ? standingColliderSize : duckingColliderSize;
        int translationDirection = (newState == RunnerState.Standing) ? 1 : -1;
        Vector2 newTranslation = new Vector2(0, translationBetweenSizes * translationDirection);
        runner.transform.Translate(newTranslation);
    }

    private void SetInitialValues()
    {
        // Set the initial velocity
        runner.velocity = new Vector2(0, 0);
        // Set the initial sprite
        runnerSprite.sprite = runnerStandingSprite;
        // Set the initial collider
        runnerCollider.size = standingColliderSize;
        // Set initial state
        currentState = RunnerState.Standing;
    }

    private void SetupReferences()
    {
        // Store the reference to the rigidbody
        runner = GetComponent<Rigidbody2D>();
        // Store the reference to the runner sprite
        runnerSprite = GetComponent<SpriteRenderer>();
        // Store the reference to the runner collider
        runnerCollider = GetComponent<BoxCollider2D>();
    }

    private void SingleJumpSequence()
    {
        // Jump up
        DOTween.Sequence()
        .Append(transform.DOMoveY(singleJumpHeight, jumpDuration).SetEase(Ease.OutQuad))
        .Append(transform.DOMoveY(-singleJumpHeight, jumpDuration).SetEase(Ease.InQuad));
    }
}
