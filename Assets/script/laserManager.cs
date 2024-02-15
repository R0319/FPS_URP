using System.Collections;
using UnityEngine;
using DG.Tweening; //DOTweenを使う際はこのusingを入れる

public class laserManager : MonoBehaviour
{
    #region 変数等
    //攻撃力
    [SerializeField]
    int damage;

    [SerializeField]
    int rayDistans = 2000;

    [SerializeField]
    GameObject laserPlefab;

    public Transform target;

    [SerializeField]
    GameObject MagicCirclePlefab;

    [SerializeField]
    Ease circleEase;

    [SerializeField]
    Transform laserPoint;

    [SerializeField]
    Transform[] laserPoints;

    [SerializeField]
    float laserspeed;

    [SerializeField]
    bool laserTestStop;

    #endregion

    void DestroyPrefab()
    {
        Destroy(laserPlefab);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistans))
            {
                Debug.Log(hit.point);

                Vector3 rayDirection = ray.direction;
                Quaternion rayRotarion = Quaternion.LookRotation(rayDirection);

                Vector3[] positions = new Vector3[laserPoints.Length];
                for(int i = 0; i < positions.Length; i++)
                {

                    positions[i] = hit.point - laserPoints[i].position;
                }
                MultLaserGeneration(positions);
            }
            else
            {
                // 物体に当たらなかった場合の処理
                // メインカメラの向いている方向にレーザーを生成する
                Vector3 cameraRayDirection = Camera.main.transform.forward;
                Quaternion cameraRayRotation = Quaternion.LookRotation(cameraRayDirection);

                Vector3[] positions = new Vector3[laserPoints.Length];
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = cameraRayDirection * 100f; // レーザーの長さを設定する場合は、適切な長さに調整してください
                }
                MultLaserGeneration(positions);
            }
        }

        #region 未使用(参考用に残しています)
        //if (Input.GetMouseButtonDown(0))
        {
            //transforom.fotwarfdはZ軸の方向（rayの正面）のことを指す
            //Ray ray = new Ray(transform.position, transform.forward);

            //if (Physics.Raycast(ray, out hit, rayDistans))
            {
                //Debug.Log(hit.point);

                //rayDirection = ray.direction;
                //rayRotarion = Quaternion.LookRotation(rayDirection);
                //レーザーの進む方向をlaserDirectionに代入（終点hit.pointと始点laserPoint.positionを元にベクトル作成）
                //laserDirection = hit.point - laserPoint.position;
            }
            //else
            {

            }
            //消したメソッド()いまはMultLaserGeneration()
        }
        #endregion
    }
    IEnumerator DestroyLaserAfterDelay(GameObject laserObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(laserObject);
    }
    IEnumerator StopAnime(Animator animator)
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("laser_start", false);
    }

    #region MultLaserGenerationレーザー発射メソッド
    /// <summary>
    /// for 文のレイザー生成処理
    /// </summary>
    /// <param name="positions">laserの方向の情報がある</param>
    void MultLaserGeneration(Vector3[] positions)
    {

        for (int i = 0; i < laserPoints.Length; i++)
        {
            Quaternion laserRotion;
            //レーザー自体の方向(生成時)を決めるためにベクトルをQuaternionにする

            laserRotion = Quaternion.LookRotation(positions[i]);
            //レーザーをInstantiate(何を生成するか,生成場所,方向)でplefab生成
            GameObject laser = Instantiate(laserPlefab, laserPoints[i].position, laserRotion);

            Vector3 MagicCirclePos = new Vector3(laserPoints[i].position.x, laserPoints[i].position.y, laserPoints[i].position.z);
            GameObject MagicCircle = Instantiate(MagicCirclePlefab, MagicCirclePos, laserRotion);

            float circleScale = MagicCircle.transform.localScale.x;
            MagicCircle.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            //魔法陣の拡大アニメーション
            sequence.Append(MagicCircle.transform.DOScale(Vector3.one * circleScale, 0.2f).SetEase(circleEase));
            sequence.AppendInterval(1.0f);
            //縮小アニメーション
            sequence.Append(MagicCircle.transform.DOScale(Vector3.zero, 0.5f).SetEase(circleEase).OnComplete(() => Destroy(MagicCircle)));

            //レーザーのアニメーションをテストするために飛ばさないようにする
            if (laserTestStop == false)
            {
                //rbを使えるように作成
                Rigidbody rb = laser.GetComponent<Rigidbody>();
                rb.AddForce(positions[i].normalized * laserspeed);
            }

            Animator animator = laser.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("laser_start", true);

                StartCoroutine(StopAnime(animator));
            }
            

            StartCoroutine(DestroyLaserAfterDelay(laser, 5.0f));

            Debug.DrawRay(laserPoints[i].position, positions[i].normalized * laserspeed, UnityEngine.Color.red, 3f);
        }
    }
    #endregion
}
