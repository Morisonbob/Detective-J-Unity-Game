using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CadeadoComCodigo : MonoBehaviour
{
    //TODO: Fazer com que o jogador possa sair do puzzle sem o terminar

    //Pai das duas setinhas
    public GameObject cursor;
    //Lista de numeros que aparecem na tela
    public List<Text> textos = new List<Text>();
    //Pega qual número está selecionado no momento (PRECISA que esse objeto tenho o script NumCadeado attached)
    public Text textoUtilizado;
    //Texto de vitória
    public Text fim;
    //Pega o audiosource que será responsavel pelo audio
    public AudioSource audioS;
    //Referencia ao jogador
    public Movimento player;
    //Referencia ao prefab do puzzle
    public GameObject canvas;

    //Pega qual número está selecionado no momento
    public int textoAtual;
    //Qual número (em texto atual) está aparecendo na tela agora
    public int numAtual;

    //Variaveis que vão pegar os inputs Raw
    float InputX, InputY;

    bool moveu;

    public string textoFinal, resposta;

    // Use this for initialization
    void Start()
    {
        //Marca que o que está selecionado no momento é o primeiro numero
        textoUtilizado = textos[textoAtual];
    }

    // Update is called once per frame
    void Update()
    {
        //Pega os inputs de -1 a 1
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");

        //Nome autoexplicativo
        MovimentoCursor();

        //Coloca o texto final para ser vazio
        textoFinal = "";

        //Passa os numeros que foram colocados como string para a checagem posterior
        for (int i = 0; i < textos.Count; i++)
        {
            string c = textos[i].text.ToString();
            textoFinal += c;
        }

        //Checa se os numeros passados são iguais a resposta do puzzle
        if (textoFinal == resposta)
        {
            fim.enabled = true;
            //O prefab é desativado após 2 segundos, o tempo pra musica tocar
            Invoke("DesativaPuzzle", 2);
            //Toca a musica de acerto
            //Tá como oneshot pq tava repetindo e eu não sei pq
            audioS.PlayOneShot(audioS.clip);
        }

        print(player.conversando);
    }

    void MovimentoCursor()
    {
        if (moveu)
        {
            //Checa se parou de mover e deixa o bool falso
            if (InputX == 0 && InputY == 0)
            {
                moveu = false;
            }
        }

        else
        {
            if (InputX != 0)
            {
                //Quando o jogador move o cursor, passa para a variavel textoAtual em que número o cursor está
                //Esse (int) é um cast de int, procura por cast C# para entender melhor
                textoAtual += (int)InputX;
                //Checa se está ou não passando do limite de números, se estiver, volta pra o inicio
                //ou pra o final, respectivamente
                if (textoAtual == -1)
                {
                    textoAtual = textos.Count;
                }
                else if (textoAtual == textos.Count)
                {
                    textoAtual = 0;
                }

                //Faz com que a posição dos cursores seja a posição do numero em que ela está no eixo X apenas
                cursor.GetComponent<RectTransform>().localPosition = new Vector3
                    (textos[textoAtual].GetComponent<RectTransform>().localPosition.x,
                     cursor.GetComponent<RectTransform>().localPosition.y, 0);

                //Passa qual texto está selecionado para a variavel que lida com o objeto Text
                textoUtilizado = textos[textoAtual];
                //Moveu se torna verdadeiro
                moveu = true;

            }

            if (InputY != 0)
            {
                //Moveu se torna verdadeiro
                moveu = true;
                //Pega o número (int) que está dentro da variavel numAtual
                numAtual = textoUtilizado.GetComponent<NumCadeado>().meuNumero;
                //Soma com o input que houve no eixo y (sempre 1)
                numAtual += (int)InputY;
                //Verifica se passa de 9 ou 0 e reseta o número pra 0 e 9, respectivamente
                if (numAtual <= -1)
                {
                    numAtual = 9;
                }
                else if (numAtual >= 10)
                {
                    numAtual = 0;
                }
                //Passa as informações de que número está armazenado
                textoUtilizado.GetComponent<NumCadeado>().meuNumero = numAtual;
                textoUtilizado.text = numAtual.ToString();
            }
        }
    }

    void DesativaPuzzle()
    {
        canvas.SetActive(false);
        print(player.conversando);
        player.conversando = false;
        print(player.conversando);
    }

}
