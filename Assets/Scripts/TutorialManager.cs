using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject TutorialTextGroup;
    [SerializeField] 
    private GameObject coin;
    [SerializeField] 
    private GameObject rock;
    [SerializeField] 
    private TextMeshProUGUI TutorialText;
    private int batPosition;
    private int tutorialCount = 0;
    private int hasPowerUp = 0;
    private int lifeCount = 0;

    public AudioClip[] tutorialSounds;
    private bool rockHittedTheSecondTime;
    private bool mustEnd;
    private float previousRockSpeed;
    public static TutorialManager Instance;



    private void Awake()
    {
        Instance = this;
        PlayerPrefs.SetFloat("wolfSpeed", 1.75f);
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = false;
        batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
    }

    public void Start() {
        SpawnRocks();
        PlayerPrefs.SetInt("isOnTutorial",1);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (tutorialCount == 0)
            {
                other.gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Tutorial");
            }
            else if (tutorialCount == 1)
            {
                rockHittedTheSecondTime = true;
                RockTutorial2();
                EnteredTutorialState();
                other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        //     else
        //     {
        //         EnteredTutorialState();
        //         RocksTutorial();
        //         GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Enemy");
        //         foreach (GameObject obj in objectsToDestroy)
        //         {
        //             Destroy(obj.GetComponent<Rigidbody2D>());
        //             Destroy(obj.GetComponent<LoboBehavior>());
        //             Destroy(obj.GetComponent<BoxCollider2D>());
        //         }
        //     }
        }

        // if (other.gameObject.tag == "Treasure")
        // {
        //     EnteredTutorialState();
        //     CoinTutorial();
        //     other.gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Tutorial");
        // }
    }

    private void UpdateText(string text, bool mustReset = true) 
    {
        TutorialText.text = text;
        if (mustReset)
            TutorialText.gameObject.GetComponent<TMPEffect>().ResetEffect();
    }

    private void RockTutorial1()
    {
        UpdateText("Voz Misteriosa: Você foi atingido por uma pedra e a caverna está desmoronando. É hora de voar para longe!",false);
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audios)
        {
            if (!(audio.gameObject.name == "Audio Description" || audio.gameObject.name == "SoundManager"))
            {
                audio.Play();
            }
        }

        GameObject.Find("Morcego").GetComponent<AudioSource>().clip = tutorialSounds[0];
        GameObject.Find("Morcego").GetComponent<AudioSource>().Play();

        Invoke("ExitedTutorialState", 5.6f);
        Invoke("SpawnRock", 2f);
    }

    private void RockTutorial2()
    {
        UpdateText("Voz Misteriosa: Uma pedra está vindo pelo centro, em sua direção. Rápido! Deslize o dedo para a DIREITA para escapar");
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
        GameObject.Find("Morcego").GetComponent<AudioSource>().clip = tutorialSounds[1];
        GameObject.Find("Morcego").GetComponent<AudioSource>().Play();

    }

    private void CoinTutorial()
    {
        UpdateText("Liro: Que barulho é esse? Parece que tem algo valioso por alí, me ajude a alcançar!");
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
        GameObject.Find("Morcego").GetComponent<AudioSource>().clip = tutorialSounds[2];
        GameObject.Find("Morcego").GetComponent<AudioSource>().Play();
    }

    private void RocksTutorial()
    {
        UpdateText("Liro: Minha nossa, não tenho para onde ir, tenho que usar os meus poderes, clique duas vezes na tela para me ajudar!");
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
        GameObject.Find("Morcego").GetComponent<AudioSource>().clip = tutorialSounds[3];
        GameObject.Find("Morcego").GetComponent<AudioSource>().Play();
    }


    private void EnteredTutorialState(bool mustPause = true)
    {
        TutorialTextGroup.SetActive(true);
        tutorialCount++;

        if (!mustPause)
            return;

        ChaoManager.Instance.isPaused = true;
        LoboBehavior[] enemies = FindObjectsOfType<LoboBehavior>();
        if (enemies.Length > 0) {
            previousRockSpeed = enemies[0].wolfSpeed;
            foreach (LoboBehavior enemy in enemies) {
                enemy.wolfSpeed = 0;
            }
        }
        
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        

        foreach (AudioSource audio in audios)
        {
            if (!(audio.gameObject.name == "SoundManager" || audio.gameObject.name == "Morcego"))
            {
                audio.Pause();
            }
        }
    }

    private void ExitedTutorialState()
    {
        TutorialTextGroup.SetActive(false);
        PlayerPrefs.SetInt("isOnTutorial",0);

        ChaoManager.Instance.isPaused = false;
        LoboBehavior[] enemies = FindObjectsOfType<LoboBehavior>();
        if (enemies.Length > 0 && previousRockSpeed != 0) {
            foreach (LoboBehavior enemy in enemies) {
                enemy.wolfSpeed = previousRockSpeed;
            }
        }
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in audios)
        {
            if (!(audio.gameObject.name == "Audio Description" || audio.gameObject.name == "SoundManager" || audio.gameObject.name == "Morcego"))
            {
                if (audio.isPlaying == false)
                    audio.Play();
            }
        }
    }

    private void TutorialEnded()
    {
        ExitedTutorialState();
        GameObject.Find("LevelManager").GetComponent<EnemyGenerator>().isInTutorial = false;
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
        Destroy(gameObject);
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("wolfSpeed", 1.75f);

        // Used Booster
        if (hasPowerUp != PlayerPrefs.GetInt("hasBooster"))
        {
            hasPowerUp = PlayerPrefs.GetInt("hasBooster");
            if (hasPowerUp == 0)
            {
                UpdateText("Liro: Parabéns! Vamos continuar essa aventura!");
                tutorialCount++;
                batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
                GameObject.Find("Morcego").GetComponent<AudioSource>().clip = tutorialSounds[4];
                GameObject.Find("Morcego").GetComponent<AudioSource>().Play();
            }
        }

        // Took damage
        if (lifeCount != LifeManager.Instance.GetLifeCount())
        {
            if (lifeCount >= LifeManager.Instance.GetLifeCount())
            {
                EnteredTutorialState(false);
                RockTutorial1();
                GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = false;
            }
            lifeCount = LifeManager.Instance.GetLifeCount();
        }

        // Moved
        if ((tutorialCount <= 3 || tutorialCount > 6) && rockHittedTheSecondTime)
        {
            if (batPosition != GameObject.Find("Morcego").GetComponent<SwipeBat>().batState)
            {
                batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
                GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = false;
                ExitedTutorialState();


                switch (tutorialCount)
                {
                    // case 2:
                    //     Invoke("SpawnCoin", 2f);
                    //     break;
                    // case 3:
                    //     PlayerPrefs.SetInt("hasBooster", 1);
                    //     Invoke("SpawnRocks", 2f);
                    //     break;
                    default:
                        mustEnd = true;
                        break;
                }

            }
        }

        if (mustEnd && GameObject.FindAnyObjectByType<LoboBehavior>() == null)
            TutorialEnded();
    }

    private void SpawnCoin()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        Vector3 postion = new Vector3(0, worldHeight * 1.01f);

        Instantiate(coin, postion, transform.rotation);
    }

    private void SpawnRock()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        Vector3 postion = new Vector3(0, worldHeight * 1.01f);

        Instantiate(rock, postion, transform.rotation);
    }

    private void SpawnRocks()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        Vector3 postion = new Vector3((worldWidth / 6) * -2, worldHeight * 1.01f);
        Instantiate(rock, postion, transform.rotation);
        postion = new Vector3((worldWidth / 6) * 2, worldHeight * 1.01f);
        Instantiate(rock, postion, transform.rotation);
        postion = new Vector3(0, worldHeight * 1.01f);
        Instantiate(rock, postion, transform.rotation);
    }
}
