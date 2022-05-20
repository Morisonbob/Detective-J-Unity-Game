using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfileMannager : MonoBehaviour
{
    string path;
    string newPath;
    public string nomeArquivo;
    public Falas descricoes;

    // Use this for initialization
    void Start()
    {
        //Armazena qual a pasta que contém os arquivos JSON. Só funciona no projeto, não na build
        path = Application.dataPath + "/Json/";
        //Referencia o nome e caminho onde esta o arquivo JSON
        newPath = path + nomeArquivo;
        //Passa o texto dentro do arquivo para uma string
        string jsonString = File.ReadAllText(newPath);
        print("Leu de: " + newPath);
        //converte o arquivo JSON em um objeto no qual o JSON é baseado
        descricoes = JsonUtility.FromJson<Falas>(jsonString);
    }


}
