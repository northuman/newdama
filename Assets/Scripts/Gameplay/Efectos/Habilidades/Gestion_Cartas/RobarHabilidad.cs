public class RobarHabilidad : IHabilidad
{
    private int cantidad;

    public RobarHabilidad(int cantidad)
    {
        this.cantidad = cantidad;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            cartajug.j1.GetComponent<Jugador>().RobarCarta(cantidad);
        }
        else
        {
            cartajug.j2.GetComponent<Jugador>().RobarCarta(cantidad);
        }
    }
}