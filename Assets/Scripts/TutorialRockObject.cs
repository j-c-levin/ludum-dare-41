using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRockObject : MonoBehaviour
{

    public void OnCollisionEnter2D(Collision2D other)
    {
        TutorialCardManager manager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<TutorialCardManager>();
        manager.AddRockToHand();
        Destroy(this.gameObject);
    }
}
