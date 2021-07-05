using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageUIManager:MonoBehaviour
{
    Transform nowPanel;//��ǰ���

    Transform homePanel;//��ҳ���
    Transform aboutPanel;//�������
    Transform modeChosePanel;//ģʽѡ�����
    PlayerSetting ps;//�û�����

    //UI��������ʼ��
    void Awake()
    {
        homePanel= transform.Find("HomePanel");
        aboutPanel = transform.Find("AboutPanel");
        modeChosePanel = transform.Find("ModeChosePanel");
        ps = GameObject.Find("PlayerSetting").GetComponent<PlayerSetting>();
        GameObject.DontDestroyOnLoad(GameObject.Find("PlayerSetting"));

        homePanel.gameObject.SetActive(true);
        nowPanel = homePanel;
      
        aboutPanel.gameObject.SetActive(false);
        modeChosePanel.gameObject.SetActive(false);
        ps.IsLoad = false;
    }

    public void OnStartGame()
    {
        nowPanel.gameObject.SetActive(false);
        modeChosePanel.gameObject.SetActive(true);
        nowPanel = modeChosePanel;
    }
    public void OnStartGame1()
    {
        SceneManager.LoadScene("GameScene1");
    }
    public void OnStartGame2()
    {
        SceneManager.LoadScene("GameScene2");
    }
    public void OnLoadGame()
    {
        nowPanel.gameObject.SetActive(false);
        modeChosePanel.gameObject.SetActive(true);
        nowPanel = modeChosePanel;
        ps.IsLoad = true;
    }
    public void OnShowAbout()
    {
        nowPanel.gameObject.SetActive(false);
        aboutPanel.gameObject.SetActive(true);
        nowPanel = aboutPanel;
    }
    public void OnExitGame()
    {
        Application.Quit();
    }
    public void OnBack()
    {
        nowPanel.gameObject.SetActive(false);
        homePanel.gameObject.SetActive(true);
        nowPanel = homePanel;
    }
}
