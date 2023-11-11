using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI circleClearedText;
    [SerializeField] private TextMeshProUGUI remainingCircles;

    private void Awake()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();

        _cam.m_Lens.OrthographicSize = 0f;

        StartCoroutine(waitSec(1.7f));

        _image.gameObject.SetActive(true);
        circleClearedText.gameObject.SetActive(false);
        remainingCircles.gameObject.SetActive(false);
    }

    private IEnumerator waitSec(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CamTransition();
    }

    private void CamTransition()
    {
        _image.DOColor(Color.clear, 2f);
        DOTween.To(() => _cam.m_Lens.OrthographicSize, x => _cam.m_Lens.OrthographicSize = x, 8f, 2f)
            .SetEase(Ease.InOutQuint).OnComplete(() =>
            {
                _image.gameObject.SetActive(false);

                DOTween.KillAll(gameObject);
            });
    }

    public void CircleClearedTransition(int circleNum)
    {
        remainingCircles.text = circleNum + " Out Of 9";
        circleClearedText.gameObject.SetActive(true);
        remainingCircles.gameObject.SetActive(true);

        circleClearedText.DOColor(Color.white, 3f).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            _image.gameObject.SetActive(true);
            _image.DOColor(Color.black, 3f).OnComplete(() => 
            {
                StartCoroutine(WaitForTransition(3f));
            
            });
        });
    }

    private IEnumerator WaitForTransition(float waitTime)
    {
        remainingCircles.DOColor(Color.white, 3f).SetEase(Ease.InOutQuint);
        yield return new WaitForSeconds(waitTime);
        remainingCircles.DOColor(Color.clear, 2f).SetEase(Ease.InOutQuint);
        circleClearedText.DOColor(Color.clear, 3f).SetEase(Ease.InOutQuint).OnComplete(() =>
        {
            _image.DOColor(Color.clear, 2f).OnComplete(() =>
            {
                circleClearedText.gameObject.SetActive(false);
                _image.gameObject.SetActive(false);
                remainingCircles.gameObject.SetActive(false);
                DOTween.Kill(gameObject);
            });
        });

    }
}