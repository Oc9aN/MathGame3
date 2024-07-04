using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGame : MonoBehaviour {
    public Main main;

    public GameObject LevelUI;

    public List<GameObject> Levels = new List<GameObject>();

    public List<Sprite> GoodsSprites = new List<Sprite>();

    public SpriteRenderer GoodsRender;

    public Text PriceText;
    public GameObject InGameUI;

    public List<Transform> Point = new List<Transform>();

    public List<SpriteRenderer> CoinRender = new List<SpriteRenderer>();
    public List<Sprite> CoinSprites = new List<Sprite>();

    public GameObject EmptyCoin;
    public GameObject CoinPrefab;

    private int CurrentLevel;

    private MoneyGoods Goods;

    public List<AudioSource> Numder1 = new List<AudioSource>(); //1~9
    public List<AudioSource> Numder2 = new List<AudioSource>(); //단위
    public AudioSource Numder3; //원

    private int AudioNum1; //1~9 1
    private int AudioNum1_; //단위 0 = 십 1 = 백 2 = 천 3 = 만
    private int AudioNum2; //1~9 2
    private int AudioNum2_; //단위

    public List<AudioSource> sound = new List<AudioSource>();

    public void StopSound()
    {
        for (int i = 0; i < sound.Count; i++)
        {
            sound[i].Stop();
        }
    }

    public void GameStart(int level)
    {
        main.ChangeText(0);
        StopSound();
        sound[0].Play();
        CurrentLevel = level;
        LevelUI.SetActive(false);
        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].SetActive(false);
            if (i + 1 == CurrentLevel)
                Levels[i].SetActive(true);
        }
    }

    private int[] coin = new int[4];
    public void SelectGoods(MoneyGoods Selected)
    {
        main.ChangeText(5);
        StopSound();
        sound[1].Play();
        Num1 = true;
        Num2 = true;
        Money = 0;
        for (int i = 0; i < EmptyCoins.Count; i++)
        {
            Destroy(EmptyCoins[i]);
        }
        EmptyCoins.RemoveRange(0, EmptyCoins.Count);
        Levels[CurrentLevel - 1].SetActive(false);

        InGameUI.SetActive(true);
        PriceText.gameObject.SetActive(true);

        Goods = Selected;
        GoodsRender.sprite = GoodsSprites[(CurrentLevel - 1) * 10 + Goods.id];
        //GoodsRender.size = new Vector2(100, 100);

        PriceText.text = Goods.price[Goods.priceint].ToString();

        switch (CurrentLevel)
        {
            case 1:
                CoinRender[0].sprite = CoinSprites[0];
                CoinRender[1].sprite = CoinSprites[1];
                CoinRender[2].sprite = CoinSprites[2];
                CoinRender[3].sprite = CoinSprites[3];
                coin[0] = Goods.price[Goods.priceint] / 500;
                coin[1] = Goods.price[Goods.priceint] % 500 / 100;
                AudioNum1 = coin[0] * 5 + coin[1];
                AudioNum1_ = 1;
                coin[2] = Goods.price[Goods.priceint] % 500 % 100 / 50;
                coin[3] = Goods.price[Goods.priceint] % 500 % 100 % 50 / 10;
                AudioNum2 = coin[2] * 5 + coin[3];
                AudioNum2_ = 0;
                SetEmptyCoin(3);
                break;
            case 2:
                CoinRender[0].sprite = CoinSprites[2];
                CoinRender[1].sprite = CoinSprites[3];
                CoinRender[2].sprite = CoinSprites[4];
                CoinRender[3].sprite = CoinSprites[5];
                coin[0] = Goods.price[Goods.priceint] / 5000;
                coin[1] = Goods.price[Goods.priceint] % 5000 / 1000;
                AudioNum1 = coin[0] * 5 + coin[1];
                AudioNum1_ = 2;
                coin[2] = Goods.price[Goods.priceint] % 5000 % 1000 / 500;
                coin[3] = Goods.price[Goods.priceint] % 5000 % 1000 % 500 / 100;
                AudioNum2 = coin[2] * 5 + coin[3];
                AudioNum2_ = 1;
                SetEmptyCoin(5);
                break;
            case 3:
                CoinRender[0].sprite = CoinSprites[4];
                CoinRender[1].sprite = CoinSprites[5];
                CoinRender[2].sprite = CoinSprites[6];
                CoinRender[3].sprite = CoinSprites[7];
                coin[0] = Goods.price[Goods.priceint] / 50000;
                coin[1] = Goods.price[Goods.priceint] % 50000 / 10000;
                AudioNum1 = coin[0] * 5 + coin[1];
                AudioNum1_ = 3;
                coin[2] = Goods.price[Goods.priceint] % 50000 % 10000 / 5000;
                coin[3] = Goods.price[Goods.priceint] % 50000 % 10000 % 5000 / 1000;
                AudioNum2 = coin[2] * 5 + coin[3];
                AudioNum2_ = 2;
                SetEmptyCoin(7);
                break;
        }
        for (int i = 0; i < CoinRender.Count; i++)
        {
            CoinRender[i].GetComponent<BoxCollider2D>().size = CoinRender[i].bounds.size;
        }
    }

    public List<GameObject> EmptyCoins = new List<GameObject>();
    private void SetEmptyCoin(int num)
    {
        for (int i = 0; i < coin.Length; i++)
        {
            for (int j = 0; j < coin[i]; j++)
            {
                GameObject CoinObj = Instantiate(EmptyCoin.gameObject, Point[i]);
                EmptyCoins.Add(CoinObj);
                CoinObj.GetComponent<SpriteRenderer>().sprite = CoinSprites[num - i];
                CoinObj.name = CoinObj.GetComponent<SpriteRenderer>().sprite.name;
                OutLineSetting(CoinObj, num - i);
                CoinObj.transform.position += Vector3.right * 3f * j;
            }
        }
    }

    private void OutLineSetting(GameObject coin, int Money)
    {
        if (Money <= 3)
        {
            coin.transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 1f);
        }
        else
        {
            coin.transform.GetChild(0).localScale = new Vector3(0.8f, 0.4f, 1f);
        }
    }

    private GameObject Coinprefab;
    private Transform drag = null;
    private void Update()
    {
        if (Input.GetMouseButton(0) && main.click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            
            if (hit.collider != null && hit.collider.CompareTag("MoneyGame") && drag == null)
            {
                Coinprefab = Instantiate(CoinPrefab);
                Coinprefab.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;
                Coinprefab.name = Coinprefab.GetComponent<SpriteRenderer>().sprite.name;
                Coinprefab.transform.localScale = Vector3.one * 1.5f;
                drag = Coinprefab.transform;
            }
            if (drag != null)
            {
                Vector3 temp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
                temp = Camera.main.ScreenToWorldPoint(temp);
                temp.z = 0;
                drag.position = temp;
            }
        }
    }

    private int Money;
    private bool Num1 = true;
    private bool Num2 = true;
    public void FinishCehck(int num)
    {
        Money += num;
        Debug.Log(Money + "," + AudioNum1 + "," + Money / 100);
        switch (CurrentLevel)
        {
            case 1:
                if (Money / 100 == AudioNum1 && Num1)
                    StartCoroutine(CountingMoney(1));
                else if (Money % 100 / 10 != 0 && Money % 100 / 10 == AudioNum2 && Num2)
                    StartCoroutine(CountingMoney(2));
                break;
            case 2:
                if (Money / 1000 == AudioNum1 && Num1)
                    StartCoroutine(CountingMoney(1));
                else if (Money % 1000 / 100 != 0 && Money % 1000 / 100 == AudioNum2 && Num2)
                    StartCoroutine(CountingMoney(2));
                break;
            case 3:
                if (Money / 10000 == AudioNum1 && Num1)
                    StartCoroutine(CountingMoney(1));
                else if (Money % 10000 / 1000 != 0 && Money % 10000 / 1000 == AudioNum2 && Num2)
                    StartCoroutine(CountingMoney(2));
                break;
        }
        if (Goods.price[Goods.priceint] == Money)
        {
            StartCoroutine(HowMuch());
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator CountingMoney(int i)
    {
        Debug.Log("a");
        if (i == 1)
        {
            Num1 = false;
            Numder1[AudioNum1 - 1].Play();
            yield return new WaitForSeconds(0.4f);
            Numder2[AudioNum1_].Play();
        }
        else if (i == 2)
        {
            Num2 = false;
            Numder1[AudioNum2 - 1].Play();
            yield return new WaitForSeconds(0.4f);
            Numder2[AudioNum2_].Play();
        }
        yield return new WaitForSeconds(0.4f);
        Numder3.Play();
        yield return 0;
    }

    private IEnumerator HowMuch()
    {
        yield return new WaitForSeconds(2f);
        Numder1[AudioNum1 - 1].Play();
        yield return new WaitForSeconds(0.4f);
        Numder2[AudioNum1_].Play();
        if (AudioNum2 != 0)
        {
            yield return new WaitForSeconds(0.8f);
            Numder1[AudioNum2 - 1].Play();
            yield return new WaitForSeconds(0.4f);
            Numder2[AudioNum2_].Play();
        }
        yield return new WaitForSeconds(0.4f);
        Numder3.Play();
        yield return new WaitForSeconds(0.4f);
        main.ChangeText(6);
        sound[4].Play();
        sound[2].Play();
        yield return new WaitForSeconds(2f);
        main.ChangeText(7);
        sound[3].Play();
        yield return 0;
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(10f);
        main.ChangeText(0);
        StopSound();
        sound[0].Play();
        InGameUI.SetActive(false);
        PriceText.gameObject.SetActive(false);
        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].SetActive(false);
            if (i + 1 == CurrentLevel)
                Levels[i].SetActive(true);
        }
        yield return 0;
    }
}
