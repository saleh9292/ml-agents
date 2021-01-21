using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenrator : MonoBehaviour
{

    public GameObject[] pathsobjects;
    public int pathLegnth = 10;
    public GameObject goal;
    public GameObject target;
    public List <Transform> targetlocations= new List<Transform>();
    public GameObject empty;
    private int n = 0;





    // Start is called before the first frame update
    void Start()
    {

        




    }


    public void CreatePath()

    {
        //targetlocations = new Transform[pathLegnth + 1];
        // Debug.Log(targetlocations.Length);
     



        GameObject tmp;
        tmp = Instantiate(empty);
        tmp.transform.parent = transform;
        tmp.transform.localPosition = new Vector3(0, 0, 0);
        Transform tmp2 = transform;



        //Debug.Log(Random.Range(0, pathsobjects.Length ));
        tmp = Instantiate(pathsobjects[Random.Range(0, pathsobjects.Length)]);
        tmp.transform.parent = transform;
        tmp.transform.localPosition = tmp2.localPosition + tmp2.GetChild(0).localPosition;
        tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
        tmp.layer = 9;


        targetlocations.Add( tmp2.GetChild(0).transform);

        tmp2 = tmp.transform;

        for (int objectindex = 1; objectindex < pathLegnth; objectindex++)
        {


            //Debug.Log(Random.Range(0, pathsobjects.Length ));
            tmp = Instantiate(pathsobjects[Random.Range(0, pathsobjects.Length)]);
            tmp.transform.parent = transform;
            tmp.transform.localPosition = tmp2.localPosition + tmp2.GetChild(1).localPosition;
            tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
            tmp.layer = 9;


            targetlocations.Add(    tmp2.GetChild(1).transform);

            tmp2 = tmp.transform;


        }

        tmp = Instantiate(goal);
        tmp.transform.parent = transform;
        tmp.transform.localPosition = tmp2.localPosition + tmp2.GetChild(1).localPosition + new Vector3(0, 0, 4);
        tmp.transform.localRotation = Quaternion.Euler(0, 0, 0);
        tmp.layer = 9;
        targetlocations.Add(  tmp2.GetChild(1).transform);


        n = 0;

        target.transform.position = targetlocations[n].position + new Vector3(0, 1, 0);

        NextTarget();
    }


    public void DestroyPath()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        targetlocations.Clear();




    }


    public bool NextTarget()
    {

        n++;

    if (n== targetlocations.Count)
        {
            return true;
        }
        else { 
        target.transform.position = targetlocations[n].position + new Vector3(0, 1, 0);


        return false;
        }

    }    

    // Update is called once per frame
    void Update()
    {



        if (Input.GetKeyDown(KeyCode.D))
        {

            bool a = NextTarget();

            if (a == true)
            {
                DestroyPath();
                CreatePath();


            }


        }








    }
}




