using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    //Referencia ao Rigidbody do personagem (Usado pra controlar fisica)
    Rigidbody2D rigid;
    //Referencia ao animator do personagem
    Animator anim;
    //Velocidades de movimento base 
    //(Não precisam ser iguais, melhor que não sejam, para dar impressão de que algo no fundo está mais distante)
    public float velHorizontal, velVertical;
    //Velocidade de corrida
    public float corridaHorizontal, corridaVertical;
    [Tooltip("Número usado no OverlapCircle")]
    public float circuloDeChecagem;
    [Tooltip("A distancia entre o player e o item proximo tem que ser menor do que esse número")]
    public float checagemDeDistancia;
    //Checa se o player está conversando e portanto não pode se mexer
    public bool conversando;
    //Layer que define o que é ou não interagivel
    public LayerMask interagiveis;
    //
    public RaycastHit2D hit;
    //Bool que diz se o personagem está ou não virado para a esquerda
    //Será usado no escript TextoParaJson para controlar de que lado o texto aparece
    public bool flip;

    [Tooltip("Arrasta o Canvas prak")]
    public TextoParaJSON controladorTexto;
    //Pega o ID do objeto que está sendo analisado
    public IdItem idControladorTexto;
    //Gameobject do TextMeshPro
    public GameObject apertarE;
    //Colisor que checa se tem algo interagivel próximo
    Collider2D temAlgoProximo;
    //Armazena o que interagivel está proximo
    public GameObject itemProximo;

    //Variaveis de controle para pode voltar a velocidade para o original
    float velBaseHorizontal, velBaseVertical;
    //Variaveis que vão pegar os inputs
    float moveEmX, moveEmY;

    //Inicializando variaveis
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        velBaseVertical = velVertical;
        velBaseHorizontal = velHorizontal;
        //O texto de interação não pode começar sendo visto
        apertarE.SetActive(false);
    }

    //Acho que vai ser usado, mas por hora tá só ai mesmo
    void FixedUpdate()
    {
        //Faz nada ainda, foi só um teste
        hit = Physics2D.Raycast(transform.position, Vector2.right,Mathf.Infinity, interagiveis);
        Debug.DrawRay(transform.position, Vector2.right, Color.red);

        if (hit.collider != null)
        {
            print(hit.collider.name);
        } 
    }

    public void FlipChar()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            flip = false;
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            flip = true;
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }

    void Update()
    {
        //Se não está em uma conversa ou puzzle, o player pode se mover
        if (!conversando)
        {
            Move();
            FlipChar();
        }
        //Se está em uma conversa, o icone de interação desaparece
        if (conversando)
        {
            apertarE.SetActive(false);
        }
        //Função que vê a disponibilidade de interagiveis no cenário
        ChecaItem();

        //Chama o caderno
        if(Input.GetKeyDown(KeyCode.Return) && !conversando)
        {
            CadernoManagger.instance.AtivaCaderno();
        }
        else if(Input.GetKeyDown(KeyCode.Return) && !conversando)
        {
            CadernoManagger.instance.DesativaCaderno();
        }

        if(CadernoManagger.instance.cadernoAtivo && Input.GetKeyDown(KeyCode.E))
        {
            CadernoManagger.instance.PassaAba(true);
            CadernoManagger.instance.SelecionaAba();
        }
        else if(CadernoManagger.instance.cadernoAtivo && Input.GetKeyDown(KeyCode.Q))
        {
            CadernoManagger.instance.PassaAba(false);
            CadernoManagger.instance.SelecionaAba();
        }

        //TODO: Muda isso pra button depois
        //Muda as páginas
        if(CadernoManagger.instance.cadernoAtivo && Input.GetKeyDown(KeyCode.D))
        {
            CadernoManagger.instance.PassaPagina(true);
        }
        else if (CadernoManagger.instance.cadernoAtivo && Input.GetKeyDown(KeyCode.A))
        {
            CadernoManagger.instance.PassaPagina(false);
        }
    }

    //Função que coordena o movimento
    void Move()
    {
        //Pega os inputs do jogador de -1 a 1
        moveEmX = Input.GetAxisRaw("Horizontal");
        moveEmY = Input.GetAxisRaw("Vertical");
        //Caso o jogador esteja apertando o botão de corrida ele corre baseado na velocidade de corrida
        if (Input.GetKey(KeyCode.LeftShift))
        {
            velHorizontal = corridaHorizontal;
            velVertical = corridaVertical;
        }
        //Caso não esteja com o botão pressionado, volta a velocidade anterior
        else
        {
            velHorizontal = velBaseHorizontal;
            velVertical = velBaseVertical;
        }
        //Se move usando fisica e baseado na velocidade e input
        rigid.velocity = new Vector2(moveEmX * velHorizontal, moveEmY * velHorizontal);
    }


    //TODO: Deixar mais generico para servir com qualquer interação (Tá meio que feito já, mas vai q)
    void ChecaItem()
    {
        //Checa se tem algo interagivel ao redor com base na posição do jogador, um layer de coisas interagiveis e um raio
        temAlgoProximo = Physics2D.OverlapCircle(transform.position, circuloDeChecagem, interagiveis);
        //Caso tenha algo proximo nessas condições
        if (temAlgoProximo)
        {
            //Armazena o gameobject do que está próximo
            itemProximo = temAlgoProximo.gameObject;
            //Armazena também o Id do que está próximo
            idControladorTexto = itemProximo.GetComponent<IdItem>();
            controladorTexto.item = idControladorTexto;
            //Se tem alguma coisa interagivel e na distancia  aparece o texto do TextMeshPro
            if (itemProximo != null && Vector2.Distance(gameObject.transform.position, itemProximo.transform.position) < checagemDeDistancia)
            {
                apertarE.SetActive(true);
                //Se apertou o "E" e tem um script de dialogo inicia o dialogo
                if (Input.GetKeyDown(KeyCode.E) && idControladorTexto != null && !conversando)
                {
                    conversando = true;
                    controladorTexto.AtivaPainel();
                    apertarE.SetActive(false);
                }
                /*
                //Seguindo nas falas caso esteja conversando, se tiver um puzzle já ativa
                if (Input.GetKeyDown(KeyCode.Mouse0) && conversando)
                {
                    controladorTexto.ProximaFala();
                }
                */
            }
            //Caso não tenha nada interagivel proximo
            else
            {
                //Desativa o texto de interação
                apertarE.SetActive(false);
                //Permite ao player se mexer
                conversando = false;
            }
        }
    }

    //Checa o tamanho do circulo sendo desenhado
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, circuloDeChecagem);
    }*/

    //TODO: Deixar o texto de interação mais dinamico, mudando o que aparece na tela de acordo com o objeto (Necessário mudar o script ID)

}