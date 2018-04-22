using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LandingStartTutorial : MonoBehaviour
{
    private float moveHeight = 30f;

    public void Start()
    {
        animate();
    }

    public void onClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void animate()
    {
        DOTween.Sequence()
        .Append(
            transform.DOLocalMoveY(moveHeight, 2f)
                .SetEase(Ease.InOutSine)
        )
        .Append(
            transform.DOLocalMoveY(-moveHeight, 2f)
                .SetEase(Ease.InOutSine)
        )
        .SetLoops(-1, LoopType.Yoyo);
    }
}
