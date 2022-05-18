using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlePersonagem : MonoBehaviour {
	public ConversaControl falaTexto;
	public LayerMask layerItens;
	float inputX,inputY;
	public float deslocamento;
	bool correndo;

	public GameObject itemProximo,apertarE;

	public bool paralisarPersonagem;

	Collider2D hit;

	void Awake (){
		itemProximo = null;
	}

	void Update () {
		if (!paralisarPersonagem) { 
			ReceberInputs ();
			VerificarItem ();
		}
			
		hit = Physics2D.OverlapCircle (transform.position, 1.5f, layerItens);

		if (hit != null) 
			itemProximo = hit.gameObject;

		if (paralisarPersonagem)
			apertarE.SetActive(false);
	}

	void FixedUpdate(){
		if (!paralisarPersonagem) 
			Movimento ();
		else
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
	}

	void ReceberInputs(){
		inputX = Input.GetAxisRaw ("Horizontal");
		inputY = Input.GetAxisRaw ("Vertical");
		correndo = ((Input.GetButton("Fire1")) ? true : false);
	}

	void Movimento(){
		if (correndo) 
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (inputX,inputY) * Mathf.Pow(deslocamento,2);
		else 
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (inputX,inputY) * deslocamento;
	}

	void VerificarItem(){
		if (itemProximo != null && Vector2.Distance (gameObject.transform.position, itemProximo.transform.position) < 2) {
			if (Input.GetButtonDown ("Fire2")) {
				paralisarPersonagem = true;
				falaTexto.enabled = true;

				falaTexto.falaTextos.RemoveRange (0, falaTexto.falaTextos.Count);

				for (int i = 0; i < itemProximo.GetComponent<IdItem> ().textoVerificacao.Count; i++) {
					falaTexto.falaTextos.Add (itemProximo.GetComponent<IdItem> ().textoVerificacao [i]);
				}

				falaTexto.LigarPanelPrincipal (0, 0, itemProximo.GetComponent<IdItem> ().textoVerificacao.Count);
			}
			apertarE.SetActive (true);
		} else 
			apertarE.SetActive (false);
	}
}
