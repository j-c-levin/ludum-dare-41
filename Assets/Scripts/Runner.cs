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
        Standing,
        Jumping
    }
    private RunnerState runnerState;
    // If the runner is in the air you can't activate a crouching card
    private enum AirbornState
    {
        Jumping,
        Landed
    }
    private AirbornState airbornState;
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

    public bool canPlayCard(Card card)
    {
        // Ducking whilst in the air is not allowed
        return (card.type == CardType.Duck && airbornState == AirbornState.Jumping) == false;
    }

    public void Jump()
    {
        // Stop the ducking animation if it's playing
        if (duckingAnimation != null)
        {
            StopCoroutine(duckingAnimation);
        }
        // Stand the runner up
        ChangeRunnerState(RunnerState.Standing);
        // Jump
        SingleJumpSequence();
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
        if (runnerState == newState)
        {
            return;
        }
        // Set the new sprite
        runnerSprite.sprite = (newState == RunnerState.Standing) ? runnerStandingSprite : runnerDuckingSprite;
        // Set the new collider size
        runnerCollider.size = (newState == RunnerState.Standing) ? standingColliderSize : duckingColliderSize;
        // Determine if the runner needs to be shifted up or down as the size changes
        int translationDirection = (newState == RunnerState.Standing) ? 1 : -1;
        // Translate the runner
        Vector2 newTranslation = new Vector2(0, translationBetweenSizes * translationDirection);
        runner.transform.Translate(newTranslation);
        // Set the new state
        runnerState = newState;
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
        runnerState = RunnerState.Standing;
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
        airbornState = AirbornState.Jumping;
        // Jump up
        DOTween.Sequence()
        .Append(transform.DOMoveY(singleJumpHeight, jumpDuration).SetEase(Ease.OutQuad))
        .Append(transform.DOMoveY(-singleJumpHeight, jumpDuration).SetEase(Ease.InQuad))
        .AppendCallback(() =>
        {
            airbornState = AirbornState.Landed;
        });
    }
}
