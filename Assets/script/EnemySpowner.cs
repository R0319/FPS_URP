using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpowner : MonoBehaviour
{
    #region 変数
    [SerializeField, Header("スポーン範囲")]
    navConteoller enmeyPrefab;
    [SerializeField]
    float spawnRadisu;
    [SerializeField]
    float minHeight;
    [SerializeField]
    float maxHeight;

    [SerializeField, Space(20)]
    float generationInterval;

    [SerializeField]
    private Transform playerTran;

    [SerializeField]
    int phaseCount = 0;

    [SerializeField]
    int[] maxEnemySpawnCounts = null;

    int enmeySpawnCount = 0;

    [SerializeField]
    public int enmeyKillCount = 0;

    #endregion

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(GenerationInterval());
    }
    void GenerateRandomPrefab()
    {
        Vector3 randomPoint = GatRandamPointInCircle(transform.position, spawnRadisu);

        if (IsPointOnNavMesh(randomPoint) && IsPointInRange(randomPoint, minHeight, maxHeight))
        {
            // NavMesh上にあり、高さの範囲内にある場合にのみ生成処理を行う
            navConteoller enemy = Instantiate(enmeyPrefab, randomPoint, Quaternion.identity);
            enemy.setAup(playerTran);
            enemy.gameObject.GetComponent<EnemyHpbarController>().SetUpEnemy(this);
        }
    }
    Vector3 GatRandamPointInCircle(Vector3 center, float radius)
    {
        //ランダムな角度と距離を代入して、円の範囲内にランダムな位置を計算
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);

        Vector3 randomPoint = center + Quaternion.Euler(0f, angle, 0f) * (Vector3.forward) * distance;
        return randomPoint;
    }


    bool IsPointOnNavMesh(Vector3 point)
    {
        NavMeshHit hit;
        //生成するための位置がnavmesh上にあるか判別
        //NavMesh.SamplePositionを使用して、指定位置の最も近いNavMesh上のポイントを探索
        return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
    }

    bool IsPointInRange(Vector3 point, float minHeight, float maxHeight)
    {
        //指定した範囲内の高さにあるか>=等を使用して判別
        return point.y >= minHeight && point.y <= maxHeight;
    }

    private IEnumerator GenerationInterval()
    {
        while (enmeySpawnCount < maxEnemySpawnCounts[phaseCount])
        {
            if (maxHeight <= 0 || minHeight > 0)
            {
                Debug.Log("スポーンの高さの値が間違っている");
            }

            yield return new WaitForSeconds(generationInterval);
            GenerateRandomPrefab();
            enmeySpawnCount++;
        }
        while(enmeyKillCount < maxEnemySpawnCounts[phaseCount])
        {
            yield return null;
        }
        CheckPhase();
    }

    void CheckPhase()
    {
        if (phaseCount < maxEnemySpawnCounts.Length-1)
        {
            phaseCount++;
            enmeySpawnCount = 0;
            enmeyKillCount = 0;
            StartCoroutine(GenerationInterval());
        }
        else
        {
            Debug.Log("ゲームクリア");
        }
    }


    #region Debagギズモ
    //ギズモを描画する（debag用）
    private void OnDrawGizmos()
    {
        //スポーン半径の円を描画
        Gizmos.color = Color.blue;
        //青のワイヤーで球(Sphere)を描画する、球の大きさはspawnRadisu
        Gizmos.DrawWireSphere(transform.position, spawnRadisu);

        //高さのラインを描画
        Vector3 minHeightPoint = new Vector3(transform.position.x, minHeight, transform.position.z);
        Vector3 maxHeightPoint = new Vector3(transform.position.x, maxHeight, transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(minHeightPoint, new Vector3(transform.position.x, maxHeight, transform.position.z));


        //最大高さの位置に緑のポイントを描画
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(maxHeightPoint, 0.1f);
    }

    #endregion

}
