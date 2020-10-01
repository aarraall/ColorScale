using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using LPWAsset;
using TMPro;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerControl : Singleton<PlayerControl>
{
    [Header("Objects")] 
    public GameObject bodyOne;
    public GameObject bodyTwo;
    public GameObject ghost;
    public Animator playerAnim;
    public ParticleSystem confeti;
    private Vector3 inputStart, currentPos;
    private Vector3 deltaPos;
    public CinemachineVirtualCamera cm;
    public Material[] colors;
    public TextMeshProUGUI plusText;
    private SkinnedMeshRenderer[] meshes;
    public Material[] ghostMaterials;
    public LowPolyWaterScript water;
    
    [Header("Variables")] [Space] 
    public bool gameStarted = false;
    public bool redC, greenC, yellowC;
    private float playerSpeed = 13f;
    public int perfectCounter;
    public int terribleCounter;
    private Sequence tweenSequence;

    // START SET
    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        tweenSequence = DOTween.Sequence();
        meshes = ghost.GetComponentsInChildren<SkinnedMeshRenderer>();
        water = FindObjectOfType<LowPolyWaterScript>();

        Color color = new Color32(0, 111, 255, 255);
        water.material.DOColor(color, 1f);

    }

    #region CONTROLLER

    private void FixedUpdate()
    {
        Run();
    }

    // RUN CONTROL - SLIDE
    private void Run()
    {
        if (gameStarted == true)
        {
            transform.position += Vector3.forward * playerSpeed * Time.deltaTime;
            playerAnim.Play("Run");
            ghost.GetComponent<Animator>().Play("Run");

            if (Input.GetMouseButtonDown(0))
            {
                inputStart = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                currentPos = Input.mousePosition;
                deltaPos = currentPos - inputStart;
                float x = Mathf.Clamp(transform.position.x + deltaPos.x, -3, 3);
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(x, transform.position.y, transform.position.z), 5f * Time.deltaTime);
            }
        }
    }

    #endregion


    #region COLLISION

    // EAT COOKIE
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cookie"))
        {
            Destroy(other.gameObject);

            // COOKIE COLOR MATCH
            if (redC && other.gameObject.GetComponent<Cookies>().red)
            {
                perfectCounter++;
                transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f,
                    transform.localScale.z + 0.2f);
                GameManager.Instance.sound[0].Play();
                
                //plustext instantiation
                plusText.gameObject.SetActive(true);
                plusText.rectTransform.DOScale(2, .3f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
                {
                    plusText.rectTransform.DOScale(1, .3f);
                    plusText.gameObject.SetActive(false);
                });
                plusText.rectTransform.DOMoveY(20, .3f).SetRelative();
                
                //ghost color instantiation
                ghost.SetActive(true);
                ghost.transform.DOScale(.05f, .3f).SetRelative().OnComplete(() =>
                    {
                        ghost.transform.DOScale(-.05f, .3f).SetRelative();
                        ghost.SetActive(false);
                    });

                
            }
            else if (greenC && other.gameObject.GetComponent<Cookies>().green)
            {
                perfectCounter++;
                transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f,
                    transform.localScale.z + 0.2f);
                GameManager.Instance.sound[0].Play();

                plusText.gameObject.SetActive(true);
                plusText.rectTransform.DOScale(2, .3f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
                {
                    plusText.rectTransform.DOScale(1, .3f);
                    plusText.gameObject.SetActive(false);
                });
                plusText.rectTransform.DOMoveY(20, .3f).SetRelative();

                ghost.SetActive(true);
                ghost.transform.DOScale(.05f, .3f).SetRelative().OnComplete(() =>
                    {
                        ghost.transform.DOScale(-.05f, .3f).SetRelative();
                        ghost.SetActive(false);
                    });
                
                
            }
            else if (yellowC && other.gameObject.GetComponent<Cookies>().yellow)
            {
                perfectCounter++;
                transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f,
                    transform.localScale.z + 0.2f);
                GameManager.Instance.sound[0].Play();

                plusText.gameObject.SetActive(true);
                plusText.rectTransform.DOScale(2, .3f).SetLoops(1, LoopType.Yoyo).OnComplete(() =>
                {
                    plusText.rectTransform.DOScale(1, .3f);
                    plusText.gameObject.SetActive(false);
                });
                plusText.rectTransform.DOMoveY(20, .3f).SetRelative();

                ghost.SetActive(true);
                ghost.transform.DOScale(.05f, .3f).SetRelative().OnComplete(() =>
                    {
                        ghost.transform.DOScale(-.05f, .3f).SetRelative();
                        ghost.SetActive(false);
                    });
                
            }
            else
            {
                terribleCounter++;
                perfectCounter = 0;
                transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f,
                    transform.localScale.z - 0.1f);
                GameManager.Instance.sound[1].Play();
                plusText.rectTransform.DOMoveY(-20, .3f).SetRelative();
                perfectCounter = 0;
                ghost.SetActive(true);
                meshes[0].material = ghostMaterials[1];
                meshes[1].material = ghostMaterials[1];
                ghost.transform.DOScale(.05f, .3f).SetRelative().OnComplete(() =>
                {
                    ghost.transform.DOScale(-.05f, .3f).SetRelative();
                    meshes[0].material = ghostMaterials[0];
                    meshes[1].material = ghostMaterials[0];
                    ghost.SetActive(false);
                });
            }

            if (perfectCounter < 4 && perfectCounter != 0)
            {
                if (perfectCounter % 2 == 0)
                {
                    GameManager.Instance.Amazer();
                }
                else if (perfectCounter % 3 == 0)
                {
                    GameManager.Instance.Perfector();
                }
            }
            else
            {
                perfectCounter = 1;
            }

            if (terribleCounter < 4 && terribleCounter != 0 && perfectCounter < 2)
            {
                if(terribleCounter %3 == 0)
                    GameManager.Instance.Terribler();
            }
            else
            {
                terribleCounter = 0;
            }
        }
    }

    // COLLISION
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RedGate"))
        {
            bodyOne.GetComponent<SkinnedMeshRenderer>().material = colors[0];
            bodyTwo.GetComponent<SkinnedMeshRenderer>().material = colors[0];
            water.material.DOColor(Color.red, .3f);

            redC = true;
            yellowC = false;
            greenC = false;
        }

        if (other.gameObject.CompareTag("GreenGate"))
        {
            bodyOne.GetComponent<SkinnedMeshRenderer>().material = colors[1];
            bodyTwo.GetComponent<SkinnedMeshRenderer>().material = colors[1];
            water.material.DOColor(Color.green, .3f);

            redC = false;
            yellowC = false;
            greenC = true;
        }

        if (other.gameObject.CompareTag("YellowGate"))
        {
            bodyOne.GetComponent<SkinnedMeshRenderer>().material = colors[2];
            bodyTwo.GetComponent<SkinnedMeshRenderer>().material = colors[2];
            water.material.DOColor(Color.yellow, .3f);

            redC = false;
            yellowC = true;
            greenC = false;
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            GameManager.Instance.FinishLevel();
            
            Color color = new Color32(0, 111, 255, 255);
            water.material.DOColor(color, 1f);
        }

        if (other.gameObject.CompareTag("Respawn"))
        {
            
            cm.m_Priority = 11;
        }
    }

    #endregion

}