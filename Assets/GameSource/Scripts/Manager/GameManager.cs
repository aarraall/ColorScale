using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [Header("GameObjects")]
    public GameObject player;
    public GameObject enemyBoss;
    public AudioSource[] sound;
    public GameObject[] levelHolder;

    [Header("UI Panel")]    
    public GameObject finishPanel;
    public GameObject gameOverPanel;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text levelText;
    public Text coinText;

    private int level;
    private int coin;
    

    [Header(("Perfect Sprites"))] 
    public Image amazing;
    public Image perfect;
    public Image terrible;


    // ---------- START
    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        GetDatas();
        LevelGenerator();       
        coinText.text = coin.ToString();
        levelText.text = "LEVEL " + level.ToString();
        Debug.Log(level);
    }

    
    // ---------- GAME EVENTS
    public void FinishLevel()
    {

        PlayerControl.Instance.gameStarted = false;
        PlayerStop();
        PlayerControl.Instance.playerAnim.Play("Fight");
        enemyBoss.GetComponent<Animator>().Play("FightBoss");

        Invoke("FinishGame", 2f);      
    }

    IEnumerator FinishPanel()
    {
        yield return new WaitForSeconds(5f);
        finishPanel.SetActive(true);
        AddCoin(50);
        
    }

    IEnumerator OverPanel()
    {
        yield return new WaitForSeconds(5f);
        gameOverPanel.SetActive(true);
        sound[3].Play();
    }


    // ---------- OTHER
    public void PlayerStop()
    {

        player.GetComponent<PlayerControl>().enabled = false;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void LevelGenerator()
    {
        int i = level - 1;
        levelHolder[i].SetActive(true);
        enemyBoss = levelHolder[i].transform.GetChild(0).gameObject;
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }

    public void GetDatas()
    {
        // LEVEL
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        else
        {
            PlayerPrefs.SetInt("level", 1);
            level = 1;
        }

        // GEM
        if (PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        else
        {
            PlayerPrefs.SetInt("coin", coin);
        }

        // SOUND
        if (!PlayerPrefs.HasKey("sound"))
        {
            PlayerPrefs.SetInt("sound", 1);
        }
    }

    public void AddCoin(int newCoin)
    {
        int prevCoin = PlayerPrefs.GetInt("coin");
        PlayerPrefs.SetInt("coin", prevCoin + newCoin);
        coin = newCoin;
    }

    private void FinishGame()
    {
        if (player.transform.localScale.x > enemyBoss.transform.localScale.x)
        {
            StartCoroutine(FinishPanel());
           
            player.GetComponent<PlayerControl>().confeti.Play();
            PlayerControl.Instance.playerAnim.Play("Win");
            enemyBoss.GetComponent<Animator>().Play("Fall");
            
            level++;
            PlayerPrefs.SetInt("level", level);

        }
        else
        {
            StartCoroutine(OverPanel());
            PlayerControl.Instance.playerAnim.Play("Fall");
            enemyBoss.GetComponent<Animator>().Play("Win");
        }
    }

    // ---------- UI BUTTON
    public void StartButton()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        
        PlayerControl.Instance.gameStarted = true;
        
    }

    public void RestartButton()
    {
        SceneLoad();
    }

    //--Perfect System--

    public void Perfector()
    {
        perfect.gameObject.SetActive(true);
        perfect.transform.DOScale(5, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            perfect.transform.DOScale(1, 0);
            perfect.gameObject.SetActive(false);
        });
    }
    public void Amazer()
    {
        amazing.gameObject.SetActive(true);
        amazing.transform.DOScale(5, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            amazing.transform.DOScale(1, 0);
            amazing.gameObject.SetActive(false);
        });
    }
    public void Terribler()
    {
        terrible.gameObject.SetActive(true);
        terrible.transform.DOScale(5, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            terrible.transform.DOScale(1, 0);
            terrible.gameObject.SetActive(false);
        });
    }
}