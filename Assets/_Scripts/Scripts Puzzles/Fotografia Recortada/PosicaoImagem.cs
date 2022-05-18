using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosicaoImagem : MonoBehaviour
{

    // Coisas que precisam ser configuradas no Inspector para que funcione
    //1. Colocar na variável scriptCursor, o objeto que contem o script da classe Cursor.
    //2. No inspector, editar manualmente qual a posição certa que essa imagem deve ter.

    //Variável do tipo Cursor (classe), para saber se alguma peça está, ou não, sendo carregada
    public Cursor scriptCursor;
    //Variável para armazenar, MANUALMENTE NO INSPECTOR, qual é a posicao no Panel que é a posição correta E FINAL dessa imagem.
    [Tooltip("Posição final da imagem, DEVE SER COLOCADA MANUALMENTE")]
    public Vector3 posCorreta;
    //Variável que irá informar se a peça está, ou não, no lugar certo.
    public bool posOk;

    void Update()
    {
        VerificarPosicao();
    }

    //No método "VerificarPosica", é feito duas (2) verificações. 
    //1: Se essa peça (this) está, de fato, na mesma posição que a variável posCorreta.
    //2: Se o jogador NÃO está segurando uma peça no momento.¹
    //Feito isso, se tudo estiver ok, a peça ativa a booleana posOk, para informar que ela está, de fato, na posicao certa, 
    //que irá se comunicar com o script Cursor.

    void VerificarPosicao()
    {
        if (posCorreta == GetComponent<RectTransform>().localPosition && !scriptCursor.holding)
            posOk = true;
        else
            posOk = false;
    }
}

//¹:
//Por quê o jogador NÃO deve estar segurando uma peça?
//Imagine a situação em que falta uma peça para concluir o quebra-cabeça. 
//Se essa condição não existece, seria apenas necessário o jogador passar com a peça por cima para concluir o quebra cabeça
//sem antes confirmar a posição da peça.
