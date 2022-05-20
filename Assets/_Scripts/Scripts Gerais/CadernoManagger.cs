using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CadernoManagger : MonoBehaviour
{

    //Obs: Os pais ficaram dentro das listas apenas para uma questão de contagem de páginas

    //Painel principal do caderno, o que deixa o caderno ativo ou não na tela
    public GameObject painelPrincipal;
    //Lista de botões das abas
    public List<Image> abas;
    //As imagens clicaveis dos personagens na aba de Profiles do caderno
    //Retirada momentaneamente
    //public List<Button> profilesButtons;
    //
    public List<GameObject> profilesListPagina;
    //As páginas dos profiles
    public List<GameObject> profilesPersonagens;
    //
    public List<GameObject> painelPistasBotoes;
    //As páginas que contém as pistas
    public List<GameObject> painelPistasPaginas;
    //
    public List<GameObject> diarioPagina;
    //
    public List<GameObject> investigacoesPaginas;
    //O pai de todos o paineis de personagem
    public GameObject painelPersonagens;
    //O pai de todos os profiles
    public GameObject PainelProfiles;
    //O pai de todo o diario
    public GameObject painelDiarioResumo;
    //O pai de todas as pistas
    public GameObject painelPistas;
    //O pai de todos os resumos de investigações
    public GameObject painelInvestigacoes;
    //
    public GameObject pistasPaginas;
    //
    public GameObject profilesPaginasPai;
    //
    public int totalPaginasProfiles;
    //
    public int totalPaginasPistas;
    //
    public int totalPaginasDiario;
    //
    public int totalPaginasInvestigacoes;
    //
    public int paginaAtual;
    //
    public TextMeshProUGUI numeroPaginaText;
    //
    public int indexProfiles;
    //
    public int indexResumos;
    //
    public int indexPistas;
    //
    public int indexInvestigacoes;
    //
    public int indexAbas;
    //
    public PlayerControl player;
    //
    public bool cadernoAtivo;

    //TODO: passar o texto como um arquivo


    public static CadernoManagger instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InicializaInteiros();
    }
    void Update()
    {

    }

    public void SelectProfile(int profileRef)
    {
        //Desativa todos os profiles
        foreach (var item in profilesPersonagens)
        {
            item.SetActive(false);
        }
        DesativaPaginas();
        PainelProfiles.SetActive(true);
        //Ativa o profile do personagem selecionado
        profilesPersonagens[profileRef].SetActive(true);
    }

    public void AtivaCaderno()
    {
        painelPrincipal.SetActive(true);
        cadernoAtivo = true;
        player.conversando = true;
        SelecionaAba();
    }

    public void DesativaCaderno()
    {
        painelPrincipal.SetActive(false);
        cadernoAtivo = false;
        player.conversando = false;
    }

    public void DesativaPaginas()
    {
        painelPersonagens.SetActive(false);
        PainelProfiles.SetActive(false);
        painelDiarioResumo.SetActive(false);
        painelPistas.SetActive(false);
        pistasPaginas.SetActive(false);
        painelInvestigacoes.SetActive(false);
    }

    public void AtualizarPagina(int index, int totalPaginas)
    {
        numeroPaginaText.text = (index + 1) + "/" + totalPaginas;
    }

    /// <summary>
    /// Passa a aba em sentido positivo e negativo. TRUE para positivo FALSE para negativo
    /// </summary>
    /// <param name="mais"></param>
    public void PassaAba(bool mais)
    {
        if (mais)
        {
            indexAbas++;
            if(indexAbas> abas.Count -1)
            {
                indexAbas = 0;
            }
        }
        else
        {
            indexAbas--;
            if(indexAbas < 0)
            {
                indexAbas = abas.Count - 1;
            }
        }
    }

    public void InicializaInteiros()
    {
        indexInvestigacoes = 0;
        indexPistas = 0;
        indexProfiles = 0;
        indexResumos = 0;
        indexAbas = 0;
        paginaAtual = 0;
        totalPaginasProfiles = profilesPersonagens.Count;
        totalPaginasPistas = painelPistasPaginas.Count;
        totalPaginasInvestigacoes = investigacoesPaginas.Count;
        totalPaginasDiario = diarioPagina.Count;
    }

    /// <summary>
    /// Passa a página para a página seguinte
    /// </summary>
    /// <param name="pai">O Gameobject pai desse bloco de páginas</param>
    /// <param name="index">Indice em que as páginas iram passar</param>
    /// <param name="paginas">Lista das páginas, cada página é um Gameobject</param>
    public void PassaPaginaPlus(GameObject pai, ref int index, List<GameObject> paginas)
    {
        DesativaPaginas();
        pai.SetActive(true);
        index++;
        if (index >= paginas.Count)
        {
            index = 0;
        }
        for (int i = 0; i < paginas.Count; i++)
        {
            paginas[i].SetActive(false);
            Debug.Log("For " + i);
        }
        paginas[index].SetActive(true);
    }

    public void PassaPagina(bool mais)
    {
        if(indexAbas == 0) //Diario/Resumo
        {
            DesativaPaginas();
            painelDiarioResumo.SetActive(true);
            //Atualiza o número de páginas
            AtualizarPagina(indexResumos, totalPaginasDiario);
            if (mais)
            {
                indexResumos++;
                if (indexResumos >= diarioPagina.Count)
                {
                    indexResumos = 0;
                }
                for (int i = 0; i < diarioPagina.Count; i++)
                {
                    diarioPagina[i].SetActive(false);
                }
                diarioPagina[indexResumos].SetActive(true);
            }
            else
            {
                indexResumos--;
                if (indexResumos <0)
                {
                    indexResumos = diarioPagina.Count -1;
                }
                for (int i = 0; i < diarioPagina.Count; i++)
                {
                    diarioPagina[i].SetActive(false);
                }
                diarioPagina[indexResumos].SetActive(true);
            }
        }

        else if (indexAbas == 1) //Pessoas
        {

        }
    }

    public void SelecionaAba()
    {
        //Reseta as scalas de todas as imagens
        foreach (var j in abas)
        {
            j.transform.localScale = new Vector3(1, 1, 1);
        }

        if (indexAbas == 0) //Diario/Resumo
        {
            //Highlight na aba que está selecionada
            //(Como que faz highlight? Será se eu mudo a imagem apenas?)
            //Aumenta o tamanho da imagem da aba
            abas[indexAbas].transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            //Faz com que essa imagem da aba apareça na frente das outras
            abas[indexAbas].transform.SetAsLastSibling();
            //Desativa todas as coisas não relativas a essa aba
            DesativaPaginas();
            //Ativa as coisas relativas a essa aba
            painelDiarioResumo.SetActive(true);
            //Atualiza o número de páginas
            AtualizarPagina(indexResumos, totalPaginasDiario);
            //Ativa no ponto em que o jogador parou da ultima vez
            diarioPagina[indexResumos].SetActive(true);
            //Sinaliza que essa é a aba ativada
            Debug.Log("Aba Diario/Resumo");
        }
        //Esse aqui vai ter que dar uma refeita
        else if (indexAbas == 1) //Pessoas
        {
            //Highlight na aba que está selecionada
            //(Como que faz highlight? Será se eu mudo a imagem apenas?)
            //Aumenta o tamanho da imagem da aba
            abas[indexAbas].transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            //Faz com que essa imagem da aba apareça na frente das outras
            abas[indexAbas].transform.SetAsLastSibling();
            //Desativa todas as coisas não relativas a essa aba
            DesativaPaginas();
            //Ativa as coisas relativas a essa aba
            PainelProfiles.SetActive(true);
            //Atualiza o número de páginas
            AtualizarPagina(1, 1); //Temporario
            //Ativa no ponto em que o jogador parou da ultima vez
            //diarioPagina[indexResumos].SetActive(true);
            //Sinaliza que essa é a aba ativada
            Debug.Log("Aba Personagens");
        }
        //Mesmo caso do anterior
        else if (indexAbas == 2) //Pistas
        {
            //Highlight na aba que está selecionada
            //(Como que faz highlight? Será se eu mudo a imagem apenas?)
            //Aumenta o tamanho da imagem da aba
            abas[indexAbas].transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            //Faz com que essa imagem da aba apareça na frente das outras
            abas[indexAbas].transform.SetAsLastSibling();
            //Desativa todas as coisas não relativas a essa aba
            DesativaPaginas();
            //Ativa as coisas relativas a essa aba
            painelPistas.SetActive(true);
            //Atualiza o número de páginas
            AtualizarPagina(1, 1); //Temporario
            //Ativa no ponto em que o jogador parou da ultima vez
            //diarioPagina[indexResumos].SetActive(true);
            //Sinaliza que essa é a aba ativada
            Debug.Log("Aba Diario/Resumo");
        }
        if (indexAbas == 3) //Investigações
        {
            //Highlight na aba que está selecionada
            //(Como que faz highlight? Será se eu mudo a imagem apenas?)
            //Aumenta o tamanho da imagem da aba
            abas[indexAbas].transform.localScale = new Vector3(1.3f, 1.3f, 1f);
            //Faz com que essa imagem da aba apareça na frente das outras
            abas[indexAbas].transform.SetAsLastSibling();
            //Desativa todas as coisas não relativas a essa aba
            DesativaPaginas();
            //Ativa as coisas relativas a essa aba
            painelInvestigacoes.SetActive(true);
            //Atualiza o número de páginas
            AtualizarPagina(indexInvestigacoes, totalPaginasInvestigacoes);
            //Ativa no ponto em que o jogador parou da ultima vez
            investigacoesPaginas[indexInvestigacoes].SetActive(true);
            //Sinaliza que essa é a aba ativada
            Debug.Log("Aba Diario/Resumo");
        }

        //Passar a aba apenas aumentando ou diminuindo o index
    }

    //TODO:Painel personagens tem que ser uma lista também
}
