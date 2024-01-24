using UnityEngine.UI;
using UnityEngine;
public class FloatingText : MonoBehaviour
{
    private float time = 0f;
    private RectTransform _rectTransform;
    private Text _text;
    
    [SerializeField] private float FloatingTime,FloatingSpeed;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _rectTransform.position += Vector3.up * FloatingSpeed;
        time += Time.deltaTime;
        if (time >= FloatingTime)
        {
            Destroy(gameObject);
        }
    }
    public void Init(int value,Vector3 position)
    {
        _text.text = "";
        if (value > 0)
        {
            _text.color = Color.green;
            _text.text = "+";
        }
        else
        {
            _text.color = Color.red;
            _text.text = "-";
        }

        _text.text = value.ToString();

        _rectTransform.localPosition = position;
    }
}
