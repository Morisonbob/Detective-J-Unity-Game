using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Container que guarda algumas informações
//TODO: Rever e talvez refazer isso tudo

public class IdItem : MonoBehaviour {
    
    //Nome do Objeto
	public string nomeObjeto;
    //Uma identificação numerica para fins de checagem
	public int valorObjeto;

    //Variavel que informa se tem um puzzle além do texto
    public bool puzzle;

    //O canvas do puzzle, se tiver
    public GameObject canvasPuzzle;

    //Lista de retratos do personagem (Usado pra mostrar retratos com outras expressões)
    public List<Sprite> retratoPersonagem;
    //Falas do personagem/coisa
    public List<string> textoVerificacao;


//Qual retrato está sendo mostrado
    public int retratoAtual;
    //Em qual fala atual a lista de falas está
    public int falaAtual;
    //Numero total de falas, é aonde a fala vai terminar
    public int totalDeFalas;
    //Local de onde a fala deve voltar após ter sido toda concluida
    public int pauseAt;

    //
    public int startAt;
    //
    public int returnFromPuzzle;
    //Local de onde a fala deve voltar após ter sido toda concluida
    public int returnSpeech;
    //Em que pedaço da fala o puzzle se inicia
    public int puzzleIn;
    //Nome do arquivo Json que esse personagem vai usar
    public string nomeArquivo;
    //Por hora não faz nada
    //Som que vai tocar quando o texto estiver sendo escrito
    public AudioClip somzinho;
    //
    public bool retorno;
    //
    public bool retornoPuzzle;
}
