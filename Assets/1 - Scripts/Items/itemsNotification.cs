using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class itemsNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemDescription;
    public Image itemSprite;
    [SerializeField] private float notificationTime;

    //private Item lastPickedItem;
    private PlayerManager playerManager;

    private void Start()
    {
        gameObject.SetActive(false);
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public void DisplayNearByItem(Item curItem, bool op)
    {
        itemSprite.sprite = curItem.GetComponent<SpriteRenderer>().sprite;
        itemDescription.text = curItem.descriptionText;

        if(op)
        {
            StartCoroutine(disableNotification(notificationTime));
        }
    }
    
    IEnumerator disableNotification(float time)
    {


        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);
    }
}
