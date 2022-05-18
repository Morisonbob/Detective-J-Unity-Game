using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControleFade : MonoBehaviour
{
	// Esse script é responsável por mostrar na tela em qual sala o jogador se encontra, além de fazer a transferência do jogador e da câmera para outra sala.

	public GameObject Player;					// GameObject que fica armazenado o personagem.
    
	public Vector3 posJogador, 					// Posição em que o jogador irá ficar depois de usar a porta.
				   posCamera;					// Posição em que a câmera irá ficar depois do uso da porta.
    
	public Animator aniCam;						// Componente Animator que fica na Camera.
    
	public bool ativarTransferencia, 			// Booleana que informa se a transferência pode ser feita (Personagem + Camera)			
				começouAni;						// Booleana que informa se a animação de transferencia começou
    											// Ver animação na camera para verificar quando as mesmas ficam True e False

	public Text textoSala;						// Componente de texto que irá mostrar em qual sala o jogador irá se encontrar
    
	public string textoSalaAtual;				// String que armazena qual sala o jogador se encontra
    
	void Awake()
    {
        aniCam = GetComponent<Animator>();		// Iniciando algumas variáveis
        textoSala.text = "Hall";
    }

    void Update()
    {
		// Caso a animação tenha começado - o jogador perde o controle sobre o personagem, quando terminá-la ele terá controle novamente
        if (começouAni)
            Player.GetComponent<Movimento>().enabled = false;
        else
            Player.GetComponent<Movimento>().enabled = true;

		// Caso a animação tenha chegado no momento em que a tela está escura - irá ser feito a transferência (Fica verdadeira na animação) 
        if (ativarTransferencia)
        {
            Player.transform.position = posJogador;
            transform.position = posCamera;
            textoSala.text = textoSalaAtual;
            ativarTransferencia = false;
        }
    }
}
