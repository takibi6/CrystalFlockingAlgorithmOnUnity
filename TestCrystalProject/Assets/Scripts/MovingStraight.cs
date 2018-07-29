using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStraight : MonoBehaviour {

    float speed;

    void Start () {
        speed = GameObject.Find("Crystals").GetComponent<CrystalManager>().freeSpeed;
    }
	
	void Update () {
        this.transform.position += this.transform.forward * speed;
    }
}
