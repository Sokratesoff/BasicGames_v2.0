using UnityEngine;

public class TailScript : MonoBehaviour
{
    public Vector3 lastPos;

    private void Start()
    {
        lastPos = transform.position;
    }
}