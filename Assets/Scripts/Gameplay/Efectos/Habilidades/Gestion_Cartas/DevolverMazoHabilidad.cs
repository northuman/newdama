public class DevolverMazoHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            cartajug.j1.GetComponent<Jugador>().barajaPartida.Add(cartajug.carta);
            cartajug.j1.GetComponent<Jugador>().barajaPartida.Randomizar();
        }
        else if (cartajug.perteneceAJugador == cartajug.j2.GetComponent<Jugador>().id)
        {
            cartajug.j2.GetComponent<Jugador>().barajaPartida.Add(cartajug.carta);
            cartajug.j2.GetComponent<Jugador>().barajaPartida.Randomizar();
        }
        cartajug = null;
    }
}