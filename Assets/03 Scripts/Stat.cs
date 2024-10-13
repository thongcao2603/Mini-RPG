using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;
    [SerializeField] private TextMeshProUGUI textStat;
    [SerializeField] private float lerpSpeed;
    private float currentFill;
    public float MyMaxValue { get; set; }

    private float currentValue;

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // kiểm tra không cho vượt quá max và nhỏ hơn 0
            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }
            // tính giá trị để fill ô stat
            currentFill = currentValue / MyMaxValue;
            textStat.text = currentValue + "/" + MyMaxValue;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {   // tạo hiệu ứng giảm máu lần lần, chứ không nhảy từng nấc
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float _currentValue, float _maxValue)
    {   // init giá trị max và giá trị current (init giá trị current trước gặp bug)
        MyMaxValue = _maxValue;
        MyCurrentValue = _currentValue;

    }
}
