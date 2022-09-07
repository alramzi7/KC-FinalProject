using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.Find("player");
    }

    void Update()
    {
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, player.transform.position, 1000 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().HitPlayer();
            Destroy(gameObject);
        }
    }
}
