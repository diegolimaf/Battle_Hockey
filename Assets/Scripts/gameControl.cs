using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class gameControl : MonoBehaviour
{
    private GameObject goalToronto, goalMontreal, disk, pauseMenu;

    int montrealScore = 0;
    int torontoScore = 0;

    bool torontoTurn, montrealTurn, useMontrealDefense, useTorontoDefense;

    public Text hitsText;

    public Text torontoPoint;
    public Text montrealPoint;
    public Text torontoPress;
    public Text montrealPress;
    public Text montrealWinner;
    public Text torontoWinner;
    public Image defenseToronto;
    public Image defenseMontreal;
    public RawImage torontoCelebration;
    public RawImage montrealCelebration;
    public GameObject VideoMontreal;
    public GameObject VideoToronto;
    public GameObject montrealDefense;
    public GameObject torontoDefense;

    SpriteRenderer montrealRenderer, torontoRenderer;

    private Animator montrealAnimator;
    private Animator torontoAnimator;

    music_manager music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<music_manager>();
        goalMontreal = GameObject.Find("GoalMontreal");
        goalToronto = GameObject.Find("GoalToronto");
        pauseMenu = GameObject.Find("Canvas");
        disk = GameObject.Find("disk");

        montrealRenderer = montrealDefense.GetComponent<SpriteRenderer>();
        torontoRenderer = torontoDefense.GetComponent<SpriteRenderer>();

        montrealAnimator = montrealCelebration.GetComponent<Animator>();
        torontoAnimator = torontoCelebration.GetComponent<Animator>();
        SetDiskMontreal();
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinner();
        StartPosition();
        SetDefense();
        HideDefense();
    }

    public void SetDiskMontreal()
    {
        disk = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/hockeyDisk", typeof(GameObject)));
        disk.transform.localPosition = new Vector3(17.6f, goalMontreal.transform.localPosition.y, -1);
        disk.GetComponent<disk>().hitsText = hitsText;
    }

    public void SetDiskToronto()
    {
        disk = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/hockeyDisk", typeof(GameObject)));
        disk.transform.localPosition = new Vector3(-17.6f, goalToronto.transform.localPosition.y, -1);
        disk.GetComponent<disk>().hitsText = hitsText;
    }
    public void MontrealPoint()
    {
        useTorontoDefense = false;
        useMontrealDefense = false;
        if (torontoRenderer.enabled)
            torontoRenderer.enabled = false;
        if (montrealRenderer.enabled)
            montrealRenderer.enabled = false;
        GameObject.Destroy(disk.gameObject);
        montrealScore++;
        montrealPoint.text = montrealScore.ToString();
        montrealPress.enabled = true;
        montrealTurn = true;
        torontoTurn = false;
        defenseToronto.enabled = true;
        defenseMontreal.enabled = true;
        montrealCelebration.enabled = true;
        VideoMontreal.GetComponent<VideoPlayer>().enabled = true;
        montrealAnimator.enabled = true;
        montrealAnimator.Play("showVideo",-1,0);
        music.PauseMusic();
    }
    public void TorontoPoint()
    {
        useTorontoDefense = false;
        useMontrealDefense = false;
        if (torontoRenderer.enabled)
            torontoRenderer.enabled = false;
        if (montrealRenderer.enabled)
            montrealRenderer.enabled = false;
        GameObject.Destroy(disk.gameObject);
        torontoScore++;
        torontoPoint.text = torontoScore.ToString();
        torontoPress.enabled = true;
        torontoTurn = true;
        montrealTurn = false;
        defenseToronto.enabled = true;
        defenseMontreal.enabled = true;
        torontoCelebration.enabled = true;
        VideoToronto.GetComponent<VideoPlayer>().enabled = true;
        torontoAnimator.enabled = true;
        torontoAnimator.Play("showVideo", -1, 0);
        music.PauseMusic();
    }

    void CheckWinner()
    {
        if (montrealScore == 3)
        {
            montrealWinner.enabled = true;
            montrealPress.enabled = false;
            torontoTurn = false;
            montrealTurn = false;
            montrealAnimator.enabled = true;
            pauseMenu.GetComponent<pauseMenu>().pauseMenuUI.SetActive(true);
            music.StopMusic();
        }
        if (torontoScore == 3)
        {
            torontoWinner.enabled = true;
            torontoPress.enabled = false;
            torontoTurn = false;
            montrealTurn = false;
            torontoAnimator.enabled = true;
            pauseMenu.GetComponent<pauseMenu>().pauseMenuUI.SetActive(true);
            music.StopMusic();
        }
    }

    void StartPosition()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && !disk && montrealTurn)
        {
            montrealPress.enabled = false;
            montrealAnimator.StopPlayback();
            montrealAnimator.enabled = false;
            montrealCelebration.enabled = false;
            VideoMontreal.GetComponent<VideoPlayer>().enabled = false;
            music.UnpauseMusic();
            SetDiskMontreal();
        }
        if (Input.GetKey(KeyCode.D) && !disk && torontoTurn)
        {
            torontoPress.enabled = false;
            VideoToronto.GetComponent<VideoPlayer>().enabled = false;
            torontoCelebration.GetComponent<Animator>().enabled = false;
            torontoCelebration.enabled = false;
            music.UnpauseMusic();
            SetDiskToronto();
        }
    }
    void SetDefense()
    {
        if (Input.GetKey(KeyCode.RightArrow) && disk && !useMontrealDefense)
        {
            montrealRenderer.enabled = true;
            useMontrealDefense = true;
            defenseMontreal.enabled = false;
        }
        if (Input.GetKey(KeyCode.A) && disk && !useTorontoDefense)
        {
            torontoRenderer.enabled = true;
            useTorontoDefense = true;
            defenseToronto.enabled = false;
        }
    }
    public void HideDefense()
    {
        if (montrealRenderer.enabled && useMontrealDefense)
        {
            StartCoroutine(waitTime(montrealRenderer, 5.0f));
        }
        if (torontoRenderer.enabled && useTorontoDefense)
        {
            StartCoroutine(waitTime(torontoRenderer, 5.0f));
        }
    }
    IEnumerator waitTime(SpriteRenderer defenseLine, float delay)
    {
        yield return new WaitForSeconds(delay);
        defenseLine.enabled = false;
    }
}
