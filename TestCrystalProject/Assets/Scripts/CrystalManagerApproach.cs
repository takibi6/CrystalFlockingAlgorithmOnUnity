using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManagerApproach : MonoBehaviour {

    //ｵﾌﾞｼﾞｪｸﾄにCollisionが付いている前提

    [Header("ｵﾌﾞｼﾞｪｸﾄ数")]
    public int crystalNum;
    [Header("速度")]
    public float moveSpeed;
    [Header("水晶の速度の差の大きさ")]
    public float differentRate;
    [Header("速度のﾗﾝﾀﾞﾑ度合い")]
    public float randomSpeed;
    [Header("ﾀｰｹﾞｯﾄが近いとき減速し始める距離")]
    public float decelerateDistance;
    [Header("加速度の適用度(大きいとｶｰﾌﾞが大きくなる)")]
    [Range(0.0f, 1.0f)]
    public float turnRate;

    [System.NonSerialized]
    public GameObject[] crystalObjects;
    [System.NonSerialized]
    public CrystalMoveApproach[] crystalClasses;

    [Header("──────────初期位置──────────")]
    [Header("初期配置の円の半径")]
    [SerializeField]
    private float startPositionRadius;
    [Header("ひとつの円にいくつ水晶があるか")]
    [SerializeField]
    private int splitNum;
    [Header("全体の位置の高さ")]
    [SerializeField]
    private float highPositionY;
    [Header("高さの段数")]
    [SerializeField]
    private int stageNum;
    [Header("始点の角度")]
    [SerializeField]
    private int startAngle;
    [Header("視界から除く個数")]
    [SerializeField]
    private int sightAngleNum;
    [Header("奥行き方面の位置のズレ度")]
    [SerializeField]
    private int depthAngleAdjust;

    [SerializeField]
    private GameObject crystalPrefab;
    [SerializeField]
    private Transform crystalParent;
    [SerializeField]
    private TargetOrbit targetOrbit;

    private int arrayCount;
    private int num;

    //--------------------------------------------------------最初だけの初期化--------------------------------------------------------
    void Start () {
        crystalObjects = new GameObject[crystalNum];
        crystalClasses = new CrystalMoveApproach[crystalNum];
        arrayCount = 0;

        for (int i = 0; i < crystalNum; i++)
        {
            crystalObjects[i] = Instantiate(crystalPrefab, crystalParent);
            crystalClasses[i] = crystalObjects[i].GetComponent<CrystalMoveApproach>();
            crystalClasses[i].managerClass = this;
            
            int num = (int)((float)targetOrbit.bodies.Length / crystalNum * i);
            crystalClasses[i].targetTransform = targetOrbit.bodies[num];

            ArrayPositionSetup(i);
        }
    }

    void ArrayPositionSetup(int i)
    {
        num = i + arrayCount + 2;
        int numInCircle = num % splitNum;                                   //その円周内で何番目か
        if (numInCircle > splitNum - sightAngleNum)                         //その円周終わり指定個数だったら
        {
            arrayCount += sightAngleNum;
        }

        float angleSplitDegree = 360f / splitNum;                           //ひとつの間隔の角度
        int circleNumY = num / splitNum;                                    //現在の縦の列番号
        float circleNumYPos = circleNumY % stageNum * 2 + highPositionY;    //現在の縦の位置
        int evenNumAdjust = circleNumY % 2 * splitNum / 2;                  //奇数番号は位置をずらす
        int nowRadiusNum = circleNumY / stageNum;                           //現在の円の番号(外側)
        float nowRadiusDistance = nowRadiusNum * splitNum / 10 + startPositionRadius;

        float angle = (((angleSplitDegree * num) + evenNumAdjust + (nowRadiusNum * depthAngleAdjust)) % 360f + startAngle) * Mathf.Deg2Rad;
        crystalObjects[i].transform.position = new Vector3(nowRadiusDistance * Mathf.Cos(angle), circleNumYPos, nowRadiusDistance * Mathf.Sin(angle));

        crystalObjects[i].transform.LookAt(crystalObjects[i].transform.position + Vector3.up);  //向き変える
    }
}
