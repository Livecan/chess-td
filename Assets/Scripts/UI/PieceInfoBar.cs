using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieceInfoBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Piece piece;

    Slider slider;
    Slider Slider
    {
        get
        {
            if (slider == null)
            {
                slider = this.gameObject.GetComponentInChildren<Slider>();
            }
            return slider;
        }
    }

    [SerializeField] RawImage strengthIcon;

    [SerializeField] Image strengthPanel;

    // Start is called before the first frame update
    void Start()
    {
        Slider.onValueChanged.AddListener(UpdateTextValue);
        Slider.maxValue = piece.HealthPoints;
        Slider.value = piece.HealthPoints;
        piece.OnChangeHP.AddListener(UpdateValue);

        for (int i = 1; i < piece.Strength; i++)
        {
            Debug.Log(strengthIcon.rectTransform.rect.width);

            RawImage imageClone = Instantiate(strengthIcon, strengthPanel.transform);
            imageClone.rectTransform.localPosition = new Vector3(i * strengthIcon.rectTransform.rect.width, 0, 0);
        }
    }

    void UpdateValue(int value)
    {
        Slider.value = value;
    }

    void UpdateTextValue(float value)
    {
        text.text = Mathf.RoundToInt(value) + "/" + Slider.maxValue;
    }
}
