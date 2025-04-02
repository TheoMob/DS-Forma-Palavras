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

        // Se o arquivo não existir, cria com cabeçalhos
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "ID_Sessao, Inicio, Fim, Duracao_Sessao, ID_Administrador, ID_Aluno, Niveis_Completados, Dados_Niveis\n");

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

        string levelData = $"Nível: {levelName} | " +
                        $"Tempo: {timeSpent}s | " +
                        $"Vidas Perdidas: {livesLost} | " +
                        $"Dicas Usadas: {hintsUsed} | " +
                        $"Finalizado: {(levelCompleted ? "Sim" : "Não")} | " +
                        $"Derrotas: {amountOfLoses}\n\n";

        levelDataList.Add(levelData);
    }

    public void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level")) // significa que tava no meio de um level
            GameController.instance.UpdateSessionLogger();

        EndTimeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SessionDuration = (float)(System.DateTime.Parse(EndTimeStamp) - System.DateTime.Parse(BeginTimeStamp)).TotalSeconds;

        // Junta todas as informações dos níveis em uma única string
        string levelsData = string.Join(" ; ", levelDataList);

        // Cria a linha da sessão
        string sessionData = $"{SessionId}, {BeginTimeStamp}, {EndTimeStamp}, {SessionDuration} segundos, {AdminID}, {PatientID}, {CompletedLevels} niveis completos, \"{levelsData}\n";

        // Salva no arquivo CSV
        File.AppendAllText(filePath, sessionData);
    }

    public string GetFilePath()
    {
        return filePath; // Para debug
    }
}
