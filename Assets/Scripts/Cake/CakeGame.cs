using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeGame : MonoBehaviour {
    public Main main;

    public List<GameObject> MainObjs = new List<GameObject>();

    public List<GameObject> Countobj = new List<GameObject>();

    private int preCountIndex;
    private int CountIndex;

    public GameObject Mainobj;

    public Transform MainPos;

    public List<AudioSource> sound = new List<AudioSource>();

    public void StopSound()
    {
        for (int i = 0; i < sound.Count; i++)
        {
            sound[i].Stop();
        }
    }

    public void GameStart()
    {
        main.ChangeText(21);
        StopSound();
        sound[0].Play();
        count = 0;

        CountIndex = Random.Range(0, MainObjs.Count + 3);
        if (CountIndex > 8)
        {
            CountIndex = 4;
        }

        if (preCountIndex != 20 && CountIndex == preCountIndex)
        {
            while (CountIndex == preCountIndex)
            {
                CountIndex = Random.Range(0, MainObjs.Count);
            }
        }
        Debug.Log(CountIndex + "," + preCountIndex);
        preCountIndex = CountIndex;

        Mainobj = Instantiate(MainObjs[CountIndex]);
        Mainobj.transform.position = MainPos.position;

        switch (CountIndex)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                CountObjActive(0); //2명
                break;
            case 4:
                CountObjActive(1); //3
                break;
            case 5:
            case 6:
            case 7:
            case 8:
                CountObjActive(2); //4
                break;
        }
    }

    private void CountObjActive(int num)
    {
        for (int i = 0; i < Countobj.Count; i++)
        {
            Countobj[i].SetActive(false);
            if (i == num)
            {
                Countobj[i].SetActive(true);
            }
        }
    }

    public Transform drag = null;
    private void Update()
    {
        if (Input.GetMouseButton(0) && main.click)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.collider.CompareTag("CakeGame") && drag == null)
            {
                hit.transform.localScale = Vector3.one * 1.5f;
                drag = hit.transform;
            }
            if (drag != null)
            {
                Vector3 temp = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
                temp = Camera.main.ScreenToWorldPoint(temp);
                temp.z = 0;
                drag.position = temp;
            }
        }
        if (Input.GetMouseButtonUp(0) && drag != null)
        {
            drag.localPosition = drag.GetComponent<Piece>().pos;
            drag = null;
        }
    }

    int count;
    public void GameCheck()
    {
        count++;
        switch (CountIndex)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                main.ChangeText(22);
                StopSound();
                sound[1].Play();
                if (count == 2)
                {
                    StartCoroutine(Finish());
                }
                break;
            case 4:
                main.ChangeText(23);
                StopSound();
                sound[2].Play();
                if (count == 3)
                {
                    StartCoroutine(Finish());
                }
                break;
            case 5:
            case 6:
            case 7:
            case 8:
                main.ChangeText(24);
                StopSound();
                sound[3].Play();
                if (count == 4)
                {
                    StartCoroutine(Finish());
                }
                break;
        }
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(1f);
        main.ChangeText(6);
        StopSound();
        sound[4].Play();
        sound[5].Play();
        yield return new WaitForSeconds(2f);
        Destroy(Mainobj);
        GameStart();
    }
}
