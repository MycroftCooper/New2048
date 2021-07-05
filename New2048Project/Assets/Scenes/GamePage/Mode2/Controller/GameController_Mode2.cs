using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static GameArchive_Mode2;

public class GameController_Mode2 : MonoBehaviour, GameController
{
    private PlayerSetting ps;
    private GamePageUIManager GPUIM;
    private SoundsController SC;
    private SubmarineController submarine;
    private GameObject waterPool;

    public bool isPause;
    public bool isMute;

    private string archivePath;
    private string bestScorePath;

    public int Score { get; set; }
    public int BestScore { get; set; }
    private bool isLose;
    public bool IsLose
    {
        get => isLose;
        set
        {
            if (value)
                GameOver();
            isLose = value;
        }
    }
    public bool isShaking;

    void Start()
    {
        archivePath = Application.persistentDataPath + "/gamesave_mode2.save";
        bestScorePath = Application.persistentDataPath + "/bestscore_mode2.save";
        GPUIM = GameObject.Find("EventSystem").GetComponent<GamePageUIManager>();
        SC = gameObject.GetComponent<SoundsController>();
        ps = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        submarine = GameObject.Find("Submarine").GetComponent<SubmarineController>();
        waterPool = GameObject.Find("WaterPool");
        GameStart();
        newEntityList = new Dictionary<Vector3, int>();
        keys = new List<Vector3>();
    }

    public void GameStart()
    {
        isPause = false;
        isLose = false;
        Score = 0;
        destroyAllEntity();

        if (ps.IsLoad && File.Exists(archivePath))
            LoadGame();

        GPUIM.UpdateScore(Score);
        LoadBestScore();

        if (!SC.isBGMClipSetted())
            SC.setBGMClip("sounds/BGM");
        SC.PlayBGM();
        if (ps.Mute) SC.IsMute = true;

        float num = UnityEngine.Random.Range(0f, 1f);
        if (num > 0.5f)
            SC.PlayVoice("sounds/VoiceBag2/GameStart");
        else
            SC.PlayVoice("sounds/VoiceBag2/游戏开始");

        originalPosition = waterPool.transform.position;
        isShaking = false;
    }
    public void GameOver()
    {
        isPause = true;
        if (Score > BestScore)
        {
            SaveBestScore();
            GPUIM.OnGG(true);
        }
        else
            GPUIM.OnGG(false);
    }
    public void ExitGame()
    {
        if (isLose)
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
        GameArchive_Mode2 ga = new GameArchive_Mode2();

        List<GameObject> entityList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));
        ga.entityList = new Dictionary<PositionVector3, int>();
        foreach(GameObject go in entityList)
        {
            PositionVector3 position = new PositionVector3();
            position.x = go.transform.position.x;
            position.y = go.transform.position.y;
            position.z = go.transform.position.z;
            int num = go.GetComponent<EntityControllerMode2>().Num;
            ga.entityList.Add( position,num);
        }

        ga.score = Score;
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
        GameArchive_Mode2 ga = (GameArchive_Mode2)bf.Deserialize(file);
        file.Close();
        EntityControllerMode2.createNewEntitys(ga.entityList);
        Score = ga.score;
        isMute = ga.mute;
    }
    public void SaveBestScore()
    {
        if (Score <=BestScore)
            return;
        BestScore = Score;
        if (!File.Exists(bestScorePath))
        {
            using (File.Create(bestScorePath)) ;
        }
        File.WriteAllText(bestScorePath, BestScore.ToString());
    }
    public void LoadBestScore()
    {
        if (!File.Exists(bestScorePath))
        {
            using (File.Create(bestScorePath)) ;
            File.WriteAllText(bestScorePath, "0");
        }
        string str = File.ReadAllText(bestScorePath);
        BestScore = Convert.ToInt32(str);
        GPUIM.UpdateBestScore(BestScore);
    }

    public void moveSubmarine(Vector3 targetPosition) => submarine.Move_EventHandle(targetPosition);
    
    static Vector3 originalPosition;
    static Vector3 upPosition;
    public void shakePool()
    {
        isShaking = true;
        upPosition = originalPosition + new Vector3(0f,0.5f,0f);
        StartCoroutine(movePool(upPosition));
    }
    private IEnumerator movePool(Vector3 targetPosition)
    {
        yield return StartCoroutine(TransfomTools.moveToPosition(waterPool.transform, targetPosition, 3));
        yield return StartCoroutine(TransfomTools.moveToPosition(waterPool.transform, originalPosition, 3));
        isShaking = false;
    }

    //生成新数字实体
    public bool hasNewEntity()
    {
        if (submarine.transform.childCount == 0) return false;
        return true;
    }
    public void createNewEntity()
    {
        EntityControllerMode2.createNewEntity(submarine.transform.position+ new Vector3(0,-1f,0));
    }
    public void dropTheEntity()
    {
        submarine.dropEntity();
    }

    //销毁所有数字实体
    private void destroyAllEntity()
    {
        List<GameObject> gos = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));
        if (gos.Count == 0) return;
        else
        {
            foreach(GameObject i in gos)
            {
                GameObject.Destroy(i);
            }
        }
    }

    //相同数字实体碰撞响应
    List<Vector3> keys;
    Dictionary<Vector3, int> newEntityList;
    public void entityCollisionHandle(Vector3 newPosition, int num)
    {
        if (newEntityList.ContainsKey(newPosition))
            return;
        newEntityList.Add(newPosition, num);
        keys.Add(newPosition);
        Score += num / 2;
        GPUIM.UpdateScore(Score);
    }
    private void createNewEntityInList()
    {
        for(int i=0;i<keys.Count;i++)
        {
            EntityControllerMode2.createNewEntity(keys[i],newEntityList[keys[i]]);
            newEntityList.Remove(keys[i]);
            keys.RemoveAt(i);
        }
    }

    void Update()
    {
        createNewEntityInList();
    }
}
