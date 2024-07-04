using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour {

    private GameObject coin;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if ((coin == null || coin != col) && col.name == transform.name && col.GetComponent<SpriteRenderer>().color.a != 1)
            coin = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (coin != null && coin.transform == col.transform)
        {
            coin = null;
        }
    }

    MoneyGame moneyGame;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (coin != null)
            {
                moneyGame = coin.GetComponentInParent<MoneyGame>();
                coin.GetComponent<SpriteRenderer>().color = Vector4.one;
                moneyGame.FinishCehck(int.Parse(coin.name));
            }
            Destroy(gameObject);
        }
    }
}
