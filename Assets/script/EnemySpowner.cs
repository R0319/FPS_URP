using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpowner : MonoBehaviour
{
    #region �ϐ�
    [SerializeField, Header("�X�|�[���͈�")]
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
            // NavMesh��ɂ���A�����͈͓̔��ɂ���ꍇ�ɂ̂ݐ����������s��
            navConteoller enemy = Instantiate(enmeyPrefab, randomPoint, Quaternion.identity);
            enemy.setAup(playerTran);
            enemy.gameObject.GetComponent<EnemyHpbarController>().SetUpEnemy(this);
        }
    }
    Vector3 GatRandamPointInCircle(Vector3 center, float radius)
    {
        //�����_���Ȋp�x�Ƌ����������āA�~�͈͓̔��Ƀ����_���Ȉʒu���v�Z
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);

        Vector3 randomPoint = center + Quaternion.Euler(0f, angle, 0f) * (Vector3.forward) * distance;
        return randomPoint;
    }


    bool IsPointOnNavMesh(Vector3 point)
    {
        NavMeshHit hit;
        //�������邽�߂̈ʒu��navmesh��ɂ��邩����
        //NavMesh.SamplePosition���g�p���āA�w��ʒu�̍ł��߂�NavMesh��̃|�C���g��T��
        return NavMesh.SamplePosition(point, out hit, 0.1f, NavMesh.AllAreas);
    }

    bool IsPointInRange(Vector3 point, float minHeight, float maxHeight)
    {
        //�w�肵���͈͓��̍����ɂ��邩>=�����g�p���Ĕ���
        return point.y >= minHeight && point.y <= maxHeight;
    }

    private IEnumerator GenerationInterval()
    {
        while (enmeySpawnCount < maxEnemySpawnCounts[phaseCount])
        {
            if (maxHeight <= 0 || minHeight > 0)
            {
                Debug.Log("�X�|�[���̍����̒l���Ԉ���Ă���");
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
            Debug.Log("�Q�[���N���A");
        }
    }


    #region Debag�M�Y��
    //�M�Y����`�悷��idebag�p�j
    private void OnDrawGizmos()
    {
        //�X�|�[�����a�̉~��`��
        Gizmos.color = Color.blue;
        //�̃��C���[�ŋ�(Sphere)��`�悷��A���̑傫����spawnRadisu
        Gizmos.DrawWireSphere(transform.position, spawnRadisu);

        //�����̃��C����`��
        Vector3 minHeightPoint = new Vector3(transform.position.x, minHeight, transform.position.z);
        Vector3 maxHeightPoint = new Vector3(transform.position.x, maxHeight, transform.position.z);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(minHeightPoint, new Vector3(transform.position.x, maxHeight, transform.position.z));


        //�ő卂���̈ʒu�ɗ΂̃|�C���g��`��
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(maxHeightPoint, 0.1f);
    }

    #endregion

}
