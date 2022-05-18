using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLockpick : MonoBehaviour
{

    //<Variáveis> ======================================================================================================
    //	[HideInInspector]
    public bool mexeu,                  // Impedir que o jogador saia trocando de pino ao segurar o Botão / Analógico
                segurandoPino;          // Se o jogador estiver controlando um pino, ele não poderá trocar de pino

    //</Viaráveis> =====================================================================================================

    void Update()
    {
        Movimento();                    // Método para fazer a movimentação do Player - Indicador 
    }

    void Movimento()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 && !mexeu && !segurandoPino)
        {
            mexeu = true;                                                                                               // Mexeu fica verdadeiro 
                                                                                                                        //1.5 é a distancia entre os pinos, valor arbitrario
            gameObject.transform.position += new Vector3(1.5f * Input.GetAxisRaw("Horizontal"), 0, 0);                  // A posição do indicador irá andar em 1.5 unidades na direção
            ControlLockpick.posicao += (int)Input.GetAxisRaw("Horizontal");                                             // do Input.
                                                                                                                    //MUDAR ISSO PARA VARIAVEIS DEPOIS																											// Além disso, a "posição" em que o jogador se encontra será 
            if (transform.position.x > 4.5f)
            {                                                                           // contabilizada pelo Incremento do valor da direção do Input
                transform.position = new Vector2(0, -2);                                                                // que poderá ser apenas 1 ou -1
                ControlLockpick.posicao = 1;
                // Caso a posição do jogador seja maior que o primeiro e ultimo
            }
            else if (transform.position.x < 0)
            {                                                                       // pino, o jogador irá para a ultima ou primeira posição,
                transform.position = new Vector2(4.5f, -2);                                                         // respectivamente
                ControlLockpick.posicao = 4;
            }
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {                                                                   // Se o jogador não fizer nem um Input do tipo Horizontal, ele poderá
            mexeu = false;                                                                                          // se mexer novamente, se tiver utilizado essa ação.
        }
    }
}
