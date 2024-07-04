using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGame : MonoBehaviour {

    public Main main;

    private int[,] Pattern = new int[5, 6];

    public List<Sprite> Fruits = new List<Sprite>();
    public List<SpriteRenderer> FruitsRenderer = new List<SpriteRenderer>();

    public Sprite RandomBox;

    public List<GameObject> FruitsPrefab = new List<GameObject>();

    public List<GameObject> BoxRender = new List<GameObject>();

    public GameObject InGameUI;

    public List<AudioSource> sound = new List<AudioSource>();

    public void StopSound()
    {
        for (int i = 0; i < sound.Count; i++)
        {
            sound[i].Stop();
        }
    }

    private void Awake()
    {
        int[] array0 = { 0, 1, 0, 1, 0, 1};
        MakePattern(0, array0);

        int[] array1 = { 0, 0, 1, 1, 0, 0};
        MakePattern(1, array1);

        int[] array2 = { 0, 1, 2, 0, 1, 2};
        MakePattern(2, array2);

        int[] array3 = { 0, 0, 1, 0, 0, 1 };
        MakePattern(3, array3);

        int[] array4 = { 0, 1, 1, 0, 1, 1 };
        MakePattern(4, array4);
    }

    private void MakePattern(int index, int[] arr)
    {
        for (int i = 0; i < 6; i++)
        {
            Pattern[index, i] = arr[i];
        }
    }

    public Vector3 TrainPos;
    public GameObject Train;
    
    private int PatternNumder;
    private Coroutine trainMove;
    public void GameStart()
    {
        main.ChangeText(19);
        StopSound();
        sound[1].Play();
        ButtonEnable = true;
        Train.transform.position = new Vector3(16f, 0f, 0f);
        InGameUI.SetActive(true);
        for (int i = 0; i < BoxRender.Count; i++)
        {
            BoxRender[i].SetActive(true);
        }
        
        for (int i = 0; i < 4; i++)
        {
            //FruitsRenderer[i].gameObject.SetActive(false);
            FruitsRenderer[i].sprite = Fruits[Pattern[PatternNumder, i]];
        }
        //FruitsRenderer[4].gameObject.SetActive(false);
        //FruitsRenderer[5].gameObject.SetActive(false);
        FruitsRenderer[4].sprite = RandomBox;
        FruitsRenderer[5].sprite = RandomBox;
        trainMove = StartCoroutine(TrainMove());
    }

    public List<Transform> points = new List<Transform>();
    private IEnumerator TrainMove()
    {
        while (Train.transform.position != new Vector3(-10f, 0f, 0f))
        {
            Train.transform.position = Vector3.MoveTowards(Train.transform.position, new Vector3(-10f, 0f, 0f), 0.2f);
            yield return new WaitForFixedUpdate();
        }
        //while (Train.transform.position != Vector3.zero)
        //{
        //    Train.transform.position = Vector3.MoveTowards(Train.transform.position, Vector3.zero, 0.2f);
        //    yield return new WaitForFixedUpdate();
        //}
        //for (int i = 0; i < 4; i++)
        //{
        //    FruitsRenderer[i].transform.position = BoxRender[Pattern[PatternNumder, i]].transform.position;
        //    FruitsRenderer[i].gameObject.SetActive(true);
        //    while (FruitsRenderer[i].transform.position != points[i].position)
        //    {
        //        FruitsRenderer[i].transform.position = Vector3.MoveTowards(FruitsRenderer[i].transform.position, points[i].position, 0.2f);
        //        yield return new WaitForFixedUpdate();
        //    }
        //    yield return new WaitForSeconds(0.5f);
        //}
        //FruitsRenderer[4].gameObject.SetActive(true);
        //FruitsRenderer[5].gameObject.SetActive(true);
        yield return null;
    }

    private GameObject Fruitprefab;
    private Transform drag = null;
    private void Update()
    {
        if (Input.GetMouseButton(0) && main.click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            
            if (hit.collider != null && hit.collider.CompareTag("TrainGame") && drag == null)
            {
                Fruitprefab = Instantiate(FruitsPrefab[int.Parse(hit.collider.name)]);
                Fruitprefab.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;
                Fruitprefab.name = Fruitprefab.GetComponent<SpriteRenderer>().sprite.name;
                Fruitprefab.transform.localScale = Vector3.one * 1.5f;
                drag = Fruitprefab.transform;
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

    private Coroutine endAni;
    public bool ButtonEnable = true;
    public void FinishButton()
    {
        if (!ButtonEnable)
            return;
        int count = 0;
        for (int i = 4; i < 6; i++)
        {
            if (FruitsRenderer[i].sprite == Fruits[Pattern[PatternNumder, i]])
                count++;
        }
        if (count == 2)
        {
            endAni = StartCoroutine(EndAni());
            ButtonEnable = false;
            Debug.Log("완료");
        }
        else
        {
            FruitsRenderer[4].sprite = RandomBox;
            FruitsRenderer[5].sprite = RandomBox;
            main.ChangeText(31);
            sound[3].Play();
            sound[4].Play();
            Debug.Log("실패");
        }
    }

    private IEnumerator EndAni()
    {
        PatternNumder++;
        if (PatternNumder == 5)
            PatternNumder = 0;
        main.ChangeText(20);
        StopSound();
        sound[2].Play();
        sound[5].Play();
        yield return new WaitForSeconds(1.2f);
        sound[0].Play();
        yield return new WaitForSeconds(1f);
        while (Train.transform.position != Vector3.left * 30f)
        {
            Train.transform.position = Vector3.MoveTowards(Train.transform.position, Vector3.left * 30f, 0.2f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        GameStart();
    }

    public void StopCoroutines()
    {
        Train.transform.position = TrainPos;
        if (endAni != null)
            StopCoroutine(endAni);
        if (trainMove != null)
            StopCoroutine(trainMove);
    }
}