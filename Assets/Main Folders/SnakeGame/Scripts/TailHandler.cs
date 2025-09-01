using System.Collections.Generic;
using UnityEngine;

public static class TailHandler
{
    public static List<GameObject> tails = new List<GameObject>();

    public static void MoveTails()
    {
        if (tails.Count == 0)
            return;
        foreach (GameObject tail in tails)
        {
            if (FindIndex(tail) == 0)
            {
                tail.GetComponent<TailScript>().lastPos = tail.transform.position;
                tail.transform.position = GameObject.Find("HeadOfSnake").GetComponent<MovementScript>().lastPos;
            }

            if (FindIndex(tail) != 0)
            {
                tail.GetComponent<TailScript>().lastPos = tail.transform.position;
                tail.transform.position = tails[FindIndex(tail) - 1].GetComponent<TailScript>().lastPos;
            }
        }
    }
    
    public static int FindIndex(GameObject tail)
    {
        return tails.IndexOf(tail);
    }
}