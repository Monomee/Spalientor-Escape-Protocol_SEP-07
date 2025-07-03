using UnityEngine;

public class EnterPasswordController : MonoBehaviour
{
    [SerializeField] string howToFindPassword;
    [SerializeField] string correctPassword = "7605"; // Correct password for the door
    [SerializeField] GameObject door; // Reference to the door GameObject
    [SerializeField] GameObject passwordUI; // Reference to the password UI GameObject
    bool done = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (done) return; 
            passwordUI.SetActive(true);
            UIManagerMap1.Instance.passwordUI.correctPassword = correctPassword;
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (done) return;
            passwordUI.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckPassword();
        }
    }
    public void CheckPassword()
    {
        if (UIManagerMap1.Instance.passwordUI.isPasswordCorrect)
        {
            door.SetActive(false);
            passwordUI.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            done = true;
            UIManagerMap1.Instance.passwordUI.isPasswordCorrect = false; // Reset the password check for next time
        }
    }
}
