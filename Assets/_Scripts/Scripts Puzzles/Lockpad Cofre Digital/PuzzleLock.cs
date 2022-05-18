using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleLock : MonoBehaviour
{
    //A senha pra destrancar o cofre
    public string codigoFinal;
    //A senha que o jogador colocou
    public string codigoInput;
    //String vazia (Se pá eu deleto isso aqui)
    public string nada;
    //O display que mostra a senha
    public GameObject display;
    //Numero de números inseridos
    public int currentHouse;
    //O limite de digitos que pode ter no cofre
    //Botei um Range pq eu quis, sei lá
    [Range (4,8)] public int limiteDigitos;

    //Pega o audiosource que será responsavel pelo audio
    public AudioSource audioS;
    //Referencia ao jogador
    public Movimento player;
    //Referencia ao prefab do puzzle
    public GameObject canvas;
    //Os clips de audio que serão tocados
    public AudioClip acertou;
    public AudioClip errou;

    //Função que recebe uma string (q vai ser o número assignado ao botão)
    public void InputButton(string Input)
    {
        //Checa se a quantidade de numeros já digitados é menor do que o limite
        if (currentHouse < limiteDigitos)
        {
            //Adiciona o numero apertado no botão a string de checagem
            codigoInput += Input;
            //Aumenta a variavel que vê quantos numeros foram digitados
            currentHouse++;
            //Mostra no display o que foi digitado
            display.GetComponent<Text>().text = codigoInput;

        }
    }
    //Apaga os números digitados
    public void Apagar()
    {
        //Reseta a quantidade de numeros digitados
        currentHouse = 0;
        //Zera o que aparece no display
        codigoInput = nada;
        display.GetComponent<Text>().text = codigoInput;
    }

    //Checa se o que foi digitado está correto
    public void Confirmar()
    {
        //Checa se o código digitado coresponde a senha
        if (codigoInput == codigoFinal)
        {
            //Aparece um texto de congratulações no display
            display.GetComponent<Text>().text = "Acertou";
            //Reseta a variavel de numeros digitados
            currentHouse = 0;
            //Muda o clip para o de vitória
            audioS.clip = acertou;
            //Toca um som e fecha o puzzle
            audioS.Play();
            //O prefab é desativado após 2 segundos, o tempo pra musica tocar
            Invoke("DesativaPuzzle", 2);
        }
        //Caso não corresponda a senha
        else
        {
            //Muda o clip de audio para um de erro
            audioS.clip = errou;
            //Toca o audio de erro
            audioS.Play();
            //Reseta a variavel de numeros digitados
            currentHouse = 0;
            //Zera o que aparece no display
            codigoInput = nada;
            //Manda uma mensagem para o diplay
            display.GetComponent<Text>().text = "Errou";
        }
    }

    void DesativaPuzzle()
    {
        print("carai");
        canvas.SetActive(false);
        player.conversando = false;
    }
}
