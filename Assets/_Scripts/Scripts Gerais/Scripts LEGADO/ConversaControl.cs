using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConversaControl : MonoBehaviour {
	public ControlePersonagem cP;

	public GameObject PanelPrincipal,imagemFalante;
	public Text textoNome, textoTexto;
	public List<Sprite> imagemPersonagemCaixaTexto;
	public List<Sprite> panelBg;
	public List<string> falaTextos;
	public List<string> nomes;

	public int qtfFalasTotal,falaAtual;

	public bool conversando;

	public void LigarPanelPrincipal(int quemFala,int nomeFalante,int qtdFalas){
		conversando = true;
		imagemFalante.GetComponent<Image>().sprite = imagemPersonagemCaixaTexto[quemFala];
		textoNome.text = nomes[nomeFalante];
		textoTexto.text = falaTextos[0];
		qtfFalasTotal = qtdFalas;
		PanelPrincipal.SetActive (true);
	}

	public void DesligarPanelPrincipal(){
		conversando = false;
		PanelPrincipal.SetActive (false);
		falaAtual = 0;
		cP.paralisarPersonagem = false;
		GetComponent<ConversaControl> ().enabled = false;
	}

	void Update(){
		ControleAnalisandoObjeto ();
	}

	void ControleAnalisandoObjeto(){
		if (conversando && Input.GetButtonDown ("Fire1")) {
			if (falaAtual >= qtfFalasTotal - 1)
				DesligarPanelPrincipal ();
			else
				ContinuarAnaliseObjeto ();
		}
	}

	void ContinuarAnaliseObjeto(){
		falaAtual++;
		textoTexto.text = falaTextos [falaAtual];
	}
}
