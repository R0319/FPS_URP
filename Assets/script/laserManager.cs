using System.Collections;
using UnityEngine;
using DG.Tweening; //DOTween���g���ۂ͂���using������

public class laserManager : MonoBehaviour
{
    #region �ϐ���
    //�U����
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
                // ���̂ɓ�����Ȃ������ꍇ�̏���
                // ���C���J�����̌����Ă�������Ƀ��[�U�[�𐶐�����
                Vector3 cameraRayDirection = Camera.main.transform.forward;
                Quaternion cameraRayRotation = Quaternion.LookRotation(cameraRayDirection);

                Vector3[] positions = new Vector3[laserPoints.Length];
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = cameraRayDirection * 100f; // ���[�U�[�̒�����ݒ肷��ꍇ�́A�K�؂Ȓ����ɒ������Ă�������
                }
                MultLaserGeneration(positions);
            }
        }

        #region ���g�p(�Q�l�p�Ɏc���Ă��܂�)
        //if (Input.GetMouseButtonDown(0))
        {
            //transforom.fotwarfd��Z���̕����iray�̐��ʁj�̂��Ƃ��w��
            //Ray ray = new Ray(transform.position, transform.forward);

            //if (Physics.Raycast(ray, out hit, rayDistans))
            {
                //Debug.Log(hit.point);

                //rayDirection = ray.direction;
                //rayRotarion = Quaternion.LookRotation(rayDirection);
                //���[�U�[�̐i�ޕ�����laserDirection�ɑ���i�I�_hit.point�Ǝn�_laserPoint.position�����Ƀx�N�g���쐬�j
                //laserDirection = hit.point - laserPoint.position;
            }
            //else
            {

            }
            //���������\�b�h()���܂�MultLaserGeneration()
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

    #region MultLaserGeneration���[�U�[���˃��\�b�h
    /// <summary>
    /// for ���̃��C�U�[��������
    /// </summary>
    /// <param name="positions">laser�̕����̏�񂪂���</param>
    void MultLaserGeneration(Vector3[] positions)
    {

        for (int i = 0; i < laserPoints.Length; i++)
        {
            Quaternion laserRotion;
            //���[�U�[���̂̕���(������)�����߂邽�߂Ƀx�N�g����Quaternion�ɂ���

            laserRotion = Quaternion.LookRotation(positions[i]);
            //���[�U�[��Instantiate(���𐶐����邩,�����ꏊ,����)��plefab����
            GameObject laser = Instantiate(laserPlefab, laserPoints[i].position, laserRotion);

            Vector3 MagicCirclePos = new Vector3(laserPoints[i].position.x, laserPoints[i].position.y, laserPoints[i].position.z);
            GameObject MagicCircle = Instantiate(MagicCirclePlefab, MagicCirclePos, laserRotion);

            float circleScale = MagicCircle.transform.localScale.x;
            MagicCircle.transform.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            //���@�w�̊g��A�j���[�V����
            sequence.Append(MagicCircle.transform.DOScale(Vector3.one * circleScale, 0.2f).SetEase(circleEase));
            sequence.AppendInterval(1.0f);
            //�k���A�j���[�V����
            sequence.Append(MagicCircle.transform.DOScale(Vector3.zero, 0.5f).SetEase(circleEase).OnComplete(() => Destroy(MagicCircle)));

            //���[�U�[�̃A�j���[�V�������e�X�g���邽�߂ɔ�΂��Ȃ��悤�ɂ���
            if (laserTestStop == false)
            {
                //rb���g����悤�ɍ쐬
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
