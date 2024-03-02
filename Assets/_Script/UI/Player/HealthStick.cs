using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthStick : MonoBehaviour
{
    public Image PlayerHealth;
    public Image ViscousEffect;
    public Text Health;
    public PlayerController PlayerController_scr;
    [Range(1, 10)] public float LerpSpeed;
    public int PlayerHealthValue =100;
    public int PlayerHealthValue_Max=100;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController_scr = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    public void Update()
    {
        ChangeUI();
    }

    public void ChangeUI()
    {
        PlayerHealthValue = PlayerController_scr.Health_Value;
        PlayerHealthValue_Max = PlayerController_scr.Fac_MaxHealth;
        float current_fillamount = ViscousEffect.fillAmount;
        PlayerHealth.fillAmount = (float)PlayerHealthValue / PlayerHealthValue_Max;
        ViscousEffect.fillAmount = Mathf.Lerp(current_fillamount,(float)PlayerHealthValue / PlayerHealthValue_Max, LerpSpeed*Time.fixedDeltaTime);
        Health.text = PlayerHealthValue.ToString() + "/" + PlayerHealthValue_Max.ToString();
    }
}
