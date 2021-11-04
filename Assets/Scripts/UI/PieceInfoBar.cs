using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] Image strengthPanel;

    // Start is called before the first frame update
    void Start()
    {
        Slider.onValueChanged.AddListener(UpdateTextValue);
        Slider.maxValue = piece.MaxHitPoints;
        Slider.value = piece.HealthPoints;
        piece.OnChangeHP.AddListener(UpdateValue);
        piece.OnChangeStrengthBonus.AddListener(UpdateStrengthValue);

        UpdateStrengthValue(piece.Strength);
    }

    void UpdateStrengthValue(int value)
    {
        RawImage[] strengthIcons = strengthPanel.gameObject.GetComponentsInChildren<RawImage>(true);

        int i = 0;
        foreach (RawImage strengthIcon in strengthIcons)
        {
            strengthIcon.gameObject.SetActive(i++ < value);
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
