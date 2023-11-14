using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Fungus;

public class MainMenu_manager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Image playBtn;
    [SerializeField] private GameObject configBtn;
    [SerializeField] private GameObject volumeBtn;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private GameObject retunrConfigBtn;
    [SerializeField] private GameObject exitBtn;
    [SerializeField] private GameObject creditsBtn;


    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject configPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Game Components")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject joystickCanva;
    [SerializeField] private GameObject buttonsCanva;
    [SerializeField] private GameObject joystickFlotating;
    
    [Header("Others")]
    [SerializeField] private GameObject gameTitle;
    [SerializeField] private Image backgroundColor;
    [SerializeField] private Image screenColor;
    [SerializeField] private AudioSource SFXClick;

    private Color endColor = new Color(185, 185, 185, 0);
    private Color redColor = new Color(0.1921569f, 0, 0.003921569f, 0);
    private Color blackColor = new Color(0, 0, 0, 0);

    void Start()
    {
        screenColor.gameObject.SetActive(true);
        screenColor.DOColor(redColor, 6f).OnComplete(() =>
        {
            screenColor.gameObject.SetActive(false);
        });

        Tweener playBtnAnim = playBtn.DOColor(endColor, 2f);
        playBtnAnim.SetEase(Ease.InOutSine);
        playBtnAnim.SetLoops(-1,LoopType.Yoyo);
        playBtnAnim.Play();
    }

    //public void IntroAnim()
    //{
    //    screenColor.enabled = true;
    //    screenColor.DOColor(redColor, 6f).OnComplete(() =>
    //    {
    //        screenColor.gameObject.SetActive(false);
    //    });
    //}

    public void OpenConfigAnim()
    {
        SFXClick.Play();

        mainMenuPanel.SetActive(false);
        configPanel.SetActive(true);
    }

    public void ExitConfigAnim()
    {
        SFXClick.Play();

        mainMenuPanel.SetActive(true);
        configPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        SFXClick.Play();
        creditsPanel.SetActive(true);
        configPanel.SetActive(false);
    }

    public void CloseCredits()
    {
        SFXClick.Play();
        creditsPanel.SetActive(false);
        configPanel.SetActive(true);
    }

    public void PlayAnim()
    {
        SFXClick.Play();
        mainMenuPanel.transform.DOScale(new Vector3(7, 7, 7), 3);
        backgroundColor.DOColor(blackColor, 2).OnComplete(() =>
        {
            joystickCanva.gameObject.SetActive(true);
            joystickFlotating.gameObject.SetActive(true);
            buttonsCanva.gameObject.SetActive(true);
            player.gameObject.SetActive(true);

            gameObject.SetActive(false);
            DOTween.KillAll(gameObject);
        });
    }


}
