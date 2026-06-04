public class NoAtacarHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente)
    {
        carta.defensor = true;
    }
}