using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGoods : MonoBehaviour {
    public int id;

    public List<int> price = new List<int>();
    public int priceint;

    public int level;

    public void OnEnable()
    {
        priceint = Random.Range(0, price.Count);
        transform.GetComponentInChildren<Text>().text = price[priceint].ToString();
    }
}
