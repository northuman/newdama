public class Destello : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        // El jugador puede jugar esta carta en cualquier momento, incluso durante el turno del oponente.
        carta.destello = true; // Marca la carta con el atributo destello
        carta.mareo = false; // No se puede aplicar mareo a una carta con destello

    }
}