using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger
{
    void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente=null);
}