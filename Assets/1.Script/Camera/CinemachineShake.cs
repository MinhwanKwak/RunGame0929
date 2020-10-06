using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CinemachineShake : MonoBehaviour
{
    // 시네머신 카메라 변수
    [SerializeField, Tooltip("시네머신 카메라")]
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin VirtualCameraNoise;

    // 화면 효과 지속시간 조절용 변수
    [Tooltip("카메라 셰이크 효과가 지속되는 시간")]
    private float ShakeDuration = 0f;
    [Tooltip("화면크기 변경 효과가 지속되는 시간")]
    private float FVchangeDuration = 0f;

    // 쉐이크 효과 조절용 변수
    private float ShakeAmplitude = 0f; // 진폭
    private float ShakeFrequency = 2f; // 빈도
    private float FieldOfViewSize = 0;

    public float InitFieldOfViewSize; // 해당 씬 기존 카메라 뷰 사이즈
    private float culFieldOfViewSize; // 해당 씬 현재 카메라 뷰 사이즈

    // 초기화
    private void Awake()
    {
        GameManager.Instance.cinemachineShake = this;
        VirtualCamera = GameObject.Find("Cinemachine").GetComponent<CinemachineVirtualCamera>();

        // 값 할당
        if (VirtualCamera != null)
        {
            VirtualCamera.m_Lens.FieldOfView = InitFieldOfViewSize;
            VirtualCameraNoise 
                = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            SetShakeParameters(0.5f, 1f, 0.5f);
        }

        ShakeCinemachineEffect();

       CinemachineFieldOfViewSizeEffect();
    }

    #region 이펙트
    // 카메라 흔들림 효과
    public void ShakeCinemachineEffect()
    {
        if (VirtualCamera != null || VirtualCameraNoise != null)
        {
            // 카메라 세이크 효과가 재생 중일 때
            if (ShakeDuration > 0)
            {
                // 카메라 노이즈 파라미터들의 값 설정
                VirtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                VirtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // 세이크 시간 감소
                ShakeDuration -= Time.deltaTime;
            }
            else
            {
                // 카메라 세이크 이팩트가 끝났을 때, 값을 리셋해준다
                VirtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeDuration = 0f;
            }
        }
    }

    public void CinemachineFieldOfViewSizeEffect()
    {
        if (VirtualCamera != null)
        {
            if (GameManager.Instance.player.playerStatus == Player.PlayerStatus.DEAD || GameManager.Instance.player.playerStatus == Player.PlayerStatus.CLEAR) return;

            // Field Of View 사이즈 변경 효과가 재생 중일 때
            if (FVchangeDuration > 0)
            {
                // Field Of View 파라미터 값 설정
                VirtualCamera.m_Lens.FieldOfView 
                    = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, FieldOfViewSize, Time.deltaTime*2);

                // 세이크 시간 감소
                FVchangeDuration -= Time.deltaTime;
            }
            else
            {
                if (GameManager.Instance.player.playerStatus == Player.PlayerStatus.DASH)
                {
                    // 해당 이팩트가 끝났을 때, 값을 리셋해준다
                    VirtualCamera.m_Lens.FieldOfView
                        = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, culFieldOfViewSize, Time.deltaTime);
                }

                FVchangeDuration = 0;
            }
        }
    }

    public void ChangeFieldOfViewSize()
    {
        if (VirtualCamera != null)
        {
            // Field Of View 사이즈 변경 효과가 재생 중일 때
            if (FVchangeDuration > 0)
            {
                // Field Of View 파라미터 값 설정
                VirtualCamera.m_Lens.FieldOfView
                    = Mathf.Lerp(VirtualCamera.m_Lens.FieldOfView, FieldOfViewSize, Time.deltaTime*2);

                // 세이크 시간 감소
                FVchangeDuration -= Time.deltaTime;
            }
            else
            {
                FVchangeDuration = 0;
            }
        }
    }
    #endregion

    #region 파라미터 셋팅
    // Shake
    public void SetShakeParameters(float ShakeDuration, float ShakeAmplitude, float ShakeFrequency = 2f)
    {
        this.ShakeDuration = ShakeDuration;
        this.ShakeAmplitude = ShakeAmplitude;
        this.ShakeFrequency = ShakeFrequency;
    }
    // Field Of View
    public void SetFieldOfViewSizeParameters(float time, float Size)
    {
        FVchangeDuration = time;  // 대쉬 지속시간을 넣을 것
        FieldOfViewSize = VirtualCamera.m_Lens.FieldOfView+Size;
        culFieldOfViewSize = VirtualCamera.m_Lens.FieldOfView;
    }
    #endregion
}
