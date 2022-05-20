using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pino : MonoBehaviour
{


    // <Variáveis> ====================================================================================================

    Vector3 posicaoInicial;                                         // A posição inicial do Pino
    public GameObject linhaInferior, linhaSuperior, martelinho;     // Recebe o GameObject das Linhas e do Player/Martelinho
    public float forca;                                             // A "força" a qual o pino sofre para sair do lugar.

    //[HideInInspector]
    //Public para poder ser visto por outras funções
    public bool sendoMexido;                                // Saber se este Pino está sendo mexido.
    public bool pinoOK;                                     // Se o pino está, ou não, OK (Entre as duas linhas)

    // <Iniciando as variáveis> ========================================================================================

    void Awake()
    {
        //Só pra questões de teste, a força pode variar a cada pino
        forca = 1.5f;
        posicaoInicial = transform.position;
    }

    //Mudei tudo pra public pra não dar merda com tags e etc
    void Start()
    {

    }

    void Update()
    {
        Subindo();
        VerificadorOk();
    }

    void Subindo()
    {
        //Se esse pino estiver sendo mexido, ele irá para "cima"
        if (sendoMexido)
        {
            //Se, enquanto ele estiver subindo, ele encostar na linha vermelha
            //OBS: O valor colocado como .5 é devido a escala do objeto ser 1 e a posição dele contar a partir do centro
            //O valor .5f poderia ser substituido por Pino.transform.localScale/2
            if (transform.position.y + 0.5f > linhaSuperior.transform.position.y)
            {
                //O pino irá voltar para o seu lugar de origem
                VoltarPosicao();
                //Além de obrigar o jogador a soltar o botão de levantar o pino (o pino para de levantar)
                //Importante para a execução do GetNuttonUp no código ControlLockpick
                martelinho.GetComponent<PlayerLockpick>().segurandoPino = false;
                //Jogador pode voltar a mexer o pino
                GetComponentInParent<ControlLockpick>().podeMexerPino = false;
                sendoMexido = false;

            }
            //A quantidade de força que o pino vai recebendo para subir.
            //Podemos brincar com que cada pino seja uma força diferente.
            transform.position += new Vector3(0, forca * Time.deltaTime, 0);
        }

        //Se o Pino não estiver sendo mexido, será verificado a posição do pino, se ele estiver em contato com a linha vermelha ele irá voltar para a posição inicial	
        //O valor colocado como .5 é devido a escala do objeto ser 1 e a posição dele contar a partir do centro
        //O valor .5f poderia ser substituido por Pino.transform.localScale/2
        //TODO: Deixar o metódo Subindo mais generico
        else
        {
            if (transform.position.y - 0.5f < linhaInferior.transform.position.y ||
                transform.position.y + 0.5f > linhaSuperior.transform.position.y)
                VoltarPosicao();
        }
    }

    //Esse método verifica se o Pino está OK ou não. Ele estará "ok" se estiver entre as linhas vermelhas
    void VerificadorOk()
    {
        //Verifica se está entre as duas linhas vermelhas
        if (transform.position.y - 0.5f > linhaInferior.transform.position.y &&
            transform.position.y + 0.5f < linhaSuperior.transform.position.y)
            pinoOK = true;
        else
            pinoOK = false;
    }

    // Método que faz o Pino voltar a sua posição inicial
    public void VoltarPosicao()
    {
        transform.position = posicaoInicial;
    }
}
