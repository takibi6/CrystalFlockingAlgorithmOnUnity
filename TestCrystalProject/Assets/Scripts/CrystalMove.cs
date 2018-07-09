using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMove : MonoBehaviour {

    CrystalManager managerClass;
    Vector3 averagePosition = Vector3.zero;
    Vector3 averageVelocity = Vector3.zero;
    Vector3 velocity = Vector3.zero;

    void Start () {
        managerClass = GameObject.Find("Crystals").GetComponent<CrystalManager>();
    }
	
	void Update () {
        velocity = Vector3.zero;

        SearchCrystals();
        DoCohesion();
        DoAlignment();

        MoveCrystal();
        RotateCrystal(velocity);
	}


    //探索
    void SearchCrystals()
    {
        //if (transform.GetSiblingIndex() != 0) return;   //テスト用

        int count = 0;
        averagePosition = Vector3.zero;
        averageVelocity = Vector3.zero;

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

                    //分離処理
                    DoSeparation(diffVector);
                }
            }
        }

        //平均を出す
        if (count > 0)
        {
            averagePosition /= count;   //平均位置（自身を含めない）
            averageVelocity /= count;
        }
        //対象がいなかった場合、リーダー位置へ向かう（平均しない）
        else
        {
            averagePosition = managerClass.targetObject.transform.position;
        }

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
        
        //this.transform.position += diff * Time.deltaTime * managerClass.moveSpeed;
        velocity = velocity * managerClass.disorder + averagePosition * (1f - managerClass.disorder);
    }


    //分離：一定距離を保つ
    void DoSeparation(Vector3 diff)
    {
        if (diff.magnitude > managerClass.keepDistance) return;
        //Debug.Log(diff.magnitude);

        float removeVelocity = managerClass.keepDistance / diff.magnitude;
        //this.transform.Translate(diff.normalized * Time.deltaTime * removeVelocity);
        velocity += diff.normalized * removeVelocity;
    }


    //整列：平均速度ベクトルに合わせる
    void DoAlignment()
    {
        velocity = velocity * managerClass.disorder + averageVelocity * (1f - managerClass.disorder);
    }


    //動く
    void MoveCrystal()
    {
        this.transform.position += velocity * Time.deltaTime * managerClass.moveSpeed;
    }

    //個体の進行方向に合わせて回転させる
    void RotateCrystal(Vector3 velocity)
    {
        this.transform.LookAt(this.transform.position + velocity.normalized);
    }
}
