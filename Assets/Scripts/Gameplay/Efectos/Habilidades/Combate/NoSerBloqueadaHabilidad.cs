public class NoSerBloqueadaHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente)
    {
        carta.noSerBloqueada = true;
    }
}