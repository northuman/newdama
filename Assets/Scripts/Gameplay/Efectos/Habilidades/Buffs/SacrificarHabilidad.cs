public class SacrificarHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente = null)
    {
        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            cartajug.j1.GetComponent<Jugador>().cementerio.Add(cartajug);
        }
        else
        {
            cartajug.j2.GetComponent<Jugador>().cementerio.Add(cartajug);
        }
        cartajug = null;
    }
}