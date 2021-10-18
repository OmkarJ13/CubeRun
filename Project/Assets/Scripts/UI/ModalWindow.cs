using TMPro;
using UnityEngine;

public class ModalWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private string message;
    
    private void OnEnable()
    {
        messageText.text = message;
    }
}
