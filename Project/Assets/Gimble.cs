using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
    }


}
