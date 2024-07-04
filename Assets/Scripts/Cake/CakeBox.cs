using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeBox : MonoBehaviour {

    public bool check = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("CakeGame") && !check)
        {
            check = true;
            transform.GetComponentInParent<CakeGame>().GameCheck();
            transform.GetComponentInParent<CakeGame>().drag = null;
            col.transform.position = transform.position + Vector3.up + Vector3.back;
            col.transform.localScale = Vector3.one;
            col.tag = "Untagged";
        }
    }

    private void OnDisable()
    {
        check = false;
    }
}
