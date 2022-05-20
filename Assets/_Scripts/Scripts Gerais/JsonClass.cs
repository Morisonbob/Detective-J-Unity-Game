using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonClass : MonoBehaviour
{

}

//TODO: Adicionar um giro nas imagens de montar

[System.Serializable]
public class Falas
{
    public List<Dialog> dialogues;
}

[System.Serializable]
public class Dialog
{
    //Fala propriamente dita
    public string falaPersonagem;
    //Nome do personagem, pode variar entre o nome próprio, ???? e um apelido
    public string nomePersonagem;
    //Código HEX da cor, exemplo FFFFFF
    public string corTexto;
    //Nome da imagem que vai ser usada como retrato do personagem quando ele estiver falando
    public string nomePortrait;
    //Nome da animação que vai tocar dependendo da fala (Vai dar merda)
    public string nomeAnim;
    //Nome do arquivo de audio que vai ser o barulho de escrevendo na tela
    //Banido por tempo indeterminado, pois não muda nada
    //public string nomeSom;
    //Codigo que vai ser armazenado na lista de escolhas feitas
    public string codigoEscolha;
    //Lista de condiçãos necessária pra esse texto aparecer, basicamente qual escolha anterior
    //deve ter sido feita pra poder entrar nessa fala, pode ter mais de um código de escolha
    public List<string> condicaoChamada = new List<string>();
    //Velocidade em que o texto aparece
    public float velocidadeTexto;
    
    public int numAnt;
    
    public int numTexto;
    
    public int numProx;
    
    public int choiceDialog;
    //
    public bool changeSide;
    //String que substituiu os bool de isEnd, isInfo etc
    //Tipos: END/ INFO/ CHOICE/ RETURNINFO
    public string type;
}
