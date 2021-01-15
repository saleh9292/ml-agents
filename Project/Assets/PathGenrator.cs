using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenrator : MonoBehaviour
{

    public GameObject[] pathsobjects;
    public int pathLegnth = 10;




    // Start is called before the first frame update
    void Start()
    {

        Transform tmp2 = transform;

        for (int objectindex=0; objectindex <pathLegnth; objectindex++)
        {
            
        GameObject tmp;
        Debug.Log(Random.Range(0, pathsobjects.Length ));
        tmp = Instantiate(pathsobjects[Random.Range(0, pathsobjects.Length)]);
        tmp.transform.parent = transform;
        tmp.transform.localPosition =  tmp2.localPosition+ tmp2.GetChild(1).localPosition;
        tmp.transform.localRotation = Quaternion.Euler(0,0, 0);
        tmp.layer = 9;
            tmp2 = tmp.transform;


        }














    }

    // Update is called once per frame
    void Update()
    {










        
    }
}
