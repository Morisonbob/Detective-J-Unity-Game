using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivarPuzzle : MonoBehaviour {

	public GameObject Player, Sala, Puzzle;
	public bool puzzleAtivo;

	void Update(){
		if (puzzleAtivo) {
			Player.GetComponent<Movimento> ().enabled = false;
			Sala.SetActive (false);
			Puzzle.SetActive (true);
		}

		if (puzzleAtivo && Input.GetButtonDown("Fire2")) {
			Player.GetComponent<Movimento> ().enabled = true;
			Sala.SetActive (true);
			Puzzle.SetActive (false);
			puzzleAtivo = false;
		}

		if (Puzzle.GetComponentInParent<Cursor> ().fimPuzzle) {
			Sala.GetComponent<SpriteRenderer> ().color = Color.blue;
		}
	}

	void OnTriggerStay2D (Collider2D collider){
		if (collider.tag == "Player" && Input.GetButtonDown ("Fire1") && !Puzzle.GetComponentInParent<Cursor>().fimPuzzle) {
			if (!puzzleAtivo) {
				puzzleAtivo = true;
			} 
		}
	}
}
