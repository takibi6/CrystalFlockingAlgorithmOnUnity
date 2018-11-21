using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetOrbit : MonoBehaviour {

    [Header("まとまる速度。50以上だとたまに荒ぶる")]
    public float speed;
    [Header("個数(長さ)を指定・0には操作ﾀｰｹﾞｯﾄを入れる")]
    public Transform[] bodies;

    [Header("ｷｭｰﾌﾞ間の距離、ﾀﾞﾏができるならこれを小さく")]
    [SerializeField]
    private float separateDistance;
    [Header("前方のﾀｰｹﾞｯﾄと同じ位置にするしきい値")]
    [SerializeField]
    private float sameDistance;
    [SerializeField]
    private GameObject bodyPrefab;
    [SerializeField]
    private Transform bodyParent;

    void Start () {
        Vector3 adjustPos = new Vector3(1f, 0f, 0f);
        for (int i = 1; i < bodies.Length; i++)
        {
            bodies[i] = Instantiate(bodyPrefab.transform, bodyParent);
            bodies[i].position = bodies[i - 1].position + adjustPos;
        }
    }
	
	void Update () {
        FlockBodies();
	}

    void FlockBodies()
    {
        Vector3 diff;
        Vector3 direction;
        float distance;
        Vector3 velocity = Vector3.zero;
        Vector3 preVelocity = Vector3.zero;

        for (int i = 1; i < bodies.Length; i++)
        {
            //前のターゲットとの位置関係を取得
            diff = bodies[i - 1].position - bodies[i].position;
            direction = diff.normalized;
            distance = diff.magnitude;

            if (distance > separateDistance)
            {
                //離れていい距離をオーバーしたら、前ターゲットから一定距離に固定
                velocity = (bodies[i - 1].position - direction * (separateDistance - sameDistance)) - bodies[i].position;
                //bodies[i].position = bodies[i - 1].position - direction * separateDistance;
            }
            else if (distance > sameDistance)
            {
                //離れていい距離範囲内だったら、普通に移動（離れているほどスピードは上がる）
                velocity = diff * speed * Time.deltaTime;
            }
            else
            {
                //前ターゲットと近かったら同じ位置にする
                bodies[i].position = bodies[i - 1].position;
            }
            velocity -= preVelocity * i / bodies.Length;    //前のターゲットの速度を加算
            bodies[i].position += velocity;                 //移動

            bodies[i].LookAt(bodies[i - 1].position);       //前のターゲットの方を向く
            preVelocity = velocity;
        }
    }
}
