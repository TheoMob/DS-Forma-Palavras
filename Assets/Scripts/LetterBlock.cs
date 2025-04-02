using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LetterBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rect;
    private Vector2 _initialPosition;
    private GameObject _letterContainer;
    private Animator _anim;

    [Header("Variaveis de Configuração")]
    public string Letter = "X";
    public bool Matched = false;
    private bool _wronglyMatched = false;

    [Header("Sons")]
    [SerializeField] private AudioClip[] _correctLetterSound;
    [SerializeField] private AudioClip[] _wrongLetterSound;

    [Header("Debug Variables - Don't alterate")]
    [SerializeField] private bool _isTouchingCorrectBlock = false;

    #region Debug and Editor Stuff
    [SerializeField] TextMeshProUGUI _letterText;
    void OnValidate()
    {
        if (_letterText == null)
            _letterText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _letterText.text = Letter;

        gameObject.name = Letter;
    }
    #endregion

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _initialPosition = _rect.localPosition;
        _anim = GetComponent<Animator>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Matched)
            return;

        _anim.Play("Null");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Matched)
            return;

        _rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Matched)
            return;

        if (!_isTouchingCorrectBlock)
        {
            _rect.localPosition = _initialPosition;

            if (_wronglyMatched)
            {
                SoundFXManager.instance.PlaySoundFXClip(_wrongLetterSound, transform.position, 1f, false);
                GameController.instance.OnMissLetter(_wronglyMatched); // isso serve pra cuidar das vidas e também pra expor a quantidade de erros
            }
                
            return;
        }
        
        SoundFXManager.instance.PlaySoundFXClip(_correctLetterSound, transform.position, 1f, false);

        gameObject.transform.SetParent(_letterContainer.transform); // prende a letra no container
        _rect.position = _letterContainer.GetComponent<RectTransform>().position; // prende a letra no seu container
        Matched = true;

        _letterContainer.tag = "Untagged"; // para evitar que mais de uma letra seja posta no mesmo letterContainer
        _letterContainer.GetComponent<LetterContainer>().Matched = true; // coloca o container como completo


        HintManager.instance.ResetHintTimer(); // reseta o timer pra dicas, já que o jogador conseguiu acertar uma das letras
        HintManager.instance.ResetHintIndex(); // reseta o index da hint, para que ele não use hints fortes depois de ter resetado
        GameController.instance.OnMatchLetter(); // testa se todas as letras ja foram colocadas no lugar correto
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LetterCheck(other.gameObject, true);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        LetterCheck(other.gameObject, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LetterCheck(other.gameObject, false);
    }

    private void LetterCheck(GameObject other, bool entering)
    {
        if (!other.CompareTag("LetterContainer"))
        {
            _wronglyMatched = false;
            return;
        }

        LetterContainer lt = other.GetComponent<LetterContainer>();
        _wronglyMatched = lt != null; // isso serve pra contabilizar os erros

        if (lt == null || lt.Letter != Letter)
            return;

        _letterContainer = entering ? other : null;
        _isTouchingCorrectBlock = entering;
    }

}
