using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameController_Mode1 : MonoBehaviour,GameController
{
    public enum MoveDirection { Up, Down, Left, Right };
    private PlayerSetting ps;
    private GamePageUIManager GPUIM;
    private SoundsController SC;
    private Map map;

    private bool isPause;
    public bool isMute;

    private string archivePath;
    private string bestScorePath;

    void Start()
    {
        archivePath = Application.persistentDataPath + "/gamesave_mode1.save";
        bestScorePath = Application.persistentDataPath + "/bestscore_mode1.save";
        GPUIM = GameObject.Find("EventSystem").GetComponent<GamePageUIManager>();
        SC = gameObject.GetComponent<SoundsController>();
        ps = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        map = GameObject.Find("Map").GetComponent<Map>();
        GameStart();
    }

    public void GameStart()
    {
        isPause = false;
        if (map.blockCounter != 0)
        {
            map.DestroyMap();
        }
        if (ps.IsLoad && File.Exists(archivePath))
            LoadGame();
        else
        {
            TimeCounter.timeSpend = 0f;
            map.InitMap(4);
            map.addNewNumber();
            map.addNewNumber();
            map.Score = 0;
        }
        GPUIM.UpdateScore(map.Score);
        LoadBestScore();

        if(!SC.isBGMClipSetted())
            SC.setBGMClip("sounds/BGM");
        SC.PlayBGM();
        if (ps.Mute) SC.IsMute = true;

        float num = UnityEngine.Random.Range(0f, 1f);
        if (num > 0.5f)
            SC.PlayVoice("sounds/VoiceBag2/GameStart");
        else
            SC.PlayVoice("sounds/VoiceBag2/ÓÎÏ·¿ªÊ¼");
    }
    public void GameOver()
    {
        isPause = true;
        if (map.Score > map.BestScore)
        {
            SaveBestScore(); 
            GPUIM.OnGG(true);
        }
        else 
            GPUIM.OnGG(false);
    }
    public void ExitGame()
    {
        if (map.isLose())
        {
            SaveBestScore();
            File.Delete(archivePath);
        }
        else
            SaveGame();
    }
    public void GamePause(bool isPause)
    {
        this.isPause = isPause;
        if (isPause)
            GPUIM.OnPause();
        else
            GPUIM.OnContinue();
    }
    public bool IsPause() => isPause;
    public void SaveGame()
    {
        GameArchive_Mode1 ga = new GameArchive_Mode1();
        ga.score = map.Score;
        ga.usingtime = GameObject.Find("Time").GetComponent<TimeCounter>().usingTime.ToString();
        ga.blocks = map.saveMap();
        ga.mute = isMute;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(archivePath);
        bf.Serialize(file, ga);
        file.Close();
    }
    public void LoadGame()
    {
        if (!File.Exists(archivePath)) return;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(archivePath, FileMode.Open);
        GameArchive_Mode1 ga = (GameArchive_Mode1)bf.Deserialize(file);
        file.Close();

        GameObject.Find("Time").GetComponent<TimeCounter>().setTime(ga.usingtime);
        map.Score = ga.score;
        map.loadMap(ga.blocks);
        isMute = ga.mute;
    }
    public void SaveBestScore()
    {
        if (map.Score <= map.BestScore)
            return;
        map.BestScore = map.Score;
        if (!File.Exists(bestScorePath))
        {
            using (File.Create(bestScorePath)) ;
        }
        File.WriteAllText(bestScorePath, map.BestScore.ToString());
    }
    public void LoadBestScore()
    {
        if (!File.Exists(bestScorePath))
        {
            using (File.Create(bestScorePath));
            File.WriteAllText(bestScorePath, "0");
        }
        string str = File.ReadAllText(bestScorePath);
        map.BestScore=Convert.ToInt32(str);
        GPUIM.UpdateBestScore(map.BestScore);
    }

    public void Move_EventHandle(MoveDirection direction)
    {
        if (map.isNowMoving()) return;
        EntityControllerMode1.resetAllEntity();
        bool canMove = false;
        switch (direction)
        {
            case MoveDirection.Up:
                canMove = map.moveUp();
                break;
            case MoveDirection.Down:
                canMove = map.moveDown();
                break;
            case MoveDirection.Left:
                canMove = map.moveLeft();
                break;
            case MoveDirection.Right:
                canMove = map.moveRight();
                break;
        }
        if (!canMove)
        {
            map.moveMap(direction);
            if (map.isLose())
            {
                isPause = true;
                GameOver();
                return;
            }
        }
        else
        {
            map.addNewNumber();
            GPUIM.UpdateScore(map.Score);
        }
    }
}
