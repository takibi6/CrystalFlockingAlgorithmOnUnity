using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBlur : MonoBehaviour {

    [SerializeField]
    private float blurDistance;
    [SerializeField]
    private CrystalMoveApproach crystalMove;

    void Update () {
        float speed = crystalMove.GetVelocity().magnitude * blurDistance;
        this.transform.localScale = new Vector3(0.9f, 0.9f, speed);
    }
}
