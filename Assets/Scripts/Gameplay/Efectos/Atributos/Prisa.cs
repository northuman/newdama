using UnityEngine;

public class Prisa : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        Debug.Log("Prisa aplicada");

        carta.mareo = false; // Puede atacar el turno en que entra al campo de batalla
    }

    public void AplicarAlHacerDanio(CartasJugadas atacante, int cantidad) { }

}