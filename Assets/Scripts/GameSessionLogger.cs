using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionLogger : MonoBehaviour
{
    public static GameSessionLogger instance;
    private string filePath;
    private List<string> levelDataList = new List<string>();

    [Header("Variáveis guardadas Globais")]
    public string SessionId;
    public string BeginTimeStamp;
    public string EndTimeStamp;
    public float SessionDuration;
    public string AdminID;
    public string PatientID;
    public int CompletedLevels = 0;

    [Header("Variáveis qualitativas")]
    public int LivesLost = 0;
    public int HintsUsed = 0;
    public int AmountOfLosses = 0;
    public string Commentary;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        RequestPermissions();

        // Define o caminho do arquivo CSV
        filePath = Path.Combine(Application.persistentDataPath, "session_log.csv");

        // Se o arquivo não existir, cria com cabeçalhos organizados
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "ID_Sessao, Inicio, Fim, Duracao_Sessao, ID_Administrador, ID_Aluno, " +
                                        "Nivel, Tempo_Gasto, Vidas_Perdidas, Dicas_Usadas, Finalizado, Derrotas\n");
        }

        // Inicia a sessão
        SessionId = System.Guid.NewGuid().ToString();
        BeginTimeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    #if UNITY_ANDROID
    void RequestPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }
    #endif

    public void LogLevelData(string levelName, int timeSpent, int livesLost, int hintsUsed, bool levelCompleted, int amountOfLoses)
    {
        Debug.Log("Atualizando LogLevelData");

        // Cada entrada é uma linha separada com as colunas já organizadas
        string levelData = $"{SessionId}, {BeginTimeStamp}, {EndTimeStamp}, {SessionDuration}, {AdminID}, {PatientID}, " +
                           $"{levelName}, {timeSpent}, {livesLost}, {hintsUsed}, {(levelCompleted ? "Sim" : "Não")}, {amountOfLoses}";

        levelDataList.Add(levelData);
    }

    public void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level")) // Se estiver em um nível ao sair, atualiza o logger
            GameController.instance.UpdateSessionLogger();

        // Atualiza o tempo de término e a duração da sessão antes de salvar os dados
        EndTimeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SessionDuration = (float)(System.DateTime.Parse(EndTimeStamp) - System.DateTime.Parse(BeginTimeStamp)).TotalSeconds;

        // Atualiza os dados da sessão antes de salvar
        for (int i = 0; i < levelDataList.Count; i++)
        {
            string[] splitData = levelDataList[i].Split(',');
            levelDataList[i] = $"{SessionId}, {BeginTimeStamp}, {EndTimeStamp}, {SessionDuration}, {AdminID}, {PatientID}, " +
                            $"{splitData[6]}, {splitData[7]}, {splitData[8]}, {splitData[9]}, {splitData[10]}, {splitData[11]}";
        }

        // Salva os dados corrigidos no CSV
        File.AppendAllLines(filePath, levelDataList);

        Debug.Log($"Dados da sessão salvos em: {filePath}");
    }

    public string GetFilePath()
    {
        return filePath; // Para debug
    }
}
