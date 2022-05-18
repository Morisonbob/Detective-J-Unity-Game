using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    //Vai virar a classe do JSON no futuro, armazena os dados do personagem.
    public IdItem id;
    //Referencia ao script que controla o player
    public Movimento Player;

    //O panel onde vai aparecer tudo
    public GameObject PanelPrincipal;
    //Imagens que aparecem no texto
    public Image imagemPainel, imagemFalante;
    //Nome de quem fala e o que fala
    public Text textoNome, textoFala;
    //Backgrounds diferentes para o tipo de texto (Grito, padrão etc)
    public List<Sprite> panelBg;

    //Ativa o painel de fala e seta os default
    public void AtivaPainel()
    {
        imagemPainel.sprite = panelBg[0];
        imagemFalante.sprite = id.retratoPersonagem[id.retratoAtual];
        textoNome.text = id.nomeObjeto;
        textoFala.text = id.textoVerificacao[id.falaAtual];
        PanelPrincipal.SetActive(true);
    }

    //Desativa o painel geral de falas
    public void DesativaPainel()
    {
        PanelPrincipal.SetActive(false);
        Player.conversando = false;
    }

    //Caso haja um puzzle, ativa ele para o jogador jogar
    public void AtivaPuzzle()
    {
        id.canvasPuzzle.SetActive(true);
        //Fingir que o puzzle nunca existiu para ele não ser mais tocado
        id.puzzle = false;
        //Essa variavel vai fazer o player parar de se move
        Player.conversando = true;
    }

    public void ProximaFala()
    {
        //Se o painel principal NÃO está ativo, não executa a função
        if (!PanelPrincipal.activeInHierarchy)
        {
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
            //Caso haja um puzlle, ativa o puzzle após terminar a conversa
            if (id.puzzle)
            {
                AtivaPuzzle();
            }
            //Volta o texto depois no ponto de pausa
            id.falaAtual = id.pauseAt;
        }
        //Mostra o texto de acordo com o indice em que ele está agora
        textoFala.text = id.textoVerificacao[id.falaAtual]; 
    }

    //TODO: Fazer com que o Id seja passado de acordo com o objeto que está proximo, não neccesáriamente nesse script

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.T))
        {
            AtivaPainel();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            DesativaPainel();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ProximaFala();
        }
        */
    }
}
