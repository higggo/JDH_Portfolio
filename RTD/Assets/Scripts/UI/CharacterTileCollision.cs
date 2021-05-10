using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTileCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Tile" &&
            //collision.gameObject.layer != gameObject.layer &&
            collision.gameObject != gameObject)

        {
            collision.gameObject.GetComponent<Tile>().InRange();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Tile" &&
            //collision.gameObject.layer != gameObject.layer &&
            collision.gameObject != gameObject)
        {
            collision.gameObject.GetComponent<Tile>().OutRange();
        }
    }
}
