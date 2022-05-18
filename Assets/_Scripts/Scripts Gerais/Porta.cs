using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Porta : MonoBehaviour
{
	// Esse script serve para fazer a transição entre salas. 
	// Ele fica associado as portas do jogo.
	// Como funciona: Quando o jogador estiver em contato com a porta e pressionar o botão de ação. Ele ativará uma animação de transição. (Ver ControleFade)
	// A animação funciona da seguinte maneira: 1. O alpha de uma imagem preta (que é filha da camera e fica na sua frente), vai de 1 a 0.
	//											2. Enquanto a tela está toda preta, a animação marca uma booleana que começa o movimento do player e da camera.
	//											3. Quando é feito a transferência de posição, a imagem preta volta a ter seu alpha, gradativamente, em 1.

    public Camera mainCamera; 					// Variável para o script ter acesso a camera.
    
	public Vector3 	PosJogador, 				// Posição em que o jogador irá aparecer depois de utilizar a porta
					PosCamera;					// Posição em que a camera irá ficar depois de utilizar a porta
    
	public string proximaSala;					// Nome da sala atual em que o jogador se encontra.
   
	void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player" && Input.GetButtonDown("Fire1"))
        {
			
			if (!mainCamera.GetComponent<Animator>().enabled)					// Caso o animator esteja desativado (Primeira ocorrência)
                mainCamera.GetComponent<Animator>().enabled = true;				// ele irá ativar.
            else mainCamera.GetComponent<Animator>().SetTrigger("iniciar");		// Mas se já estiver ativado, ele irá executar essa animação novamente.
																				// Fiz isso para a animação, que fica na camera, não acontecer assim que o jogo é ligado.
          
			// Nessas 2 linhas esse script manda as informações que ficam na porta para o ControleFade, lá será feito o reposicionamento do personagem e da camera no momento certo
			mainCamera.GetComponent<ControleFade>().posCamera = PosCamera;		
            mainCamera.GetComponent<ControleFade>().posJogador = PosJogador;
           
			// Já aqui ele irá informar em que sala o jogador irá se encontrar.
			mainCamera.GetComponent<ControleFade>().textoSalaAtual = proximaSala; 
        }
    }
}
