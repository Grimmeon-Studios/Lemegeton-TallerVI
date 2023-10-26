using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    [SerializeField] private Image blockingImage;

    Color transparent = new Color(0, 0, 0, 0);

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();

        _cam.m_Lens.OrthographicSize = 0f;

        StartCoroutine(waitSec(1.7f));

        blockingImage.gameObject.SetActive(true);
    }

    private IEnumerator waitSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CamTransition();
    }

    private void CamTransition()
    {
        blockingImage.DOColor(transparent, 2f);
        DOTween.To(() => _cam.m_Lens.OrthographicSize, x => _cam.m_Lens.OrthographicSize = x, 8f, 2f)
            .SetEase(Ease.InOutQuint).OnComplete(() =>
            {
                blockingImage.gameObject.SetActive(false);
                DOTween.KillAll(gameObject);
            });
    }
}
