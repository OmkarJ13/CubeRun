using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpWheelButton : MonoBehaviour
{
    public PowerUpType type;
    public string itemName;
    public string itemDescription;

    [SerializeField] private TextMeshProUGUI selectedItemText;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;

    [SerializeField] private PowerUpWheel powerUpWheel;

    private AudioManager audioManager;
    private Animator animator;
    private Toggle toggle;

    private static readonly int Selected = Animator.StringToHash("Selected");

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();
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
        animator.SetBool(Selected, true);
        toggle.image.color = Color.yellow;

        selectedItemText.text = itemName.ToUpper();
        selectedItemDescription.text = itemDescription;
        powerUpWheel.selectedButton = this;
        
        audioManager.PlayClip("toggleClick");
    }

    public void ButtonDeselected()
    {
        animator.SetBool(Selected, false);
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
