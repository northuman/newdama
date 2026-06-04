public class NoBloquearHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente)
    {
        carta.girada = true;
    }
}