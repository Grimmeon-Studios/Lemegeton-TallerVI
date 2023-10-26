using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();

        _cam.m_Lens.OrthographicSize = 0f;

        StartCoroutine(waitSec(1.7f));
    }

    private IEnumerator waitSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CamTransition();
    }

    private void CamTransition()
    {
        DOTween.To(() => _cam.m_Lens.OrthographicSize, x => _cam.m_Lens.OrthographicSize = x, 8f, 2f)
            .SetEase(Ease.InOutQuint).OnComplete(() =>
            {
                DOTween.KillAll(gameObject);
            });
    }
}
