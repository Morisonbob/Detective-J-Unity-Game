using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlLockpick : MonoBehaviour
{


    //<Variáveis> ======================================================================================================
    #region
    public static int posicao;                      // Saber qual posição está o Jogador vai de 1 - Extremo esquerda a 4 - Extrema Direita

    GameObject Player;                              // GameObject que recebe o Jogador
    public Text textoTempo;                         // Texto que fica com o tempo.
                                                    //MUDAR PRA LIST DEPOIS
    public GameObject[] Pinos = new GameObject[4];  // Vetor que armazena os Pinos

    [HideInInspector]
    public bool iniciouDestrava,                    // Quando o jogador começa a destravar, essa booleana ativa o relógio
                destravouTranca,                    // Booleana que controla o fim do jogo. 
                podeMexerPino;                      // Booleana para controlar se o jogador pode, ou não, mexer o pino.

    public float tempo;                                 // Tempo. Relógio para reniciar a destrava da fechadura

    //</Viaráveis> =====================================================================================================
    #endregion

    // Inicializando Variáveis =========================================================================================
    #region

    void Awake()
    {
        podeMexerPino = true;
        iniciouDestrava = false;
        tempo = 10;
        posicao = 1;
    }

    void Start()
    {
        //MUDAR PARA PUBLIC PARA EVITAR ERROS
        Player = GameObject.FindWithTag("Player");
    }
    // =================================================================================================================
    #endregion

    void Update()
    {
        if (iniciouDestrava) { tempo -= Time.deltaTime; }                                   // Se a destrava tiver iniciado, o Relógio começa a contar regressivamente
        if (!destravouTranca) { textoTempo.text = "Tempo Restante: " + tempo.ToString("0.00"); }    // Enquanto o jogador não finalizar o jogo, o Texto mostra o tempo restante

        MovimentandoPinos();
        VerificadorPosicaoPinos();
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

    //POSSIVELMENTE MUDAR COMPLETAMENTE ISSO AQUI QUANDO EU NÃO TIVER PREGUIÇA
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
            for (int i = 0; i < 4; i++)
            {
                Pinos[i].GetComponent<Pino>().sendoMexido = false;              // Os Pinos iram voltar ao lugar inicial e eles não estarão sendo mexidos
                Pinos[i].GetComponent<Pino>().VoltarPosicao();
            }
        }
    }

    // Verificador das posições dos Pinos
    void VerificadorPosicaoPinos()
    {   // Se os 4 Pinos estiverem com suas variáveis Pino "Ok" (entre as duas linhas), o método irá ser executado.
        //Precisa meter um metodo recursivo nisso aqui pelo amor de deus
        if (Pinos[0].GetComponent<Pino>().pinoOK && Pinos[1].GetComponent<Pino>().pinoOK && Pinos[2].GetComponent<Pino>().pinoOK && Pinos[3].GetComponent<Pino>().pinoOK)
        {
            destravouTranca = true;                                                 // Seria o "finalizou"
            iniciouDestrava = false;                                                // Irá impedir que o relógio continue, para não resetar a posição iniciando o método Resetar()
            textoTempo.text = "Venceu arrombado";                                   // O texto mostrando que o jogador "Venceu"
        }
    }
}
