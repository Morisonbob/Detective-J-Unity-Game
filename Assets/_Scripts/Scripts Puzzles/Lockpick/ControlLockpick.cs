using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlLockpick : MonoBehaviour
{


    //<Variáveis> ======================================================================================================
    public static int posicao;                      // Saber qual posição está o Jogador vai de 1 - Extremo esquerda a 4 - Extrema Direita

    public GameObject Player;                       // GameObject que recebe o Jogador
    public Text textoTempo;                         // Texto que fica com o tempo.
                                                    //MUDAR PRA LIST DEPOIS
    public List<GameObject> Pinos;  //Lista que armazena os Pinos

    [HideInInspector]
    public bool iniciouDestrava;                    // Quando o jogador começa a destravar, essa booleana ativa o relógio
    public bool destravouTranca;                    // Booleana que controla o fim do jogo. 
    public bool podeMexerPino;                      // Booleana para controlar se o jogador pode, ou não, mexer o pino.

    public float tempo;                             // Tempo. Relógio para reniciar a destrava da fechadura
    bool podeSair = true;                           //Impedir que o player saia do puzzle de forma indevida



    // Inicializando Variáveis =========================================================================================

    void Awake()
    {
        podeMexerPino = true;
        iniciouDestrava = false;
        tempo = 10;
        posicao = 1;
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
    // =================================================================================================================

    void Update()
    {
        //Se a destrava tiver iniciado, o Relógio começa a contar regressivamente
        if (iniciouDestrava)
        {
            tempo -= Time.deltaTime;
        }
        //Enquanto o jogador não finalizar o jogo, o Texto mostra o tempo restante
        if (!destravouTranca)
        {
            textoTempo.text = "Tempo Restante: " + tempo.ToString("0.00");
        }

        MovimentandoPinos();
        VerificadorPosicaoPinos();
        SairDoPuzzle();
        Resetar();
    }

    // Quando o jogador estiver apertando o botão para fazer o Pino subir, será executado o método
    void MovimentandoPinos()
    {
        //Talvez mudar isso para UpArrow					
        if (Input.GetButton("Fire1") && podeMexerPino)
        {
            iniciouDestrava = true;                                                 // Para começar a contar o relógio
            Player.GetComponent<PlayerLockpick>().segurandoPino = true;              // Para fazer o jogador não movimentar-se entre os pinos
            Pinos[posicao - 1].GetComponent<Pino>().sendoMexido = true;             // O Pino acima do Jogador será movimentado, apenas ele.
        }

        if (Input.GetButtonUp("Fire1"))
        {                                           // Se o jogador soltar o botão de ação
            podeMexerPino = true;                                                   // Ele poderá movimentar um pino (Ver no Script Pino)
            Pinos[posicao - 1].GetComponent<Pino>().sendoMexido = false;            // O pino que estava sendo mexido, agora não estará mais
            Player.GetComponent<PlayerLockpick>().segurandoPino = false;         // E o jogador pode voltar a movimentar-se.
        }
    }

    // Se o jogador quiser OU o tempo(relógio) for inferior a zero, ele pode reiniciar algumas coisas.
    void Resetar()
    {
        if (Input.GetButtonDown("Jump") || tempo < 0)
        {
            destravouTranca = false;                                                // O jogo volta ao inicio, tirando o estado de "Finalizado"
                                                                                    //Mudar para uma variavel
            tempo = 10;                                                             // O tempo voltará ao tempo inicial - 10segundos
            iniciouDestrava = false;                                                // O relógio não irá voltar a contabilizar o tempo
                                                                                    //Que for zuado da porra, tenho que mudar isso aqui
            for (int i = 0; i < Pinos.Count; i++)
            {
                Pinos[i].GetComponent<Pino>().sendoMexido = false;              // Os Pinos iram voltar ao lugar inicial e eles não estarão sendo mexidos
                Pinos[i].GetComponent<Pino>().VoltarPosicao();
            }
        }
    }

    //Verificador das posições dos Pinos
    void VerificadorPosicaoPinos()
    {
        //Se os Pinos estiverem com suas variáveis Pino "Ok" (entre as duas linhas), o método irá ser executado.
        //Checa cada pino, independente do número de pinos
        for (int i = 0; i < Pinos.Count; i++)
        {
            //Se algum deles não está ok (entre as duas linhas) há a saida do método
            if (!Pinos[i].GetComponent<Pino>().pinoOK)
            {
                return;
            }
        }

        //Caso tenha passado por toda a checagem de ifs dentro do for, todos estão ok
        //se todos estão ok, executa o final
        //Impede o jogador de sair indevidamente
        podeSair = false;
        //Seria o "finalizou"
        destravouTranca = true;
        //Irá impedir que o relógio continue, para não resetar a posição iniciando o método Resetar()
        iniciouDestrava = false;
        //O texto mostrando que o jogador "Venceu"
        textoTempo.text = "Destrancado";
        //O prefab é desativado após 2 segundos, o tempo pra musica tocar
        //Usando o Invoke a partir do singleton do TextController por motivos de organização
        TextoParaJSON.singleton.Invoke("DesativaPuzzle", 2);
    }

    void SairDoPuzzle()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && podeSair)
        {
            Resetar();
            TextoParaJSON.singleton.PausaPuzzle();
        }      
    }
}
