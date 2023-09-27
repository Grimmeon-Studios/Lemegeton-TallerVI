using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class itemsNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemSprite;

    private Item lastPickedItem;
    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void ItemPickedUp()
    {
        lastPickedItem = playerManager.itemsHeld.Peek();

        itemSprite.sprite = lastPickedItem.GetComponent<SpriteRenderer>().sprite;


    }
}
