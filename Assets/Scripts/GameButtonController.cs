using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    public int value, colorValue;
    TMPro.TextMeshProUGUI buttonText;
    public bool isClicking;

    void Start()
    {
        buttonText = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        SetColor();
        GetComponent<Button>().onClick.AddListener(ClickEvent);
    }

    private void SetColor()
    {
        if (value == 0)
        {
            colorValue = Random.Range(1, gridSystemSO.useableMaxNumber + 1);
            value = colorValue;
        }
        else
        {
            colorValue = value;
        }
        GetComponent<Image>().color = gridSystemSO.colors[colorValue];
        buttonText.text = value.ToString();
    }


    private void ClickEvent()
    {
        if (value != 5)
        {
            value++;
            SetColor();
        }

    }

}
