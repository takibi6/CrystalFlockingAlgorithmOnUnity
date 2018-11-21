using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalMoveApproach : MonoBehaviour {

    [System.NonSerialized]
    public CrystalManagerApproach managerClass;
    [System.NonSerialized]
    public Transform targetTransform;

    [SerializeField]
    private Rigidbody rigidbody;

    int myNum;
    Vector3 acceleration;
    Vector3 velocity;
    float randomSpeed;
    float orderSpeed;

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    //--------------------------------------------------------最初だけの初期化--------------------------------------------------------
    void Start () {
        myNum = transform.GetSiblingIndex();
        velocity = Vector3.zero;
        orderSpeed = managerClass.differentRate - managerClass.differentRate / managerClass.crystalNum * (myNum + 1) + 1f;
        randomSpeed = Random.Range(1f, managerClass.randomSpeed);
    }

    //--------------------------------------------------------メインループ--------------------------------------------------------
    void Update () {
        MoveCrystal();
        RotateCrystal(velocity);
    }
    
    //---------------------------------------------------------動く--------------------------------------------------------
    //各種適用度によるベクトルのブレンド
    void MoveCrystal()
    {
        Vector3 diff = targetTransform.position - transform.position;
        acceleration = diff.normalized;

        //加速度の適用度(managerClass.turnRate)。曲がるときの旋回具合に影響
        velocity = velocity * (1f - managerClass.turnRate) + acceleration * managerClass.turnRate;

        //速度を計算
        velocity += acceleration * managerClass.moveSpeed * orderSpeed * randomSpeed * Time.deltaTime * DecelerateNearTarget(diff);
        rigidbody.velocity = velocity;
        //this.transform.position += velocity;
    }
    
    //ターゲットに近いほど減速
    float DecelerateNearTarget(Vector3 diff)
    {
        if (diff.magnitude < managerClass.decelerateDistance && PointingController.pointerVector.magnitude < 1f)
        {
            return PointingController.pointerVector.magnitude;
        }
        else
        {
            return diff.magnitude / 1f;
        }
    }


    //個体の進行方向に合わせて回転させる
    void RotateCrystal(Vector3 velocity)
    {
        this.transform.LookAt(this.transform.position + velocity.normalized);
    }
}
