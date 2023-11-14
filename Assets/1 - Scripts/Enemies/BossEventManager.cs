using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventManager : MonoBehaviour
{

    public GameObject BossImage;
    public GameObject Boss;
    public GameObject BossHealth;

    public PlayerManager player;
    public GameObject joystickCanva;

    public CinemachineVirtualCamera virtualCamera;

    private float originalSpeed;

    private void Start()
    {
        CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        framingTransposer.m_DeadZoneWidth = 0.1378f;
        framingTransposer.m_DeadZoneHeight = 0.119f;

        framingTransposer.m_SoftZoneWidth = 0.57f;
        framingTransposer.m_SoftZoneHeight = 0.697f;
    }

    public void FreezePlayer()
    {
        if(virtualCamera != null)
        {
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            virtualCamera.Follow = BossImage.transform;

            
        }

        originalSpeed = player.speed;

        if (player != null)
        {
            player._Rigidbody.velocity = Vector3.zero;
            player.speed = 0;
            joystickCanva.SetActive(false);
        }
    }

    public void ActivateBoss()
    {
        if(virtualCamera != null)
        {
            CinemachineFramingTransposer framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            virtualCamera.Follow = player.transform;

            DOTween.To(() => virtualCamera.m_Lens.OrthographicSize, x => virtualCamera.m_Lens.OrthographicSize = x, 12f, 2f)
            .SetEase(Ease.InOutQuint).OnComplete(() =>
            {
                framingTransposer.m_TrackedObjectOffset = new Vector3(0f, 0f, 0f);

                framingTransposer.m_DeadZoneWidth = 0.5f;
                framingTransposer.m_DeadZoneHeight = 0.5f;

                framingTransposer.m_SoftZoneWidth = 0.7f;
                framingTransposer.m_SoftZoneHeight = 1;
                DOTween.Kill(gameObject);
            });
        }

        if (player != null)
        {
            player.speed = originalSpeed;
            joystickCanva.SetActive(true);
        }

        if (Boss != null)
        {
            BossHealth.SetActive(true);
            BossImage.SetActive(false);
            Boss.SetActive(true);
        }
    }
}
