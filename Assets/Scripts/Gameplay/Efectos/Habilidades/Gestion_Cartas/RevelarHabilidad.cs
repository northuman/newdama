public class RevelarHabilidad : IHabilidad
{

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            //cartajug.j2.GetComponent<Jugador>(); debemos revelar la mano del oponente con la animacion integrada
        }
        else
        {
            //cartajug.j1.GetComponent<Jugador>(); debemos revelar la mano del oponente con la animacion integrada
        }
    }
}