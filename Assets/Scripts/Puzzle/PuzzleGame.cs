using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGame : MonoBehaviour {
    public Main main;

    public GameObject LevelUI;

    public List<GameObject> Levels = new List<GameObject>();

    //public List<Sprite> LeftUpSprite = new List<Sprite>();
    public List<string> LeftDownString = new List<string>();
    public List<Sprite> LeftDownSprite = new List<Sprite>();
    public List<GameObject> MainPuzzle = new List<GameObject>();

    public SpriteRenderer LeftUpRenderer;
    //public SpriteRenderer LeftDownRenderer;
    public Text LeftDownText;
    public Transform MainRenderer;

    public Transform point;

    public List<AudioSource> sound = new List<AudioSource>();

    public GameObject Line;

    public GameObject BGBox;


    public void StopSound()
    {
        for (int i = 0; i < sound.Count; i++)
        {
            sound[i].Stop();
        }
    }

    private int CurrentLevel;
    public void GameStart(int level)
    {
        check = 0;

        CurrentLevel = level;
        LevelUI.SetActive(false);
        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].SetActive(false);
            if (i + 1 == CurrentLevel)
                Levels[i].SetActive(true);
        }
    }


    public GameObject MainObj;
    public void SelectPuzzle(int PuzzleNum)
    {
        LeftDownText.gameObject.SetActive(true);
        main.ChangeText(25);
        BGBox.SetActive(true);
        Line.SetActive(true);
        sound[0].Play();
        Click = false;
        LeftUpRenderer.gameObject.SetActive(true);
        MainRenderer.gameObject.SetActive(true);

        Levels[CurrentLevel - 1].SetActive(false);

        LeftDownText.text = LeftDownString[PuzzleNum];
        LeftUpRenderer.sprite = LeftDownSprite[PuzzleNum];
        MainObj = Instantiate(MainPuzzle[PuzzleNum]);
        MainObj.transform.parent = transform;
        MainObj.transform.position = MainRenderer.position;

        startani = StartCoroutine(StartAni());
    }

    private Coroutine startani;
    private IEnumerator StartAni()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < MainObj.transform.childCount / 2; i++)
        {
            Transform T = MainObj.transform.GetChild(i).transform;

            Vector3 MovePos = Vector3.zero;
            if (i <= 5)
            {
                MovePos = point.position;
                MovePos.y += (T.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2);
                if (i > 0)
                {
                    for (int j = i - 1; j >= 0; j--)
                        MovePos.y += MainObj.transform.GetChild(j).transform.GetComponent<SpriteRenderer>().bounds.size.y;
                }
            }
            else
            {
                MovePos = point.position + Vector3.right * 2f;
                MovePos.y += (T.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2);
                if (i > 6)
                {
                    for (int j = i - 1; j >= 6; j--)
                        MovePos.y += MainObj.transform.GetChild(j).transform.GetComponent<SpriteRenderer>().bounds.size.y;
                }
            }

            while (T.position != MovePos)
            {
                T.position = Vector3.MoveTowards(T.position, MovePos, 0.2f);
                T.GetComponent<Piece>().ReSetPos();
                yield return new WaitForFixedUpdate();
            }
        }
        Click = true;
        yield return null;
    }

    public bool Click = false;
    public Transform drag = null;
    private void Update()
    {
        if (Input.GetMouseButton(0) && Click && main.click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);


            if (hit.collider != null && hit.collider.CompareTag("PuzzleGame") && drag == null)
            {
                drag = hit.collider.transform;
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

    public int check = 0;
    public void GameCheck()
    {
        check += 1;
        //Debug.Log(check + ", " + MainObj.transform.childCount / 2);
        if (check == MainObj.transform.childCount / 2)
        {
            check = 0;
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        main.ChangeText(6);
        StopSound();
        sound[1].Play();
        sound[2].Play();
        yield return new WaitForSeconds(2f);
        main.TextBox.SetActive(false);
        LeftDownText.gameObject.SetActive(false);
        Line.SetActive(false);
        LevelUI.SetActive(false);
        Destroy(MainObj);
        LeftUpRenderer.gameObject.SetActive(false);
        MainRenderer.gameObject.SetActive(false);
        for (int i = 0; i < Levels.Count; i++)
        {
            Levels[i].SetActive(false);
            if (i + 1 == CurrentLevel)
                Levels[i].SetActive(true);
        }
    }
}
