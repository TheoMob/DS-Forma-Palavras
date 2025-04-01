using UnityEngine;
using TMPro;  // Se estiver usando TextMeshPro

public class InputField : MonoBehaviour
{
    public TMP_InputField AdminInputField; // Para Input Field com TextMeshPro
    public TMP_InputField PatientInputField; // Para Input Field com TextMeshPro
    private string adminName;
    private string patientName;

    void Start()
    {
        if (GameController.instance.FirstTimeOnMainMenu == false)
        {
            gameObject.SetActive(false);
            return;
        }

        if (PlayerPrefs.HasKey("Professor"))
        {
            adminName = PlayerPrefs.GetString("Professor");
            AdminInputField.text = adminName; // Exibe o nome salvo no input field

            GameSessionLogger.instance.AdminID = adminName;
        }

        if (PlayerPrefs.HasKey("Aluno"))
        {
            patientName = PlayerPrefs.GetString("Aluno");
            PatientInputField.text = patientName; // Exibe o nome salvo no input field

            GameSessionLogger.instance.PatientID = patientName;
        }
    }

    public void SaveAdminName()
    {
        adminName = AdminInputField.text;
        PlayerPrefs.SetString("Professor", adminName);
        PlayerPrefs.Save();
        Debug.Log("Nome salvo: " + adminName);

        GameSessionLogger.instance.AdminID = adminName;
    }

    public void SavePatientName()
    {
        patientName = PatientInputField.text;
        PlayerPrefs.SetString("Aluno", patientName);
        PlayerPrefs.Save();
        Debug.Log("Nome salvo: " + patientName);  

        GameSessionLogger.instance.PatientID = patientName;
    }

}
