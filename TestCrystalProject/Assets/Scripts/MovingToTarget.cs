using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTarget : MonoBehaviour {

    CrystalManager crystalManager;
    GameObject target;
    float speed;
    Vector3 velocity;

    void Start () {
        crystalManager = GameObject.Find("Crystals").GetComponent<CrystalManager>();
        target = crystalManager.targetObject;
        speed = crystalManager.toTargetSpeed;
        velocity = Vector3.zero;
    }
	
	void Update () {
        Vector3 diff = target.transform.position - this.transform.position;

        velocity += diff.normalized * speed * Time.deltaTime;
        this.transform.position += velocity;
    }
}
