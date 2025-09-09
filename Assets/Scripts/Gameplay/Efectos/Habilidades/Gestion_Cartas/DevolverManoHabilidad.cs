public class DevolverManoHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            cartajug.j1.GetComponent<Jugador>().mano.Add(cartajug.carta);
        }
        else if (cartajug.perteneceAJugador == cartajug.j2.GetComponent<Jugador>().id)
        {
            cartajug.j2.GetComponent<Jugador>().mano.Add(cartajug.carta);
        }
        cartajug = null;
    }
}