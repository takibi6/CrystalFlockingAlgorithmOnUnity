using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManagerBoid : MonoBehaviour {

    [Header("ｵﾌﾞｼﾞｪｸﾄ数")]
    public int crystalNum;
    [Header("初期位置のﾗﾝﾀﾞﾑ範囲")]
    public float randomSetupPosition;
    [Header("各ｵﾌﾞｼﾞｪｸﾄの探索範囲")]
    public float searchDistance;
    [Header("各ｵﾌﾞｼﾞｪｸﾄの視野(内積)")]
    public float crystalGetAngle;
    [Header("速度")]
    public float moveSpeed;
    [Header("速度のﾗﾝﾀﾞﾑ度合い")]
    public float randomRate;
    [Header("加速度の適用度(ｶｰﾌﾞに影響)")]
    [Range(0.0f, 1.0f)]
    public float turnRate;
    [Header("他ｵﾌﾞｼﾞｪｸﾄと保つ距離")]
    public float keepDistance;
    [Header("群衆の適用度")]
    [Range(0.0f, 1.0f)]
    public float applicability;
    
    public GameObject targetObject;
    public GameObject centerMark;

    [System.NonSerialized]
    public GameObject[] crystalObjects;
    [System.NonSerialized]
    public CrystalMoveBoid[] crystalClasses;
    [System.NonSerialized]
    public Vector3[] crystalVelocity;

    [SerializeField]
    private GameObject crystalPrefab;
    [SerializeField]
    private Transform crystalParent;

    //--------------------------------------------------------最初だけの初期化--------------------------------------------------------
    void Start () {
        crystalObjects = new GameObject[crystalNum];
        crystalClasses = new CrystalMoveBoid[crystalNum];
        crystalVelocity = new Vector3[crystalNum];

        for (int i = 0; i < crystalNum; i++)
        {
            crystalObjects[i] = Instantiate(crystalPrefab, crystalParent);
            crystalObjects[i].transform.position = new Vector3(Random.Range(-randomSetupPosition, randomSetupPosition), Random.Range(-randomSetupPosition, randomSetupPosition), Random.Range(-randomSetupPosition, randomSetupPosition));
            crystalClasses[i] = crystalObjects[i].GetComponent<CrystalMoveBoid>();
            crystalClasses[i].managerClass = this;
            crystalVelocity[i] = Vector3.zero;
        }
	}
}
