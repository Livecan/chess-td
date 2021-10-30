using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfoBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    Slider slider;
    Slider Slider
    {
        get
        {
            if (slider == null)
            {
                slider = this.gameObject.GetComponent<Slider>();
            }
            return slider;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Slider.onValueChanged.AddListener(UpdateTextValue);
        Slider.value = slider.maxValue;
    }

    public void UpdateTextValue(float value)
    {
        text.text = Mathf.RoundToInt(value) + "/" + Slider.maxValue;
    }
}
