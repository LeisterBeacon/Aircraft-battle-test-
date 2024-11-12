using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    static Text amountText;
    static Image coolDownImage;
    private void Awake()
    {
        amountText = transform.Find("Amount Text").GetComponent<Text>();
        coolDownImage=transform.Find("CoolDown Image").GetComponent<Image>();
    }

    private void Start()
    {

        ScoreManager.Instance.ResetScore();
    }

    public static void UpdateText(int amount)
    {
        amountText.text = "X"+amount.ToString();
    }
    public static void UpdateCooldownImage(float fillAmount)
    {
        coolDownImage.fillAmount = fillAmount;
    }
}
