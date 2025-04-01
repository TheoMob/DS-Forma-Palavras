using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintManager : MonoBehaviour
{
    [Header("Variaveis de controle")]
    [SerializeField] private float[] _tempoParaDicas = new float[3];

    [Header("Sons")]
    [SerializeField] private AudioClip[] _hintSounds;

    [Header("Debug variables Don't Change")]
    [SerializeField] private int _currentHintIndex = 0;
    public bool HintTimerRunning = false;
    [SerializeField] private float _hintTimer = 0f;

    public List<LetterBlock> LetterBlocks = new List<LetterBlock>();
    public List<LetterContainer> LetterContainers = new List<LetterContainer>();

    public static HintManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);   
        }
        else
            Destroy(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (GameController.instance.GamePaused)
            return;
            
        if (HintTimerRunning && _currentHintIndex < 3) // se for 3 é pq significa que todas as dicas ja foram tocadas
        {
            _hintTimer -= Time.deltaTime;

            if (_hintTimer <= 0) // caso o tempo de uma dica tenha chegado
            {
                switch(_currentHintIndex) // depois trocar para que de fato tenha as 3 dicas
                {
                    case 0:
                        Hint1();
                    break;
                    case 1:
                        Hint2();
                    break;
                    case 2:
                        Hint3();
                    break;
                    default:
                        Hint2();
                    break;
                }

                _currentHintIndex++;
                if (_currentHintIndex >= _tempoParaDicas.Length)
                    _currentHintIndex = _tempoParaDicas.Length - 1;

                _hintTimer = _tempoParaDicas[_currentHintIndex];

                GameSessionLogger.instance.HintsUsed++; // registra quando uma hint é usada
            }
        }
    }

    public void GetLetterBlocksAndContainers()
    {
        LetterBlocks = new List<LetterBlock>();
        LetterContainers = new List<LetterContainer>();

        foreach(Transform child in GameObject.FindWithTag("LetterBlockMaster").transform)
        {
            if (child.CompareTag("LetterBlock"))
                LetterBlocks.Add(child.GetComponent<LetterBlock>());
        }

        foreach(Transform child in GameObject.FindWithTag("LetterContainerMaster").transform)
        {
            if (child.CompareTag("LetterContainer"))
                LetterContainers.Add(child.GetComponent<LetterContainer>());
        }
    }

    public void ResetHintTimer()
    {
        _hintTimer = _tempoParaDicas[_currentHintIndex];
    }

    public void ResetHintIndex()
    {
        _currentHintIndex = 0;
    }

    public void Hint1()
    {
        if (LetterContainers.Count == 0 || LetterBlocks.Count == 0)
            GetLetterBlocksAndContainers();

        for (int i = 0; i <= GameController.instance.WordsCount; i++)
        {   
            foreach(LetterContainer lc in LetterContainers.Where(lc => lc.WordIndex == i))
            {
                if (lc.Matched == false)
                {
                    Debug.Log("A proxima letra não colocada é " + lc.Letter + " E ela fica na wordIndex " + lc.WordIndex);

                    LetterBlock lb = LetterBlocks.Find(lb => lb.Letter == lc.Letter && lb.Matched == false);
                    Animator anim = lb.GetComponent<Animator>();
                    anim.Play("Null");
                    anim.Play("Hint1");
                    return;
                }
            }
        }


        Debug.LogWarning("Nenhuma letra foi encontrada na Hint1");
    }

    public void Hint2()
    {
        if (LetterContainers.Count == 0 || LetterBlocks.Count == 0)
            GetLetterBlocksAndContainers();

        for (int i = 0; i <= GameController.instance.WordsCount; i++)
        {   
            foreach(LetterContainer lc in LetterContainers.Where(lc => lc.WordIndex == i))
            {
                if (lc.Matched == false)
                {
                    Debug.Log("A proxima letra não colocada é " + lc.Letter + " E ela fica na wordIndex " + lc.WordIndex);

                    LetterBlock lb = LetterBlocks.Find(lb => lb.Letter == lc.Letter && lb.Matched == false);
                    Animator anim = lb.GetComponent<Animator>();
                    anim.Play("Null");
                    anim.Play("Hint2");
                    SoundFXManager.instance.PlaySoundFXClip(_hintSounds, transform.position, 1f, false);
                    return;
                }
            }
        }


        Debug.LogWarning("Nenhuma letra foi encontrada na Hint2");
    }

    public void Hint3()
    {
        if (LetterContainers.Count == 0 || LetterBlocks.Count == 0)
            GetLetterBlocksAndContainers();

        for (int i = 0; i <= GameController.instance.WordsCount; i++)
        {   
            foreach(LetterContainer lc in LetterContainers.Where(lc => lc.WordIndex == i))
            {
                if (lc.Matched == false)
                {
                    Debug.Log("A proxima letra não colocada é " + lc.Letter + " E ela fica na wordIndex " + lc.WordIndex);

                    LetterBlock lb = LetterBlocks.Find(lb => lb.Letter == lc.Letter && lb.Matched == false);
                    Animator anim = lb.GetComponent<Animator>();
                    anim.Play("Null");
                    anim.Play("Hint2");

                    Animator containerAnim = lc.GetComponent<Animator>();
                    containerAnim.Play("Hint3"); // não tem null na frente pq eu n quero que refaça depois da primeira vez
                    SoundFXManager.instance.PlaySoundFXClip(_hintSounds, transform.position, 1f, false);
                    return;
                }
            }
        }


        Debug.LogWarning("Nenhuma letra foi encontrada na Hint3");
    }

    #region startingLevel

    public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (!scene.name.Contains("Level")) // se não tem level no nome da cena, não é um level, logo não precisa do hint manager
            return;
        

        _hintTimer = _tempoParaDicas[0];
        _currentHintIndex = 0;
        HintTimerRunning = true;

        Debug.Log("Level iniciado, sistema de dicas ligado");
    }
    #endregion
}
