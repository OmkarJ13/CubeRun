using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerUpWheelButton : MonoBehaviour
{
    public PowerUpType type;
    public string itemName;
    public string itemDescription;

    [SerializeField] private TextMeshProUGUI selectedItemText;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;

    [SerializeField] private PowerUpWheel powerUpWheel;
    [SerializeField] private UnityEvent buttonSelected;
    [SerializeField] private UnityEvent buttonDeselected;

    private AudioManager audioManager;
    private Toggle toggle;
    
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate {OnValueChanged(toggle);});
    }

    private void OnEnable()
    {
        toggle.isOn = false;
        selectedItemText.text = "";
        selectedItemDescription.text = "";
    }

    public void ButtonSelected()
    {
        buttonSelected.Invoke();
        toggle.image.color = Color.yellow;

        selectedItemText.text = itemName.ToUpper();
        selectedItemDescription.text = itemDescription;
        powerUpWheel.selectedButton = this;
        
        audioManager.PlayClip("toggleClick");
    }

    public void ButtonDeselected()
    {
        buttonDeselected.Invoke();
        toggle.image.color = new Color(225, 225, 225, 200);

        selectedItemText.text = "";
        selectedItemDescription.text = "";
        powerUpWheel.selectedButton = null;
    }

    public void OnValueChanged (Toggle change)
    {
        if (change.isOn)
        {
            ButtonSelected();
        }
        else
        {
            ButtonDeselected();
        }
    }
}
