using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//Biblioteca do TextMeshProa
using TMPro;

//TODO: Achar fonte em pixel art que tenha caracteres especiais e exclamações.

public class TextoParaJSON : MonoBehaviour
{
    //Obs: Usando o TextMeshPro no lugar do texto próprio da Unity por questões de flexibilidade e nitidez do texto

    //O IdItem do objeto que está selecionado, parte do objeto que guarda certas informações
    //Esse item é pego dinamicamente no script PlayerControl
    public IdItem item;
    //Vai receber as falas que estão dentro do arquivo json referenciado no "item"
    public Falas falas;
    //Texto com o nome de quem fala do lado esquerdo (TextMeshPro Text)
    public TextMeshProUGUI nomeChar;
    //Texto com o nome de quem fala do lado direito (TextMeshPro Text)
    public TextMeshProUGUI nomeCharRight;
    //Fala do personagem do lado esquerdo (TextMeshPro Text)
    public TextMeshProUGUI falaChar;
    //Fala do personagem do lado direito (TextMeshPro Text)
    public TextMeshProUGUI falaCharRight;
    //O retrato do personagem que esta falando a esquerda (Image)
    public Image portraitChar;
    //O retrato do personagem que esta falando a direita (Image)
    public Image portraitCharRight;
    //O Panel que é pai de todos os objetos REFERENTES A FALA (Excluindo o pai geral e as opções)
    public GameObject painelPrincipal;
    //O Objeto que age como pai do retrato do personagem e fundo do retrato, a esquerda (Image)
    public GameObject portraitObject;
    //O Objeto que age como pai do retrato do personagem e fundo do retrato, a direita (Image)
    public GameObject portraitObjectRight;
    //Imagem aonde o nome do personagem que fala vai ser escrito, a esquerda (Image)
    public GameObject nomeCharObject;
    //Imagem aonde o nome do personagem que fala vai ser escrito, a direita (Image)
    public GameObject nomeCharObjectRight;
    //Cor RGB que vai definir a cor do texto de fala de acordo com a cor HEX no JSON
    public Color minhaCor;
    //audiosource que vai servir para reproduzir o som das falas
    public AudioSource audioSource;
    //Clip que será reproduzido quando a fala estiver sendo escrita
    public AudioClip somzinho;
    //Lista com os botões de escolhas
    public List<Button> choicesButton;
    //Lista com os textos que serão escritos nos botões de escolhas
    public List<TextMeshProUGUI> choicesText;
    //Lista que ira armazenar as escolhas possiveis de serem feitas durante o dialogo
    public List<Dialog> choicesReference;
    //Referencia ao jogador
    [Tooltip("Mover o player EM CENA para aqui")]
    public PlayerControl player;

    //Contador que auxilia em funções de escolhas
    public int contadorEscolhas;
    //Referencia para a próxima fala na lista de falas 
    public int numProx;
    //Gerencia que fala da lista de falas será exibida
    public int contadorDialogues;
    //Bool que checa se o jogador acabou de escolher uma opção de dialogo
    bool justChose;
    //Bool que checa se o jogador esta escolhendo uma opção de dialogo
    bool escolhendo;
    //Referencia a PASTA com os arquivos JSON
    string path;
    //Referencia ao ARQUIVO JSON
    string newPath;
    //Garante que o texto apareceu por completo
    bool linhaConcluida = false;

    //Garante que só existe uma instancia de TextoParaJSON existe
    //É usada também para chamada de funções desse script em outros scripts
    public static TextoParaJSON singleton;

    //Inicialização do singleton
    private void Awake()
    {
        //Se o singleton não está preenchido, preenche ele com esse objeto
        if (singleton == null)
        {
            singleton = this;
        }
        //Se o singleton já existe, destroy o objeto para não haver co´pias do mesmo objeto
        else if (singleton != this)
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //Armazena na variavel quem é o AudioSource
        audioSource = GetComponent<AudioSource>();
        print("Checa essa pasta aqui: " + Application.persistentDataPath);
    }

    //TODO: Fazer o path funcionar na build
    public void AtivaPainel()
    {
        //Referencia o nome e caminho onde esta o arquivo JSON
        newPath = "Json/" + item.nomeArquivo;
        //Passa o texto dentro do arquivo para uma string
        Debug.Log(newPath);
        string jsonString = Resources.Load<TextAsset>(newPath).text;
        print("Leu de: " + newPath);
        //converte o arquivo JSON em um objeto no qual o JSON é baseado
        falas = JsonUtility.FromJson<Falas>(jsonString);
        //Caso não seja a primeira vez que as falas desse objeto estejam sendo vistas

        if (item.retorno)
        {  
            //Contador de dialogos irá começar a partir do ponto de retorno e não do ponto zero
            contadorDialogues = item.returnSpeech;
            //Checa de que lado a fala deve aparecer
            ChecaLado();
            Debug.Log("Retorno");
        }
        else if (item.retornoPuzzle)
        {
            contadorDialogues = item.returnFromPuzzle;
            //Checa de que lado a fala deve aparecer
            ChecaLado();
        }
        else
        {
            //Contador de dialogos começa a contar as falas do ponto zero
            contadorDialogues = item.startAt;
            //Checa de que lado a fala deve aparecer
            ChecaLado();
        }

        //Se for para falar ser exibida do lado direito
        if (falas.dialogues[contadorDialogues].changeSide)
        {
            CarregaDireita();
            numProx = falas.dialogues[contadorDialogues].numProx;
            //Não dá pra ser no JSon sem fazer uma função inutil
            somzinho = item.somzinho;
        }
        else
        {
            CarregaEsquerda();
            numProx = falas.dialogues[contadorDialogues].numProx;
            //Não dá pra ser no JSon sem fazer uma função inutil
            somzinho = item.somzinho;
        }
    }

    //Função que checa pra que lado o player esta olhando e muda a propriedade changeside da fala
    //Essa função permite deixar o changeside vazio no JSON e faz com que as mudanças de lado sejam automaticas
    public void ChecaLado()
    {
        //Se o player esta olhando para a direita, então sua fala aparece no lado esquerdo da tela
        if(falas.dialogues[contadorDialogues].nomePersonagem == "Jamal" && player.flip == false)
        {
            falas.dialogues[contadorDialogues].changeSide = false;
        }
        //Se o player esta olhando para a esquerda, então sua fala aparece no lado direito da tela
        else if (falas.dialogues[contadorDialogues].nomePersonagem == "Jamal" && player.flip == true)
        {
            falas.dialogues[contadorDialogues].changeSide = true;
        }
        //Se o player esta olhando para a esquerda, então a fala DO NPC aparece no lado direito da tela
        else if (player.flip == true)
        {
            falas.dialogues[contadorDialogues].changeSide = false;
        }
        //Se o player esta olhando para a direita, então a fala DO NPC aparece no lado esquerdo da tela
        else
        {
            falas.dialogues[contadorDialogues].changeSide = true;
        }
    }

    void Update()
    {
        //Caso a tecla designada seja apertada
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Se não estiver escolhendo uma escolha de dialogo, executa a função ProximaFala
            if (!escolhendo)
            {
                ProximaFala();
            }
        }
    }

    //Função que gerencia a passagem de falas no dialogo
    public void ProximaFala()
    {
        //Se o painel principal NÃO está ativo, NÃO executa a função
        if (!painelPrincipal.activeInHierarchy)
        {
            return;
        }

        //Caso, por algum acaso da natureza, o texto ainda esteja rodando, para ele
        StopCoroutine("AnimateText");

        //Prevenção de erro, após escolher uma opção a fala não estava seguindo corretamente, então
        //foi criado esse caso para poder lidar com uma situação de pós escolha 

        //Se a ultima ação do jogador foi escolher uma opção, entra nesse if
        if (justChose)
        {
            ChecaLado();
            //Primeiramente checa em que lado a fala deve aparecer
            if (falas.dialogues[contadorDialogues].changeSide)
            {
                CarregaDireita();
                linhaConcluida = false;
            }
            else
            {
                CarregaEsquerda();
                linhaConcluida = false;
            }
            //Torna o bool falso para que não entre novamente nesse if
            justChose = false;
            //Sai da função como um todo, para que o jogador tenha que apertar o botão novamente para passar de fala
            return;
        }

        //Caso seja apertado o botão de continuar a fala, antes da fala acabar
        if (!linhaConcluida)
        {
            //Checa se o texto deve aparecer no lado esquerdo ou direito
            if (falas.dialogues[contadorDialogues].changeSide == true)
            {
                //Faz o texto aparecer de uma vez só
                falaCharRight.text = falas.dialogues[contadorDialogues].falaPersonagem;
            }
            else
            {
                //Faz o texto aparecer de uma vez só
                falaChar.text = falas.dialogues[contadorDialogues].falaPersonagem;
            }
            //Muda o bool para que não se entre mais nessa função
            linhaConcluida = true;
            //Sai da função como um todo, para que o jogador tenha que apertar o botão novamente para passar de fala
            return;
        }


        //Se o objeto possui um puzzle relacionado a ele E o puzzle deve aparecer no numero da proxima fala
        if (item.puzzle && item.puzzleIn == falas.dialogues[contadorDialogues].numProx)
        {
            DesativaPainel();
            AtivaPuzzle();
            return;
        }

        //Caso a fala atual seja uma fala final
        if (falas.dialogues[contadorDialogues].type == "END")
        {
            DesativaPainel();
            Debug.Log("É final");
            return;
        }

        //Checa se é a ultima fala da lista (prevenção de erros)
        if (numProx >= falas.dialogues.Count)
        {
            DesativaPainel();
            return;
        }

        //Após essa fala, volta para a fala que originou as INFOs
        if (falas.dialogues[contadorDialogues].type == "RETURNINFO")
        {
            numProx = falas.dialogues[contadorDialogues].choiceDialog;
        }


        //Chama a função que lida com as escolhas
        ChoicesManager();

        if (escolhendo)
        {
            falas.dialogues[contadorDialogues].changeSide = false;
            falaChar.text = "";
            contadorDialogues = 0;
            numProx = 1;
            Debug.Log("Resetei aqui. numProx: " + numProx + " contadorDialogues: " + contadorDialogues);
        }

        //Caso a fala atual NÃO seja uma escolha (a função que lida com as escolha faz a mesma checagem internamente)
        if (falas.dialogues[contadorDialogues].type != "CHOICE" && falas.dialogues[contadorDialogues].type != "INFO" && !escolhendo)
        {
            //Invoca a função que checa se a próxima fala/escolha/info pode ou não aparecer (baseado na lista de escolhas)
            PodeSerChamado();
        }

        //Checa de que lado a fala será colocada
        ChecaLado();

        if (falas.dialogues[contadorDialogues].changeSide)
        {
            CarregaDireita();
            linhaConcluida = false;
        }
        else
        {
            CarregaEsquerda();
            linhaConcluida = false;
        }

        //Obs: O numProx é atualizado na função PodeSerChamado, não tendo assim a necessidade de ser chamado aqui
    }

    //Função que gerencia a condição de chamada de todas as falas
    public void PodeSerChamado()
    {
        Debug.Log("Entrou no PodeSerChamado");

        //Se o número do próximo dialogo for maior que o numero total de falas, sai da função
        //Acontece quando não há condição para chamar nenhum dos próximos dialogos
        if (numProx >= falas.dialogues.Count)
        {
            Debug.Log("Passou do limite. Numprox: " + numProx + "ContadorDialogues: " + contadorDialogues);
            //Sai da função
            return;
        }
        //Caso a próxima fala não tenha nenhuma condição de chamada
        if (falas.dialogues[numProx].condicaoChamada.Count == 0)
        {
            //Passa para a próxima fala, atualizando o contadorDialogues
            contadorDialogues = numProx;
            //Atualiza o numero da proxima fala
            numProx = falas.dialogues[numProx].numProx;
            //Sai da função
            Debug.Log("Sem condição de chamada. Numprox: " + numProx + "ContadorDialogues: " + contadorDialogues);
            return;
        }
        //Checa cada condição de chamada da fala e vê se a proxima fala pode ser chamada
        foreach (string condicao in falas.dialogues[numProx].condicaoChamada)
        {
            //Se dentro da lista de escolhas feitas NÃO existe a condição de chamada necessária para a fala 
            //que esta sendo avaliada ser chamada
            if (!Indispensaveis.escolhasFeitas.Contains(condicao))
            {
                //Não passa pro próximo, passa para o próximo do próximo
                //Vai fazer isso até chegar em um ponto em que a fala pode ser chamada ou ao final da lista de falas
                numProx = falas.dialogues[numProx].numProx;
                //Chama novamente a função para que faça novamente as checagens
                PodeSerChamado();
                //Sai da função
                Debug.Log("Entrou no if. Numprox: " + numProx + "ContadorDialogues: " + contadorDialogues);
                return;
            }
        }
        //Se não entrou em nenhum dos if, significa que possui a condição de chamada
        //Passa para a próxima fala
        contadorDialogues = numProx;
        //Atualiza o número que corresponde a próxima fala
        numProx = falas.dialogues[numProx].numProx;
        //Sai da função
        //Esse retorno é necessário principalmente pelo fato dessa função ser recursiva
        Debug.Log("Tinha condição. Numprox: " + numProx + "ContadorDialogues: " + contadorDialogues);
        return;
    }

    //Função que lida com as escolhas
    public void ChoicesManager()
    {
        Debug.Log("Entrou no ChoicesManager");
        //Checa se a proxima fala é uma escolha
        if (falas.dialogues[numProx].type == "CHOICE" || falas.dialogues[numProx].type == "INFO")
        {
            //Chama a função que confere se existe condição necessária para chamada da proxima fala/escolha/info
            PodeSerChamado();
            //Checa se a fala atual é uma escolha
            if (falas.dialogues[contadorDialogues].type == "CHOICE" || falas.dialogues[contadorDialogues].type == "INFO")
            {
                //Ativa o botão na cena
                choicesButton[contadorEscolhas].gameObject.SetActive(true);
                //Passa o texto da escolha pra dentro do texto do botão
                choicesText[contadorEscolhas].text = falas.dialogues[contadorDialogues].falaPersonagem;
                //Incrementa o contador de escolhas que é usado para checar quantas escolhas são possiveis de ser feitas
                contadorEscolhas++;
                //Coloca a escolha na lista de escolhas que estão aparecendo atualmente
                choicesReference.Add(falas.dialogues[contadorDialogues]);
                //Deixa verdadeiro o bool que checa se o jogador ainda está escolhendo uma opção
                escolhendo = true;
                //Chama a função de novo pra checar se o próximo é uma escolha válida e assim por diante
                ChoicesManager();
                //Sai da função
                return;
            }
        }
        //Se o próximo não é uma escolha, zera o contador de escolhas e sai da função
        contadorEscolhas = 0;
        return;
    }

    public void InfoManager()
    {
        Debug.Log("Entrou no InfoManager");
        //Checa se a proxima fala é uma escolha
        if (falas.dialogues[numProx].type == "INFO")
        {
            //Chama a função que confere se existe condição necessária para chamada da proxima fala/escolha/info
            PodeSerChamado();
            //Checa se a fala atual é uma informação
            if (falas.dialogues[contadorDialogues].type == "INFO")
            {
                //Ativa o botão na cena
                choicesButton[contadorEscolhas].gameObject.SetActive(true);
                //Passa o texto da escolha pra dentro do texto do botão
                choicesText[contadorEscolhas].text = falas.dialogues[contadorDialogues].falaPersonagem;
                //Incrementa o contador de escolhas que é usado para checar quantas escolhas são possiveis de ser feitas
                contadorEscolhas++;
                //Coloca a escolha na lista de escolhas que estão aparecendo atualmente
                choicesReference.Add(falas.dialogues[contadorDialogues]);
                //Deixa verdadeiro o bool que checa se o jogador ainda está escolhendo uma opção
                escolhendo = true;
                //Chama a função de novo pra checar se o próximo é uma escolha válida e assim por diante
                InfoManager();
                //Sai da função
                return;
            }
        }
        //Se o próximo não é uma escolha, zera o contador de escolhas e sai da função
        contadorEscolhas = 0;
        return;
    }

    //Função que lida com as escolhas nos botões
    /// <summary>
    /// Na função ChoicesManager as falas equivalentes a escolhas foram passadas para uma lista chamada
    /// choicesReference. A função aqui explicada atribui um valor de 0 a X de acordo com o indice da escolha armazenado
    /// na lista choicesReference. De acordo com a opção escolhida, seu valor é repassado para a função e a partir disso
    /// a função retorna a fala seguinte de acordo
    /// </summary>
    /// <param name="valor">O valor que vai equivaler a escolha que será feita</param>
    public void Chose(int valor)
    {
        //Passa o código da escolha feita para a lista de escolhas
        Indispensaveis.escolhasFeitas.Add(choicesReference[valor].codigoEscolha);
        //Desativa os botões de escolha
        foreach (var item in choicesButton)
        {
            item.gameObject.SetActive(false);
        }
        //Pega o número de referencia para a fala que vai vir após a escolha
        int dialogoEscolhido = choicesReference[valor].choiceDialog;
        Debug.Log("dialogoEscolhido: " + dialogoEscolhido);
        //Atualiza o numProx
        numProx = falas.dialogues[dialogoEscolhido].numProx;
        //Atualiza o contadorDialogues
        contadorDialogues = falas.dialogues[dialogoEscolhido].numTexto;
        Debug.Log("numProx foi mudado em Chose para: " + numProx);
        //Segue para o texto seguinte a opção selecionada
        //Limpa a lista para que ela possa ser refeita quando as próximas escolhas aparecerem
        choicesReference.Clear();
        //Deixa o justChose como verdadeiro para se enquadrar na opção de passagem correta na função ProximaFala
        justChose = true;
        //Deixa o escolhendo como falso para que a tecla de seguir com o texto não seja mais bloqueada
        escolhendo = false;
        //Chama a função ProximaFala
        ProximaFala();
    }

    //Desativa o painel geral de falas
    public void DesativaPainel()
    {
        //Desativa o painel principal (O panel geral)
        painelPrincipal.SetActive(false);
        //Desativa a checagem de conversa, permitindo o player a voltar a andar
        player.conversando = false;
        //Zera o contador de escolhas
        contadorEscolhas = 0;
        //Zera o numprox
        numProx = 0;
        if (!item.retornoPuzzle)
        {
            //Marca como verdadeiro o bool que indica que as falas desse objeto já foram vistas
            item.retorno = true;
        } 
        //Diz que a fala não foi concluida (pq não há mais fala passando)
        linhaConcluida = false;
    }

    //Caso haja um puzzle, ativa ele para o jogador jogar
    public void AtivaPuzzle()
    {
        //Essa variavel vai fazer o player parar de se move
        player.conversando = true;
        //Ativa o canvas (prefabzin) do puzzle
        item.canvasPuzzle.SetActive(true);
    }

    //Desativa o puzzle, fechando o canvas e fazendo o jogador voltar a se mexer
    //Vai ser chamado pelo sigleton
    public void DesativaPuzzle()
    {
        item.retornoPuzzle = true;
        item.retorno = false;
        //Desativa o canvas do puzzle
        item.canvasPuzzle.SetActive(false);
        //Diz que esse objeto não possui mais um puzzle
        item.puzzle = false;
        //Permite que o jogador volte a se mexer
        player.conversando = false;
        //Reseta o contador de escolhas
        contadorEscolhas = 0;
        //Reseta o numprox
        numProx = 0;
        //Diz que a fala não foi concluida (pq não há mais fala passando)
        linhaConcluida = false;
    }

    //Fecha o puzzle, fazendo ele voltar ao inicio
    //Dentro de cada puzzle essa função será chamada após resetar o puzzle para o estado inicial (não mexido pelo jogador)
    public void PausaPuzzle()
    {
        item.retorno = true;
        //Desativa o canvas do puzzle
        item.canvasPuzzle.SetActive(false);
        //Mantem o bool de que há um puzzle a ser feito ativo
        item.puzzle = true;
        //Faz com que o jogador possa voltar a se mover
        player.conversando = false;
        //Reseta o contador de escolhas do objeto
        contadorEscolhas = 0;
        //Reseta o numProx do objeto
        numProx = 0;
        //Seta o bool de texto concluido para falso
        linhaConcluida = false;
    }

    //Muda a cor do texto de acordo com o especificado no arquivo JSON
    //Dentro do IEnumerator AnimateText há um complemento a essa função que permite que certas palavras tenham cores
    //diferentes do texto geral. "Checar AnimateText"
    public void MudaCor()
    {
        //Função que converte uma cor HEX em RGB
        ColorUtility.TryParseHtmlString(falas.dialogues[contadorDialogues].corTexto, out minhaCor);
        //Prevenção de erros
        //Garante que o alpha da cor está no máximo, não deixando que ela fique invisivel ou transparente demais
        minhaCor.a = 255;
        //Passa a cor já transformada em RGB para o texto, tanto na direita quanto na esquerda
        falaChar.color = minhaCor;
        falaCharRight.color = minhaCor;
    }

    //Função que busca e retorna uma imagem de acordo com o nome especificado
    public Sprite CarregaImagem(string spriteName)
    {
        foreach (Sprite s in item.retratoPersonagem)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }

    //Faz todo o processo de carregar e atualizar a caixa de dialogo do lado esquerdo
    //Obs:Não pode (ao meu ver) ser dividido em uma função de ativação e uma de atualização, pois a troca constante
    //de dialogo faz com que seja necessária a ativação e desativação de objetos constantemente
    public void CarregaEsquerda()
    {
        //Desativa os objetos da caixa de dialogo que ficam a direita
        portraitObjectRight.SetActive(false);
        nomeCharObjectRight.SetActive(false);
        //Ativa os objetos da caixa de dialogo que ficam a esquerda
        portraitObject.SetActive(true);
        nomeCharObject.SetActive(true);
        //Passa o nome de quem está falando agora
        nomeChar.text = falas.dialogues[contadorDialogues].nomePersonagem;
        //Passa a fala de acordo com o índice em que ela está agora
        //Apenas caso ele não esteja no meio de uma escolha
        if (!escolhendo)
        {
            falaChar.text = falas.dialogues[contadorDialogues].falaPersonagem;
        }
        //Passa o retrato de acordo com o indice em que ele está agora
        //Checar a função "CarregaImagem"
        portraitChar.sprite = CarregaImagem(falas.dialogues[contadorDialogues].nomePortrait);
        //Função que muda a cor do texto para a cor especificada dentro do JSON
        MudaCor();
        //Ativa o painel principal (o panel que é pai de quase todos os objetos da caixa de dialogo)
        painelPrincipal.SetActive(true);
        //Passa o texto letra a letra de acordo com os parametros especificados no JSON
        //Checar AnimateText
        StartCoroutine("AnimateText", falaChar.text);
    }

    //Faz todo o processo de carregar e atualizar a caixa de dialogo do lado edireito
    //Obs:Não pode (ao meu ver) ser dividido em uma função de ativação e uma de atualização, pois a troca constante
    //de dialogo faz com que seja necessária a ativação e desativação de objetos constantemente
    public void CarregaDireita()
    {
        //Ativa os objetos da caixa de dialogo que ficam a direita
        portraitObjectRight.SetActive(true);
        nomeCharObjectRight.SetActive(true);
        //Desativa os objetos da caixa de dialogo que ficam a esquerda
        portraitObject.SetActive(false);
        nomeCharObject.SetActive(false);
        //Passa o nome de quem está falando agora
        nomeCharRight.text = falas.dialogues[contadorDialogues].nomePersonagem;
        //Passa a fala de acordo com o índice em que ela está agora
        //Apenas caso ele não esteja no meio de uma escolha
        if (!escolhendo)
        {
            falaCharRight.text = falas.dialogues[contadorDialogues].falaPersonagem;
        }
        //Passa o retrato de acordo com o indice em que ele está agora
        //Tem que estar com a escala x em -1 se for do lado direito
        //Checar a função "CarregaImagem"
        portraitCharRight.sprite = CarregaImagem(falas.dialogues[contadorDialogues].nomePortrait);
        //Função que muda a cor do texto para a cor especificada dentro do JSON
        MudaCor();
        //Ativa o painel principal (o panel que é pai de quase todos os objetos da caixa de dialogo)
        painelPrincipal.SetActive(true);
        //Passa o texto letra a letra de acordo com os parametros especificados no JSON
        //Checar AnimateText
        StartCoroutine("AnimateText", falaCharRight.text);
    }

    //"Metódo" que faz letras serem passadas uma a uma de acordo com a velocidade definida no JSON
    IEnumerator AnimateText(string dialogueText)
    {
        //Zera o texto antes de iniciar o letra a letra (tanto a direita quanto a esquerda)
        falaChar.text = "";
        falaCharRight.text = "";
        //Armazena o total de letras que tem na frase
        int lenghtFala = dialogueText.Length;
        //Inicia um contador pra checagem posterior
        int contadordeletras = 0;

        //TODO: Adicionar um som que varia de velocidade de acordo com a velocidade do texto e não toque quando aparecem espaços
        //Inicializa uma string vazia que será usada para armazenar certas palavras
        string colorida = "";
        //Inicia um contador que será usado para checar quando uma tag foi fechada
        int fechaTag = 0;
        //Bool que indica
        bool preencheColorida = false;
        //Passa as letras de uma a uma
        foreach (char letter in dialogueText)
        {
            //Se tem um abre tag significa que vai haver uma mudança de cor nessa palavrar
            //portanto deixa o bool preencheColorida como verdadeiro
            if (letter == '<')
            {
                preencheColorida = true;
            }
            //Se encontrou um fecha tag aumenta o numero no contador fechaTag
            if (letter == '>')
            {
                fechaTag++;
            }
            //Caso preencheColorida seja verdadeiro, em vez de as letras irem para a caixa de texto, elas vão para
            //a string colorida
            if (preencheColorida)
            {
                colorida += letter;
            }
            //Caso preencheColorida seja falso, segue normalmente colocando as letras uma por uma na caixa de dialogo
            else
            {
                if (falas.dialogues[contadorDialogues].changeSide)
                {
                    falaCharRight.text += letter;
                }
                else
                {
                    //Passa as letras de uma a uma
                    falaChar.text += letter;
                }
            }
            //Se o contador está em 2 ou mais, significa que todas as tags abertas foram fechadas
            //Ver o arquivo JSON para entender melhor
            if (fechaTag >= 2)
            {
                //Reseta o contador, para que ele possa ser usado novamente
                fechaTag = 0;
                //Seta o bool como falso para que ele pare de entrar no if relacionado
                preencheColorida = false;
                //Passa a palavra com a cor diferenciada para a caixa de dialogo de uma vez só
                if (falas.dialogues[contadorDialogues].changeSide)
                {
                    falaCharRight.text += colorida;
                }
                else
                {
                    falaChar.text += colorida;
                }
            }
            //Se a letra a ser passada não for um espaço vazio, toca o somzinho
            if (letter != ' ')
            {
                audioSource.Stop();
                audioSource.PlayOneShot(somzinho);
            }
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
            if (!preencheColorida)
            {
                yield return new WaitForSeconds(falas.dialogues[contadorDialogues].velocidadeTexto);
            }
        }
    }
}