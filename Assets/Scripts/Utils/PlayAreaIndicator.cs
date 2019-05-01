using UnityEngine;

public class PlayAreaIndicator : MonoBehaviour
{
    public bool ActiveAfterStart = false;

    void Start()
    {
        gameObject.SetActive(ActiveAfterStart);
    }
}
