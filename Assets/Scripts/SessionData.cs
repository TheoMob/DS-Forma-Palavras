using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Xml.Linq;


public class SessionData : MonoBehaviour
{


    public static SessionData instance;
    public string filePath = "";
    public string id;
    public float sessionTime;
    public int lostLives = 0;
    public int numClues = 0;
    public string nameResp;
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    CsvCheck();
        //}
    }
    public void ResetSessionData()
    {
        this.lostLives = 0;
        this.numClues = 0;
    }

    public void ClueAddCount()
    {
        this.numClues++;
        return;
    }

    public void Lifeloss()
    {
        this.lostLives++;
        return;
    }
    public void GetPath()
    {
        string appPath = System.IO.Directory.GetCurrentDirectory();
        this.filePath = appPath + "/Dados.csv";
        return;
    }
    public void CsvCheck()
    {
        GetPath();
        StreamWriter writer = new StreamWriter(this.filePath, true);
        if (new FileInfo(this.filePath).Length == 0)
        {
            writer.WriteLine("ID, Data, Duração da Sessão, Profissional Responsável, ID do Paciente, Nome do nível,\n Tempo gasto no nível, Dicas Usadas, Nivel finalizado, Vidas Perdidas, Comentario do Profissional");
        }
        writer.Close();
        Debug.Log($"Dados salvos em: {this.filePath} por check");
        return;
    }
    public void saveData(float sessionTime, string paciID, int level, float spentTime, int nivelEnd, string commentary)
    {
        DateTime dt = DateTime.Now;
        string date = dt.ToString("yyyyMMddHHmmss");
        GetPath();
        string data = ($"{this.id}, {date}, {sessionTime}, {nameResp}, {paciID}, {level}, {spentTime}, {this.numClues}, {nivelEnd}, {this.lostLives}, {commentary}");
        WriteToCSV(data);
    }

    public void WriteToCSV(string data)
    {
        StreamWriter writer = new StreamWriter(this.filePath, true);
        writer.WriteLine(data);
        writer.Close();
        Debug.Log($"Dados salvos em: {this.filePath}");

    }
}
