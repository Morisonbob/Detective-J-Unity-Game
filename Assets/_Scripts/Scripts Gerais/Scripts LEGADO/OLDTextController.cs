/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OLDTextController : MonoBehaviour
{
    //OBS: Chamar as coroutines APENAS pelo método de STRING

    //Vai virar a classe do JSON no futuro, armazena os dados do personagem.
    public IdItem id;
    //Referencia ao script que controla o player
    [Tooltip("Mover o player EM CENA para aqui")]
    public PlayerControl player;

    //O panel onde vai aparecer tudo
    public GameObject PanelPrincipal;
    //Imagens que aparecem no texto
    public Image imagemPainel, imagemFalante;
    //Nome de quem fala e o que fala
    public Text textoNome, textoFala;
    //Backgrounds diferentes para o tipo de texto (Grito, padrão etc)
    public List<Sprite> panelBg;
    //Garante que o texto apareceu por completo
    bool linhaConcluida = false;

    //Um singleton, vai ser usado para cuidar das funções de puzzle
    public static OLDTextController singleton;

    //Preenchendo o singleton
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(this.gameObject);
        }
    }


    //Ativa o painel de fala e seta os default
    public void AtivaPainel()
    {
        imagemPainel.sprite = panelBg[0];
        imagemFalante.sprite = id.retratoPersonagem[id.retratoAtual];
        textoNome.text = id.nomeObjeto;
        textoFala.text = id.textoVerificacao[id.falaAtual];
        PanelPrincipal.SetActive(true);
        StartCoroutine("AnimateText", textoFala.text);
    }

    //Desativa o painel geral de falas
    public void DesativaPainel()
    {
        PanelPrincipal.SetActive(false);
        player.conversando = false;
    }

    //Caso haja um puzzle, ativa ele para o jogador jogar
    public void AtivaPuzzle()
    {
        //Essa variavel vai fazer o player parar de se move
        player.conversando = true;
        //Ativa o canvas (prefabzin) do puzzle
        id.canvasPuzzle.SetActive(true);
    }

    //Desativa o puzzle, fechando o canvas e fazendo o jogador voltar a se mexer
    //Vai ser chamado pelo sigleton
    public void DesativaPuzzle()
    {
        id.canvasPuzzle.SetActive(false);
        id.puzzle = false;
        player.conversando = false;
    }

    //Fecha o puzzle, fazendo ele voltar ao inicio
    public void PausaPuzzle()
    {
        id.canvasPuzzle.SetActive(false);
        id.puzzle = true;
        player.conversando = false;
    }

    public void ProximaFala()
    {
        //Se o painel principal NÃO está ativo, não executa a função
        if (!PanelPrincipal.activeInHierarchy)
        {
            return;
        }

        //Caso, por algum acaso da natureza, o texto ainda esteja rodando, para ele
        StopCoroutine("AnimateText");

        //Caso seja apertado o botão de continuar a fala, antes da fala acabar
        if (!linhaConcluida)
        {
            //Faz o texto aparecer de uma vez só
            textoFala.text = id.textoVerificacao[id.falaAtual];
            //Muda o bool para que não se entre mais nessa função
            linhaConcluida = true;
            //Sai da função como um todo, para que o jogador tenha que apertar o botão novamente para passar de fala
            return;
        }

        //Pega o total de falas que tem na lista do personagem
        id.totalDeFalas = id.textoVerificacao.Count;

        //Passa para a próxima fala
        id.falaAtual++;

        //Se passar do limite de falas, encerra a conversa
        if (id.falaAtual >= id.totalDeFalas)
        {

            DesativaPainel();
            //Ativa um puzzle no final, se houver
            if (id.puzzle)
            {
                AtivaPuzzle();
            }

            //Volta o texto depois, no ponto de pausa
            id.falaAtual = id.pauseAt;
        }
        //Passa o texto de acordo com o indice em que ele está agora
        textoFala.text = id.textoVerificacao[id.falaAtual];
        //Antes de iniciar a corotina, volta o bool a ser falso
        linhaConcluida = false;
        //Inicia a passagem do texto letra a letra
        StartCoroutine("AnimateText", textoFala.text);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //"Metódo" que faz letras serem passadas uma a uma de acordo com a velocidade definida no container
    IEnumerator AnimateText(string dialogueText)
    {
        //Zera o texto antes de iniciar o letra a letra
        textoFala.text = "";
        //Armazena o total de letras que tem na frase
        int lenghtFala = dialogueText.Length;
        //Inicia um contador pra checagem posterior
        int contadordeletras = 0;
        print("Entrou no IEnumerator");

        //TODO: Adicionar um som que varia de velocidade de acordo com a velocidade do texto e não toque quando aparecem espaços

        //Passa as letras de uma a uma
        foreach (char letter in dialogueText)
        {
            //Passa as letras de uma a uma
            textoFala.text += letter;
            //Aumenta o numero de letras que foram passadas no contador
            contadordeletras++;
            //Se o numero de letras passadas for igual ao total de letras que devem ser passadas
            //Deixa a variavel de controle verdadeira
            if (contadordeletras == lenghtFala)
            {
                linhaConcluida = true;
            }
            //Tempo de espera pra executar o foreach novamente, faz com que as letras
            //sejam passadas na velocidade dentro do parametro
            yield return new WaitForSeconds(id.velocidadeFala);
        }
    }
}
*/

