using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour {

    private GameObject puzzle = null;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == transform.name && transform.GetComponentInParent<PuzzleGame>().Click)
            puzzle = col.gameObject;
    }

    private void Update()
    {
        if (transform.GetComponentInParent<PuzzleGame>().Click && Input.GetMouseButtonUp(0))
        {
            if (puzzle != null)
            {
                transform.position = puzzle.transform.position;
                transform.tag = "Untagged";
                transform.GetComponentInParent<PuzzleGame>().drag = null;
                transform.GetComponentInParent<PuzzleGame>().GameCheck();
                puzzle = null;
            }
            else if (puzzle == null && transform.CompareTag("PuzzleGame"))
            {
                transform.transform.localPosition = transform.GetComponent<Piece>().pos;
                transform.GetComponentInParent<PuzzleGame>().drag = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == transform.name && transform.GetComponentInParent<PuzzleGame>().Click)
        {
            puzzle = null;
        }
    }
}
