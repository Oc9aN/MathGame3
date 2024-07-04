using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBox : MonoBehaviour {

    private GameObject Box;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((Box == null || Box != col) && col.name == "RandomBox")
            Box = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (Box != null && Box.transform == col.transform)
        {
            Box = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Box != null)
            {
                Box.GetComponent<SpriteRenderer>().sprite = transform.GetComponent<SpriteRenderer>().sprite;
            }
            Destroy(gameObject);
        }
    }
}
