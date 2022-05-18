using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script temporario, dando os moldes para o futuro JSON

public class IdItem : MonoBehaviour {
    
	public string nomeObjeto;
    //Uma identificação numerica para fins de checagem
	public int valorObjeto;

    //Variavel que informa se tem um puzzle além do texto
    public bool puzzle;

    //O canvas do puzzle, se tiver
    public GameObject canvasPuzzle;

    //Vai ficar aqui pra ter um maior controle de em que ponto a fala está.
    //Por exemplo, esse número pode ser usado para pular para falas especificas.
    public int numeroDaFala;

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

    //TODO: Definir ids de verificação para mudança de Imagem

}
