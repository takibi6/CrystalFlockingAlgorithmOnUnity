using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMoveAverage : MonoBehaviour {

    [System.NonSerialized]
    public CrystalManagerAverage managerClass;

    [SerializeField]
    private Rigidbody rigidbody;
    
    Vector3 acceleration;
    Vector3 velocity;
    Vector3 destination;
    float randomSpeed;

    //--------------------------------------------------------最初だけの初期化--------------------------------------------------------
    void Start () {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        destination = Vector3.zero;
        randomSpeed = Random.Range(1/managerClass.randomRate, managerClass.randomRate);
    }

    //--------------------------------------------------------メインループ--------------------------------------------------------
    void Update () {
        InitParam();
        
        DoCohesion();
        //DoAlignment();

        MoveCrystal();
        RotateCrystal(velocity);
    }

    //--------------------------------------------------------ループ内の初期化--------------------------------------------------------
    void InitParam()
    {
        acceleration = Vector3.zero;
        destination = Vector3.zero;
    }

    //--------------------------------------------------------Boidアルゴリズム系--------------------------------------------------------
    //結合：中心に向かって動く
    void DoCohesion()
    {
        managerClass.allSumPosition += this.transform.position;
        destination += managerClass.allAveragePosition;
    }
    
    //整列：平均速度ベクトルに合わせる
    void DoAlignment()
    {
        managerClass.allSumVelocity += this.transform.position;
        destination += managerClass.allAverageVelocity;
    }

    
    //---------------------------------------------------------動く--------------------------------------------------------
    //各種適用度によるベクトルのブレンド
    void MoveCrystal()
    {
        Vector3 diff = managerClass.targetObject.transform.position - transform.position;

        //群衆の目的地と、ターゲット位置のブレンド(managerClass.applicability)
        acceleration = destination.normalized * managerClass.applicability + diff.normalized * (1f - managerClass.applicability);

        //加速度の適用度(managerClass.turnRate)。曲がるときの旋回具合に影響
        velocity = velocity * (1f - managerClass.turnRate) + acceleration * managerClass.turnRate;

        //速度を計算
        velocity += acceleration * managerClass.moveSpeed * randomSpeed * Time.deltaTime;
        rigidbody.velocity = velocity;
        //this.transform.position += velocity;

        //平均速度の計算
        managerClass.allSumVelocity += velocity;
    }

    //個体の進行方向に合わせて回転させる
    void RotateCrystal(Vector3 velocity)
    {
        this.transform.LookAt(this.transform.position + velocity.normalized);
    }
}
