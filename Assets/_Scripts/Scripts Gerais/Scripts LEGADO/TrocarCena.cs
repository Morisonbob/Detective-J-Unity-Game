using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour {

	int currentScene; 

	void Update () {
		currentScene = SceneManager.GetActiveScene ().buildIndex;						//Irá receber o número da cena carregada (Build Settings)
		TrocarCenaAtual ();
	}

											
	void TrocarCenaAtual(){																//Método responsável por realizar a troca das Cenas
		if (Input.GetButtonDown ("Fire3")){												//Fire3 = Left Shift		
			if (currentScene == SceneManager.sceneCountInBuildSettings - 1)		 		//Pega o número total de cenas no projeto		
				SceneManager.LoadScene (0);												//Se a Cena carregada no momento for a "ultima", será carregada a primeira cena		
			else 																		//Se não, ele irá apenas para a próxima.
				SceneManager.LoadScene (currentScene + 1);								//Mesma fila do Build Setting		
		}
	} 
}
