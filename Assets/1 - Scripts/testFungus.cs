using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class testFungus : MonoBehaviour
{
    public Flowchart fc;
    public string blockName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        fc.ExecuteBlock(blockName);

        gameObject.SetActive(false);
    }

    public void RecibirMensaje(string mens)
    {
        Debug.Log(mens);
    }

}
