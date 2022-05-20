using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{

    //TODO: Otimização de chamadas

    // Coisas que precisam ser configuradas no Inspector para que funcione
    // 1. A quantidade de peças que irão ser interagiveis no pecasFotos - Size
    // 2. Colocar essas imagens na lista pecasFotos
    // 3. Colocar na variável PlayerCursor, quem será o cursor que o player irá controlar.
    // 4. Colocar na variável Panel Puzzle, o Panel que irá cobrir todas as fotos (pode ser o canvas inteiro ou o Maior panel)
    // 5. Na variável Pack, colocar o GameObject que serve como Pai / Container de todos as imagens recortadas.
    // 6. Olhar o script PosicaoImagem para mais informaçõe 

    //Variáveis

    //Lista que irá salvar os GameObjects referente as imagens "únicas". Lembrar que devem ser 
    //adicionadas MANUALMENTE no inspector, tanto a quantidade como os Objetos que irão ser manipulados														
    public List<GameObject> pecasFotos = new List<GameObject>();

    public GameObject playerCursor;          //GameObject referente ao Cursor que o player irá controlar.
    public GameObject panelPuzzle;          //Panel PRINCIPAL no qual o cursor e as peças da foto irão ficar.
    public GameObject pecaCarregada;       //Saber qual peça está sendo "Carregada" pelo cursor.
    public GameObject pack;               //GameObject PAI das imagens cortadas. Esse GameObjct contém TODAS as imagens que compõe a foto, como filhas.
    [Tooltip("Só arrasta o canvas prak")]
    public GameObject canvas;            //Gameobject do Canvas em que esse puzzle se encontra
    public AudioSource sourceAudio;     //AudioSource que vai tocar os sonzinhos

    //Só pra destravar esse pau no cu e ele poder andar de novo, mas eu vou muito mudar essa merda, vou sim
    //ATUALIZADO EM 10/05/2018: Vou não
    public PlayerControl player;

    public bool holding;        //Booleana para saber se o jogador está, ou não, segurando alguma foto.
    public bool fimPuzzle;      //Booleana criada para informar se o jogo acabou ou não. Por enquanto, ainda não é utilizada diretamente.

    public int distanceGap;     //Aqui será guardado a quantidade de unidades de distância entre uma imagem recortada e outra após sua movimentação. Futuramente pode acabar mudando.

    Vector2 posCursorAntesMexer;    //Responsável por salvar a posição do Cursor ANTES de o mesmo realizar o movimento 
    Vector2 posCursorPosMexer;      //Irá receber a posição do cursor APOS o seu movimento.
    Vector2 panelPrincipalSize;     //Irá guardar o tamanho do Panel no qual o jogo irá se passar, sua Largura e sua altura.
    Vector2 posInicialCursor;       //Irá guardar a posição inicial do cursor

    bool moveu;                 //Booleana para saber se o jogador já fez um movimento. Criada para impedir que o movimento saia mais de uma vez.

    float InputX;               // Computar o Input do jogador na horizontal
    float InputY;               // e na vertical.


    void Start()
    {
        //Armazenando o tamanho atual do panel. É feito isso pois, além de ser referenciado na movimentação do cursor, 
        //o panel ou o canvas podem escalar em resoluções diferentes.
        panelPrincipalSize = panelPuzzle.GetComponent<RectTransform>().sizeDelta;
        //A distância entre as peças, POR ENQUANTO, é de 10, como não tenho outras resoluções para testar, por enquanto fica assim.
        distanceGap = 10;
        //Guarda a posição inicial do cursor
        posInicialCursor = playerCursor.GetComponent<RectTransform>().localPosition;
        //Player arrombado, to botando essa var aqui com ódio
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        //Recebendo os inputs
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");

        Movimento();       // Método responsável pela movimentação do cursor.
        PegarPeca();       // Método responsável por pegar / soltar uma peça
        Resetar();
    }

    void Movimento()
    {
        if (moveu)
        {
            // Se o jogador realizou movimento, para ele não sair indo na direção do movimento, 
            //resolvi parar o movimento até o jogador parar com o inputs de movimento.
            if (InputX == 0 && InputY == 0)
            {
                moveu = false;
            }
        }

        else
        {
            //Caso o jogador não tenha realizado o movimento e insira um input:
            if (InputX != 0 || InputY != 0)
            {
                //A posição antes de ele se mexer será salva
                posCursorAntesMexer = playerCursor.GetComponent<RectTransform>().localPosition;

                // O cursor que o jogador controla irá para a direção desejada pegando o tamanho do próprio cursor e 
                //indo para uma das direções, SEMPRE lembrando do gap 

                playerCursor.GetComponent<RectTransform>().localPosition += new Vector3(
                (playerCursor.GetComponent<RectTransform>().sizeDelta.x + distanceGap) * InputX,
                (playerCursor.GetComponent<RectTransform>().sizeDelta.y + distanceGap) * InputY,
                                                                                          0);
                //OBS: O tamanho do cursor DEVE ser igual ao tamanho das imagens recortadas
                //Após feito a "movimentação", a posição do cursor é salva novamente.
                posCursorPosMexer = playerCursor.GetComponent<RectTransform>().localPosition;

                //CASO o cursor saia dos LIMITES do PANEL (Lembre-se de CENTRALIZAR OS ANCHORS NO CENTRO DO CANVAS), 
                //ele irá voltar para a posição ANTES da movimentação, dando a impressão de que o movimento não aconteceu
                if ((posCursorPosMexer.x < -panelPrincipalSize.x / 2 || posCursorPosMexer.x > panelPrincipalSize.x / 2
                    || posCursorPosMexer.y > panelPrincipalSize.y / 2 || posCursorPosMexer.y < -panelPrincipalSize.y / 2))
                    playerCursor.GetComponent<RectTransform>().localPosition = posCursorAntesMexer;

                // Se o cursor estiver segurando uma imagem, a imagem irá para a mesma posição do cursor.
                if (holding)
                {
                    pecaCarregada.GetComponent<RectTransform>().localPosition = playerCursor.GetComponent<RectTransform>().localPosition;
                }
                moveu = true;   // Após isso, será informado que o Cursor fez o movimento, assim, não podendo se mexer até ele ser falso
            }
        }
    }

    void PegarPeca()
    {
        if (Input.GetButtonDown("Fire1") && !holding)
        {
            // Para tentar pegar uma peça, o jogador deve Apertar o Fire1 e não estar segurando nada.
            for (int i = 0; i < pecasFotos.Count; i++)
            {
                // Assim, é feita uma verificação, entre TODAS as imagens, qual está na MESMA posicao do cursor.
                if (pecasFotos[i].transform.localPosition == playerCursor.GetComponent<RectTransform>().localPosition)
                {   //Caso alguma esteja, irá ocorrer:
                    //1.Essa peça será a peça que poderá ser carregada.
                    pecaCarregada = pecasFotos[i].gameObject;
                    //2.Será informado que o jogador está segurando algo.
                    holding = true;
                    //3.Malaca(A peça carregada deixa de ser filha do Pack para ser filha do Panel, depois volta a ser filha do pack)
                    //Isso para fazer com que ele "Suba" e "Desça" na escala de Hierarquia do canvas de renderização. Quão mais baixo melhor
                    //Isso faz com que a peça que estou segurando sempre fique "por cima" das imagens que estão paradas.
                    pecaCarregada.transform.SetParent(panelPuzzle.transform);
                    pecaCarregada.transform.SetParent(pack.transform);
                    //4.Sair do laço logo depois disso
                    i = pecasFotos.Count;
                }
            }
        }

        else if (Input.GetButtonDown("Fire1") && holding)
        {
            // Caso esteja segurando, primeiro é verificado se:
            //1.Existe alguma peça na mesma posição da peça Carregada.
            for (int i = 0; i < pecasFotos.Count; i++)
            {
                //2.E se essa peça é diferente da peça carregada.
                if (pecasFotos[i].GetComponent<RectTransform>().localPosition ==
                    pecaCarregada.GetComponent<RectTransform>().localPosition && pecasFotos[i] != pecaCarregada)
                {
                    // Caso tenha alguma peça lá, é feito a saida do laço e do método
                    i = pecasFotos.Count;
                    return;
                }

                if (i == pecasFotos.Count - 1)
                {
                    // Caso não tenha nenhuma peça, antes de sair do laço, é informado que o jogador não está mais segurando nada.
                    //Assim a peçaCarregada não ira mais seguir o cursor (Linha 70/71)
                    holding = false;
                    //Além disso, o seguinte método é Invocado depois de 0.2s. Fiz isso pois, além de tira-lo do update, a verificação acontecia
                    //Antes de receber os verdadeiros valores das peças (Se estavam ok ou não).
                    Invoke("VerificadorPosicoes", 0.2f);
                }
            }
        }
    }

    //O método realiza uma verificação se todas as peças estão na posição correta.
    void VerificadorPosicoes()
    {
        //Primeiro, se houver outro invoke do mesmo método, paramos para ele ser executado apenas uma vez.
        CancelInvoke("VerificadorPosicoes");
        // Uma variável interna é criada para auxiliar na função.
        bool verifcadorFim = true;
        //O laço irá verificar se as imagens estão na posição correta e ligar com a variável local.
        for (int i = 0; i < pecasFotos.Count; i++)
        {
            //Se, no final do laço, o verificadorFim estiver True, logo todas as posições estão certas,
            //mas se algum estiver falso logo, verificadorFim será falso.
            verifcadorFim = pecasFotos[i].GetComponent<PosicaoImagem>().posOk && verifcadorFim;
            // Se verificadorFim for verdadeiro no fim, ele irá iniciar a finalização do puzzle
            if (i == pecasFotos.Count - 1)
            {
                if (verifcadorFim) FinalizarJogo();
            }
        }
    }

    //Método com algumas informações se o jogo for FINALIZADO
    void FinalizarJogo()
    {
        //Booleana informando que, de fato, o puzzle teve um fim
        fimPuzzle = true;
        //Pega o componente animator, que fica no GameObject q é PAI de todas as imagens e realiza a animação (a da imagens se unirem)
        pack.GetComponent<Animator>().enabled = true;
        //O Cursor é desativado.
        playerCursor.SetActive(false);
        //Toca o somzinho de puzlle concluido
        //TODO: Mudar para tocar vários audios diferentes
        sourceAudio.Play();
        //O Canvas é desativado após 2 segundos, o tempo pra animação tocar e mais um extrazinho
        //Usando o Invoke a partir do singleton do TextController por motivos de organização
        TextoParaJSON.singleton.Invoke("DesativaPuzzle", 2);
        //E esse script para de funcionar.
        gameObject.GetComponent<Cursor>().enabled = false;
    }

    //TODO: Melhorar a chamada da função resetar, está muito pesada
    void Resetar()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Volta todas as imagens para a posição inicial e reseta os bools delas
            for (int i = 0; i < pack.transform.childCount; i++)
            {
                pack.transform.GetChild(i).transform.localPosition = pack.transform.GetChild(i).GetComponent<PosicaoImagem>().posInicial;
            }
            playerCursor.GetComponent<RectTransform>().localPosition = posInicialCursor;
            TextoParaJSON.singleton.PausaPuzzle();
        }
    }

    //<------------------------------------------------------------------------------>


    //Coisas não necessárias (Mas não apaguei ainda, vai que)\/
    //  Variáveis que parei de utilizar
    //	public List<Vector2> canvasPos = new List<Vector2> (); 

    // </Variáveis que parei de utilizar>
    //
    //	// Esse método servia para mapear as posições possíveis no plane, mas acabou não endo útil.
    //	void ColetarPosicoesPossiveis(){
    //		for (int i = 0; i < 24; i++) {
    //			if (i >= 0 && i <= 5)
    //				canvasPos.Add (new Vector2 (MinPosX + (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.x + distanceGap) * i, MaxPosY));
    //			else if (i >= 6 && i <= 11)
    //				canvasPos.Add (new Vector2 (MinPosX + (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.x + distanceGap) * (i-6), MaxPosY - (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.y + distanceGap)));
    //			else if (i >= 12 && i <= 17)
    //				canvasPos.Add (new Vector2 (MinPosX + (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.x + distanceGap) * (i-12), MaxPosY - (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.y + distanceGap) * 2));
    //			else if (i >= 18 && i <= 24)
    //				canvasPos.Add (new Vector2 (MinPosX + (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.x + distanceGap) * (i-18), MaxPosY - (PlayerCursor.GetComponent<RectTransform> ().sizeDelta.y + distanceGap) * 3));
    //		}
    //	}
    //
    //	// Dado o mapeamento, esse método iria testar as posições utilizaveis.
    //	void TestePosicoes(){
    //		if (Input.GetButtonDown ("Fire1")) {
    //			PlayerCursor.GetComponent<RectTransform> ().localPosition = canvasPos [currentPossition];
    //			currentPossition++;
    //			if (currentPossition >= canvasPos.Count)
    //				currentPossition = 0;
    //		}
    //	}
    // Acho que pode apagar essa merda, mas deixei para tu vêr 
}
