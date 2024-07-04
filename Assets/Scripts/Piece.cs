using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public Vector3 pos;

    private void Awake()
    {
        pos = transform.localPosition;
    }

    public void ReSetPos()
    {
        pos = transform.localPosition;
    }
}
