using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ProgressBar : MonoBehaviour
{
    private Slider _slider;
    private Vector3 _goldFirstPosition, _gemFirstPosition;
    private int _goldProduction, _gemProduction;
    private float _cooldownProduction = 0f;

    [SerializeField] private Text GoldProductionText, GemProductionText, TimeText;
    [SerializeField] private RectTransform FloatingGold, FloatingGem;
    [SerializeField] private float FloatTime = 1.5f;
    [SerializeField] private float FloatSpeed = 0.00005f;

    private void Awake()
    {
        _goldFirstPosition = FloatingGold.localPosition;
        _gemFirstPosition = FloatingGem.localPosition;
        _slider = GetComponent<Slider>();
    }
    public void SetBar(float currentValue, float maxValue)
    {
        _slider.value = currentValue / maxValue;
        TimeText.text = Mathf.Ceil(maxValue - currentValue).ToString("0");
    }
    public void FloatText(int goldProduction, int gemProduction)
    {
        _goldProduction = goldProduction;
        _gemProduction = gemProduction;
        StartCoroutine(nameof(FloatTextRoutine));
    }
    private IEnumerator FloatTextRoutine()
    {  
        FloatingGold.gameObject.SetActive(true);
        FloatingGem.gameObject.SetActive(true);
        
        GoldProductionText.color = Color.green;;
        if (_goldProduction<0)
        {
            GoldProductionText.text = "-";
            GoldProductionText.color= Color.red;
        }
        else
        {
            GoldProductionText.text = "+";
            if (_goldProduction==0)
            {
                FloatingGold.gameObject.SetActive(false);
            }
        }
        GoldProductionText.text += _goldProduction.ToString();

        GemProductionText.color = Color.green;
        if (_gemProduction < 0)
        {
            GemProductionText.text = "-";
            GemProductionText.color = Color.red;
        }
        else
        {
            GemProductionText.text = "+";
            if (_gemProduction == 0) FloatingGem.gameObject.SetActive(false);
        }
        GemProductionText.text += _gemProduction.ToString();
        
        while (_cooldownProduction <= FloatTime)
        {
            _cooldownProduction += Time.deltaTime;
            FloatingGold.position += Vector3.up * FloatSpeed * Mathf.Sin(_cooldownProduction);
            FloatingGem.position += Vector3.up * FloatSpeed * Mathf.Sin(_cooldownProduction);
            yield return null;
        }

        FloatingGold.localPosition= _goldFirstPosition;
        FloatingGem.localPosition = _gemFirstPosition;
        
        FloatingGold.gameObject.SetActive(false);
        FloatingGem.gameObject.SetActive(false);
        
        _cooldownProduction = 0f;
    }
}
