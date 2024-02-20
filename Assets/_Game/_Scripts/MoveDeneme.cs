using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveDeneme : MonoBehaviour
{
    public List<GameObject> objects = new();
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            direction += Vector3.forward * 5;
            StartCoroutine(moving(direction));
        }
    }

    IEnumerator moving(Vector3 direction)
    {
        int i = 0;
        //Vector3 direction = transform.position + Vector3.forward * 10;
        while (i < objects.Count)
        {
            objects[i].transform.DOJump(direction, 3, 1, 0.5f);
            i++;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
