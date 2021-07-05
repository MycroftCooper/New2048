using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePageUIManager : MonoBehaviour
{
    public Canvas pauseCanvas;
    public TextMesh scoreText;
    public Canvas GGCanvas;
    private GameController GC;
    private SoundsController SC;

    private void Awake()
    {
        pauseCanvas.gameObject.SetActive(false);
        GGCanvas.gameObject.SetActive(false);
        GC = GameObject.Find("GameController").GetComponent<GameController>();
        SC= GameObject.Find("GameController").GetComponent<SoundsController>();
    }
    public void OnPause()
    {
        pauseCanvas.gameObject.SetActive(true);
        if (!GC.IsPause()) GC.GamePause(true);
    }
    public void OnBack()
    {
        GC.ExitGame();
        SceneManager.LoadScene("HomePageScene");
    }
    public void OnRestart()
    {
        GC.ExitGame();
        pauseCanvas.gameObject.SetActive(false);
        GGCanvas.gameObject.SetActive(false);
        GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>().IsLoad = false;
        GC.GameStart();
    }
    public void OnContinue()
    {
        pauseCanvas.gameObject.SetActive(false);
        if (GC.IsPause()) GC.GamePause(false);
    }
    public void OnExitGame()
    {
        GC.ExitGame();
        Application.Quit();
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
    public void UpdateBestScore(int bestScore)
    {
        GameObject.Find("BestScore").GetComponent<Text>().text = bestScore.ToString();
    }
    public void OnGG(bool isNewScore)
    {
        Transform GGP = GGCanvas.transform.GetChild(0).GetChild(0);
        GGP.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        GGCanvas.gameObject.SetActive(true);
        StartCoroutine(TransfomTools.Scale(GGP, new Vector3(1f, 1f, 1f), 1f));
        Text EndScore = GameObject.Find("EndScore").GetComponent<Text>();
        EndScore.text = scoreText.text;
        if (isNewScore)
        {
            SC.PlayVoice("sounds/VoiceBag2/新纪录");
            GameObject.Find("NewScore").SetActive(true);
        }
        else
        {
            GameObject.Find("NewScore").SetActive(false);
            float num = Random.Range(0f, 1f);
            if (num > 0.5f)
                SC.PlayVoice("sounds/VoiceBag2/GameOver");
            else
                SC.PlayVoice("sounds/VoiceBag2/失败了");
        }
        
    }
    public void OnSoundsButton()
    {
        SC.IsMute = !SC.IsMute;
        Image bgmImg = GameObject.Find("BGMButton").GetComponent<Image>();
        if (SC.IsMute)
            bgmImg.sprite = Resources.Load<Sprite>("imgs/UI/GamePage/音量关闭");
        else
            bgmImg.sprite = Resources.Load<Sprite>("imgs/UI/GamePage/音量开启"); 
    }
}
