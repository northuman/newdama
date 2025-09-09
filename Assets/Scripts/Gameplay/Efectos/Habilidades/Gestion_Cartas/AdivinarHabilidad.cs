public class AdivinarHabilidad : IHabilidad
{
    private int cantidad;
    public AdivinarHabilidad(int cantidad) //en realidad solo se le muestra la carta al jugador
    {
        this.cantidad = cantidad;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        int adivinadas = 0;

        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            for (int i = 0; i < cartajug.j1.GetComponent<Jugador>().barajaPartida.Count && adivinadas < cantidad; i++)
            {
                Carta cartaAdivinada = cartajug.j1.GetComponent<Jugador>().barajaPartida[i];
                //Implementar interfaz grafica para que el jugador pueda elegir que hacer con la carta
                //Implementar animacion de cuando se ve la carta
            }
        }
        else
        {
            for (int i = 0; i < cartajug.j2.GetComponent<Jugador>().barajaPartida.Count && adivinadas < cantidad; i++)
            {
                Carta cartaAdivinada = cartajug.j2.GetComponent<Jugador>().barajaPartida[i];
                //Implementar interfaz grafica para que el jugador pueda elegir que hacer con la carta
                //Implementar animacion de cuando se ve la carta
            }
        }
    }
}