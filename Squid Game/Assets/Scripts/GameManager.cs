using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool running = false;

    bool game_started = false;
    bool game_over = false;

    public GameObject player;
    public Animator player_animator;

    public GameObject toy;
    public Animator toy_animator;

    public GameObject Laser;

    public GameObject Camera;


    public ParticleSystem blood_splash;
    public GameObject blood;

    AudioSource source;

    public AudioClip step;
    public AudioClip shooting;
    public AudioClip hit;
    public AudioClip fall;

    float steps_counter;

    public GameObject ui_start;
    public GameObject ui_gameover;
    public GameObject ui_win;
    public Text ui_guide;

    float speed = 1;

    KeyCode key1 = 0, key2 = 0;

    void Start()
    {
        source = GetComponent<AudioSource>();
        ui_start.SetActive(true);

    }

    void Update()
    {
        if (running)
        {
            player.transform.position -= new Vector3(0, 0, 0.5f * Time.deltaTime);
            Camera.transform.position -= new Vector3(0, 0, 0.5f * Time.deltaTime);

            steps_counter += Time.deltaTime;
            if (steps_counter > .25f)
            {
                steps_counter = 0;
                source.PlayOneShot(step);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !game_started)
        {
            running = true;
            game_started = true;
            ui_start.SetActive(false);
            player_animator.SetTrigger("run");
            StartCoroutine(Sing());

        }

        if (Input.GetKeyDown(KeyCode.Space) && game_over)
        {
            SceneManager.LoadScene("Game");
        }

        if (Input.GetKey(key1) && Input.GetKey(key2) && !game_over)
        {
            running = false;
            player_animator.speed = 0;
        }
        else if ((Input.GetKeyUp(key1) || Input.GetKeyUp(key2)) && !game_over)
        {
            running = true;
            player_animator.speed = 1;
        }

    }

    IEnumerator Sing()
    {
        toy.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(4.5f / speed);

        key1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));
        key2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));

        ui_guide.text = "PRESS " + key1 + " + " + key2 + " TO STOP";
        toy_animator.SetTrigger("look");
        yield return new WaitForSeconds(2 / speed);
        // CHECK IF THE PLAYER IS STILL MOVING
        if (running)
        {
            Debug.Log("SHOOT THE PLAYER");
            GameObject new_Laser = Instantiate(Laser);
            new_Laser.transform.position = toy.transform.GetChild(0).transform.position;
            game_over = true;
            source.PlayOneShot(shooting);

        }

        ui_guide.text = "";

        yield return new WaitForSeconds(2 / speed);
        toy_animator.SetTrigger("idle");
        yield return new WaitForSeconds(1 / speed);
        toy.GetComponent<AudioSource>().Stop();

        speed = speed * 1.10f;
        toy.GetComponent<AudioSource>().pitch = speed;

        if (!game_over) StartCoroutine(Sing());

    }

    public void HitPlayer()
    {
        running = false;
        player_animator.SetTrigger("idle");
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 2, 2);
        player.GetComponent<Rigidbody>().angularVelocity = new Vector3(3, 0, 0);
        Camera.GetComponent<Animator>().Play("camera_lose");
        blood_splash.Play();
        StartCoroutine(ShowBlood());
        source.PlayOneShot(hit);
    }

    IEnumerator ShowBlood()
    {
        yield return new WaitForSeconds(.3f);
        ui_gameover.SetActive(true);
        source.PlayOneShot(fall);
        blood.SetActive(true);
        blood.transform.position = new Vector3(player.transform.position.x, 0.001f, player.transform.position.z + 0.10f);
    }


     public IEnumerator PlayerWin()
     {
        game_over = true;
        yield return new WaitForSeconds(1f);

        running = false;
        player_animator.SetTrigger("idle");

        ui_win.SetActive(true);
     }

}
