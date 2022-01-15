using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject score;
    [SerializeField] GameObject highScoreText;
    [SerializeField] GameObject highScore;
    [SerializeField] GameObject backGround;
    [SerializeField] GameObject Name;
    [SerializeField] GameObject sound;
    [SerializeField] GameObject replay;
    [SerializeField] GameObject play;
    GameObject player;
    private float coinNow, scoreNow, highScoreNow, soundOn;
    public static UIController ui;

    void Awake()
    {
        if (ui == null)
            ui = this;
    }
    private void Start()
    {
        coinNow = PlayerPrefs.GetFloat("coin", 0);
        highScoreNow = PlayerPrefs.GetFloat("highscore", 0);
        soundOn = PlayerPrefs.GetFloat("sound", 1);
        coin.GetComponent<Text>().text = coinNow.ToString();
        AudioListener.volume = soundOn;
        backGround.SetActive(true);
        Name.SetActive(true);
        StartCoroutine(StartScreen());
    }
    IEnumerator StartScreen()
    {
        yield return new WaitForSeconds(1);
        score.SetActive(false);
        highScore.SetActive(false);
        highScoreText.SetActive(false);
        backGround.SetActive(false);
        Name.SetActive(false);
        sound.SetActive(false);
        play.SetActive(true);
        replay.SetActive(false);
    }
    public void OnPlay()
    {
        score.SetActive(true);
        play.SetActive(false);
        player.GetComponent<Swiping>().alive = true;
        CameraController.camera.alive = true;
    }
    public void OnReplay()
    {
        StartCoroutine(Replay());
    }
    public IEnumerator Replay()
    {
        PlayerPrefs.SetFloat("sound", soundOn);
        backGround.SetActive(true);
        Name.SetActive(true);
        SceneManager.LoadScene("Main");
        yield return new WaitForSeconds(2);

    }
    public void OnSound()
    {
        AudioListener.volume = -AudioListener.volume + 1;
        soundOn = AudioListener.volume;
    }
    public void TakeCoin()
    {
        coinNow++;
        coin.GetComponent<Text>().text = coinNow.ToString();
    }
    public void MoveScore()
    {
        scoreNow++;
        score.GetComponent<Text>().text = scoreNow.ToString();
    }
    public void Death()
    {
        if (highScoreNow < scoreNow)
            highScoreNow = scoreNow;
        highScore.GetComponent<Text>().text = highScoreNow.ToString();
        score.SetActive(true);
        highScore.SetActive(true);
        highScoreText.SetActive(true);
        backGround.SetActive(false);
        Name.SetActive(false);
        sound.SetActive(true);
        play.SetActive(false);
        replay.SetActive(true);
        PlayerPrefs.SetFloat("coin", coinNow);
        PlayerPrefs.SetFloat("highscore", highScoreNow);
    }
    public void SetPlayer(ref GameObject playerOrig)
    {
        player = playerOrig;
    }
}
