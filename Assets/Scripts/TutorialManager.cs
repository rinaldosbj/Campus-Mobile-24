using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject TutorialTextGroup;
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject rock;
    [SerializeField] private TextMeshProUGUI TutorialText;
    private int batPosition;
    private int tutorialCount = 0;
    private int hasPowerUp = 0;


    private void Awake()
    {
        PlayerPrefs.SetFloat("wolfSpeed", 1.75f);
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = false;
        batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (tutorialCount == 0)
            {
                EnteredTutorialState();
                RockTutorial();
                other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                EnteredTutorialState();
                RocksTutorial();
                GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject obj in objectsToDestroy)
                {
                    Destroy(obj.GetComponent<Rigidbody2D>());
                    Destroy(obj.GetComponent<LoboBehavior>());
                    Destroy(obj.GetComponent<BoxCollider2D>());
                }
            }
        }

        if (other.gameObject.tag == "Treasure")
        {
            EnteredTutorialState();
            CoinTutorial();
            other.gameObject.GetComponent<BoxCollider2D>().excludeLayers = LayerMask.GetMask("Tutorial");
        }
    }

    private void RockTutorial()
    {
        TutorialText.text = "Liro: Minha nossa, tem uma pedra vindo na minha direção, arraste para os lados para me ajudar desviar!";
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
    }

    private void CoinTutorial()
    {
        TutorialText.text = "Liro: Que barulho é esse? Parece que tem algo valioso por alí, me ajude a alcançar!";
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
    }

    private void RocksTutorial()
    {
        TutorialText.text = "Liro: Minha nossa, não tenho para onde ir, tenho que usar os meus poderes, clique duas vezes na tela para me ajudar!";
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
    }


    private void EnteredTutorialState()
    {
        tutorialCount++;
        Time.timeScale = 0;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        TutorialTextGroup.SetActive(true);

        foreach (AudioSource audio in audios)
        {
            if (audio.gameObject.name != "SoundManager")
            {
                audio.Pause();
            }
        }
    }

    private void ExitedTutorialState()
    {
        Time.timeScale = 1;
        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        TutorialTextGroup.SetActive(false);

        foreach (AudioSource audio in audios)
        {
            if (!(audio.gameObject.name == "Audio Description" || audio.gameObject.name == "SoundManager"))
            {
                audio.Play();
            }
        }
    }

    private void TutorialEnded()
    {
        ExitedTutorialState();
        GameObject.Find("LevelManager").GetComponent<EnemyGenerator>().isInTutorial = false;
        Destroy(gameObject);
        GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = true;
    }

    private void Update()
    {
        PlayerPrefs.SetFloat("wolfSpeed", 1.75f);
        if (hasPowerUp != PlayerPrefs.GetInt("hasBooster"))
        {
            hasPowerUp = PlayerPrefs.GetInt("hasBooster");
            if (hasPowerUp == 0)
            {
                TutorialText.text = "Liro: Parabéns! Vamos continuar essa aventura!";
                tutorialCount++;
                batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
            }
        }

        if (tutorialCount <= 2 || tutorialCount > 5)
        {
            if (batPosition != GameObject.Find("Morcego").GetComponent<SwipeBat>().batState)
            {
                batPosition = GameObject.Find("Morcego").GetComponent<SwipeBat>().batState;
                GameObject.Find("Morcego").GetComponent<SwipeBat>().canSwipe = false;
                ExitedTutorialState();

                switch (tutorialCount)
                {
                    case 1:
                        Invoke("SpawnCoin", 2f);
                        break;
                    case 2:
                        PlayerPrefs.SetInt("hasBooster", 1);
                        Invoke("SpawnRocks", 2f);
                        break;
                    default:
                        TutorialEnded();
                        break;
                }
            }
        }
    }

    private void SpawnCoin()
    {
        float aspect = (float)Screen.width / Screen.height;
        float worldHeight = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize * 2;
        float worldWidth = worldHeight * aspect;
        Vector3 postion = new Vector3(0, worldHeight * 1.01f);

        Instantiate(coin, postion, transform.rotation);
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
