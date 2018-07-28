using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMove : MonoBehaviour {

    CrystalManager managerClass;
    Vector3 averagePosition;
    Vector3 averageVelocity;
    Vector3 acceleration;
    Vector3 velocity;
    Vector3 angle;

    void Start () {
        managerClass = GameObject.Find("Crystals").GetComponent<CrystalManager>();
        averagePosition = Vector3.zero;
        averageVelocity = Vector3.zero;
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        angle = Vector3.zero;
    }
	
	void Update () {
        InitParam();

        SearchCrystals();
        DoCohesion();
        DoAlignment();

        //MoveCrystal();
        RotateCrystal(velocity);
    }

    void InitParam()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        averagePosition = Vector3.zero;
        averageVelocity = Vector3.zero;
    }

    //探索
    void SearchCrystals()
    {
        //if (transform.GetSiblingIndex() != 0) return;   //テスト用
        int count = 0;

        for (int i = 0; i < managerClass.crystalNum; i++)
        {
            if (i != transform.GetSiblingIndex())   //自分自身を探索しない
            {
                Vector3 diffVector = this.transform.position - managerClass.crystalObjects[i].transform.position;

                if (InView(diffVector))                      //視界内だったら
                {
                    averagePosition += managerClass.crystalObjects[i].transform.position;
                    averageVelocity += managerClass.crystalClasses[i].velocity;
                    count++;
                }
                //分離処理
                DoSeparation(diffVector);
            }
        }

        //平均を出す
        if (count > 0)
        {
            //averagePosition += managerClass.targetObject.transform.position - transform.position;
            //count++;
            averagePosition /= count;   //平均位置（自身を含めない）
            averageVelocity /= count;
        }
        //対象がいなかった場合、リーダー位置へ向かう（平均しない）
        else
        {
            Vector3 diff = managerClass.targetObject.transform.position - transform.position;
            averagePosition = diff;
        }
        //averagePosition = managerClass.targetObject.transform.position - transform.position;  //デバッグ用、全員ターゲットを目指す
        //averagePosition += managerClass.targetObject.transform.position - transform.position;
        //averagePosition /= 2;

        if (transform.GetSiblingIndex() != 0) return;   //テスト用
        managerClass.centerMark.transform.position = averagePosition;   //確認用マーカーを動かす
    }

    //視界内か
    bool InView(Vector3 diff)
    {
        //視界内か判定
        if (diff.magnitude < managerClass.searchDistance)    //一定範囲内か
        {
            if (Vector3.Dot(this.transform.forward, diff.normalized) > managerClass.crystalGetAngle)  //一定角度内か
            {
                return true;
            }
        }
        return false;
    }


    //結合：中心に向かって動く
    void DoCohesion()
    {
        //Vector3 diff = averagePosition - this.transform.position;
        //if (diff.magnitude < managerClass.keepDistance) return;     //分離を優先
        
        acceleration = acceleration * managerClass.disorder + averagePosition * (1f - managerClass.disorder);
        angle = acceleration;
        //acceleration *= acceleration.magnitude;
    }


    //分離：一定距離を保つ
    void DoSeparation(Vector3 diff)
    {
        if (diff.magnitude > managerClass.keepDistance) return;
        //Debug.Log(diff.magnitude);
        
        float removeVelocity = managerClass.keepDistance / diff.magnitude;
        velocity += diff.normalized * removeVelocity;
    }


    //整列：平均速度ベクトルに合わせる
    void DoAlignment()
    {
        acceleration += acceleration * managerClass.disorder + averageVelocity * (1f - managerClass.disorder);
        angle += acceleration;
    }


    //動く
    void MoveCrystal()
    {
        //float tooSpeed = 1f;                    //行きすぎる（後でコントローラーの速度に変える）
        //velocity += acceleration * tooSpeed;
        //this.transform.position += velocity * managerClass.moveSpeed * Time.deltaTime;
        this.transform.position += this.transform.forward * managerClass.moveSpeed * Time.deltaTime;
    }

    //個体の進行方向に合わせて回転させる
    void RotateCrystal(Vector3 velocity)
    {
        this.transform.LookAt(this.transform.position + angle.normalized);
    }
}
