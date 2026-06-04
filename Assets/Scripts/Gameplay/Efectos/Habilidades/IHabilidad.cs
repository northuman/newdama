using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHabilidad
{
    void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente=null);
}