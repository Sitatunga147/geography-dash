using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private int deathTime = 4;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Death());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(deathTime);
        Destroy(this.gameObject);
    }
}
