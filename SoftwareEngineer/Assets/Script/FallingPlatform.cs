using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {
    public Rigidbody2D rb;
    private Vector3 startingPos;
    private bool shake = false;
    private bool falling = false;
    // Use this for initialization

    void Awake()
    {
        startingPos.x = transform.localPosition.x;
        startingPos.y = transform.localPosition.y;
    }

    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (shake)
        {
            transform.localPosition = startingPos + Random.insideUnitSphere * .1f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!falling)
            {
                shake = true;
                StartCoroutine(fall());
            }
        }
    }

    public IEnumerator fall() {
        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
        shake = false;
        falling = true;
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
