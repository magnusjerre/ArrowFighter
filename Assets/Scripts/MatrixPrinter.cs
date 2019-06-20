using UnityEngine;
using System.Collections;

public class MatrixPrinter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("matrix:\n" + transform.localToWorldMatrix);
    }
}
