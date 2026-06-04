public class IndestructibleHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente)
    {
        carta.indestructible = true;
    }
}