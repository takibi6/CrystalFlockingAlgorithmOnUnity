using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingToTarget : MonoBehaviour {

    [System.NonSerialized]
    public CrystalManager managerClass;
    [System.NonSerialized]
    public Vector3 velocity;

    GameObject target;
    float speed;
    

    void Start () {
        managerClass = GameObject.Find("Crystals").GetComponent<CrystalManager>();
        target = managerClass.targetObject;
        speed = managerClass.freeSpeed;
        velocity = Vector3.zero;
    }
	
	void Update () {
        //Vector3 diff = target.transform.position - this.transform.position;

        //velocity += diff.normalized * speed * Time.deltaTime;

        //Vector3 baseVelocity = managerClass.crystalVelocity[transform.GetSiblingIndex()];
        //managerClass.crystalVelocity[transform.GetSiblingIndex()] += velocity;
        //this.transform.position += managerClass.crystalVelocity[transform.GetSiblingIndex()];
    }
}
