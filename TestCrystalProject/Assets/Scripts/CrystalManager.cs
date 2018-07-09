using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : MonoBehaviour {

    public int crystalNum;
    public float randomSetupPosition;
    public float searchDistance;
    public float crystalGetAngle;
    public float moveSpeed;
    public float lateSpeed;
    public float keepDistance;
    public float disorder;

    public GameObject crystalPrefab;
    public GameObject targetObject;
    public GameObject centerMark;

    public GameObject[] crystalObjects;
    public CrystalMove[] crystalClasses;

	void Start () {
        crystalObjects = new GameObject[crystalNum];
        crystalClasses = new CrystalMove[crystalNum];
        Transform crystalParent = GameObject.Find("Crystals").transform;

        for (int i = 0; i < crystalNum; i++)
        {
            crystalObjects[i] = Instantiate(crystalPrefab, crystalParent);
            crystalObjects[i].transform.position = new Vector3(Random.Range(-randomSetupPosition, randomSetupPosition), Random.Range(-randomSetupPosition, randomSetupPosition), Random.Range(-randomSetupPosition, randomSetupPosition));
            crystalClasses[i] = crystalObjects[i].GetComponent<CrystalMove>();
        }
	}
	
	void Update () {
		
	}
}
