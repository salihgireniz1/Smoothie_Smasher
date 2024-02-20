using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveWall : MonoBehaviour
{
    Rigidbody rb;
    int factor = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.position += Vector3.left * Time.deltaTime * factor;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.position += Vector3.right * Time.deltaTime * factor;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.position += Vector3.forward * Time.deltaTime * factor;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.position += Vector3.back * Time.deltaTime * factor;
        }

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("ssssssss");
    //    if (other.gameObject.layer == 18)
    //    {
    //        //Debug.Log("asdafa");
    //        StartCoroutine(delayedParticle(other.gameObject));

    //    }
    //}


    IEnumerator delayedParticle(GameObject other)
    {
        yield return new WaitForSeconds(0.1f);
        GameObject particle = other.transform.GetChild(6).gameObject;
        particle.transform.parent = null;
        particle.GetComponent<ParticleSystem>().Play();
        particle.transform.DOJump(transform.position, 3, 1, 0.25f);

        Destroy(other.transform.parent.gameObject, 0.05f);

    }
}
