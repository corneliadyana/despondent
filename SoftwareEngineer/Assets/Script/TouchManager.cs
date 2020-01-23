using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public float movespeed;
    public float jumpforce;
    private float stopspeed = 0;
    private float drag_distance;
    private bool walking;
    private bool walkingl;
    public bool ground;
    public float maxwidht;

    private Rigidbody2D rb;
    private SpriteRenderer sp;
    private Animator anim;
    public GameManager game;
    public AudioSource walk;
    private Vector2 fp;
    private Vector2 lp;
    public Vector2 jump;
    private bool gamedone = false;
    private Color alpha;
    void Start()
    {

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        alpha = sp.color;
        game = FindObjectOfType<GameManager>();
        drag_distance = Screen.width * 10 / 200;
    }

    void Update()
    {
        if (!gamedone)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    lp = touch.position;
                    if (Mathf.Abs(lp.x - fp.x) > drag_distance || Mathf.Abs(lp.y - fp.y) > drag_distance)
                    {

                        //drag
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {
                            //swip horizontal
                            if (lp.x > fp.x)
                            {
                                walking = true;
                                walkingl = false;

                            }
                            else
                            {
                                walking = false;
                                walkingl = true;

                            }
                        }
                        else if (lp.y > fp.y)
                        {
                            if (ground)
                            {
                                rb.velocity = new Vector3(0f, jumpforce, 0f);

                            }
                        }
                    }
                    else
                    {
                        stop();
                    }
                }
            }

            if (walking || walkingl)
            {
                if (walking && !walkingl)
                {
                    transform.Translate(movespeed * Time.deltaTime, 0, 0);
                    sp.flipX = true;
                    anim.SetBool("Walking", true);

                }
                else if (walkingl && !walking)
                {
                    anim.SetBool("Walking", true);
                    transform.Translate(-movespeed * Time.deltaTime, 0, 0);
                    sp.flipX = false;

                }

                if (ground && walk.isPlaying == false)
                {
                    walk.Play();

                }
                else if (!ground && walk.isPlaying == true)
                {
                    walk.Stop();
                }

            }
        }
        else {
            alpha.a = alpha.a - .5f * Time.deltaTime;
            sp.color = alpha;
        }
    }


    public void stop()
    {
        walking = false;
        walkingl = false;
        anim.SetBool("Walking", false);
        if(walk.isPlaying == true) walk.Stop();
        transform.Translate(stopspeed, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Mplat")
        {
            ground = true;
            if (collision.gameObject.tag == "Mplat")
            {
                transform.parent = collision.transform;
            }
        }
        if (collision.gameObject.tag == "boundary")
        {

            stop();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Mplat")
        {
            ground = false;
        }
        if (collision.gameObject.tag == "Mplat")
        {
            transform.parent = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "Mplat")
        {
            ground = true;
            if (collision.gameObject.tag == "Mplat")
            {
                transform.parent = collision.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "checkpoint")
        {
            StartCoroutine(win());
        }
        else if (collision.gameObject.tag == "killzone") {
            game.death();
            Destroy(this.gameObject);
        }
    }

    IEnumerator win(){
        stop();
        gamedone = true;
        yield return new WaitForSeconds(2f);
        game.Win();
    }
}
