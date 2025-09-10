public class BuscarBibliotecaHabilidad : IHabilidad
{
    private int cantidad;
    private int tipo; // 0 tierra, 1 criatura, 2 conjuro, 3 instantaneo, 4 artefacto, 5 encantamiento
    private string color;
    private int accion; // 0 poner en mano, 1 poner en cima de la biblioteca
    private Carta cartaBuscada;

    public BuscarBibliotecaHabilidad(int cantidad, int tipo, int accion, string color = null)
    {
        this.cantidad = cantidad;
        this.tipo = tipo;
        this.color = color;
        this.accion = accion;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        int devueltas = 0;

        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            for (int i = 0; i < cartajug.j1.GetComponent<Jugador>().barajaPartida.Count && devueltas < cantidad; i++)
            {
                cartaBuscada = cartajug.j1.GetComponent<Jugador>().barajaPartida[i];

                if ((color == null && tipo == cartaBuscada.tipo) || (tipo == cartaBuscada.tipo && color == cartaBuscada.color))
                {
                    if(accion == 0) // poner en mano
                    {
                        cartajug.j1.GetComponent<Jugador>().mano.Add(cartaBuscada);
                        cartajug.j1.GetComponent<Jugador>().barajaPartida.Remove(cartaBuscada);
                    }
                    else if(accion == 1) // poner en cima de la biblioteca
                    {
                        cartajug.j1.GetComponent<Jugador>().barajaPartida.Insert(0, cartaBuscada);
                    }
                    devueltas++;
                }
            }
        }
        else
        {
            for (int i = 0; i < cartajug.j2.GetComponent<Jugador>().barajaPartida.Count && devueltas < cantidad; i++)
            {
                cartaBuscada = cartajug.j2.GetComponent<Jugador>().barajaPartida[i];

                if ((color == null && tipo == cartaBuscada.tipo) || (tipo == cartaBuscada.tipo && color == cartaBuscada.color))
                {
                    if (accion == 0) // poner en mano
                    {
                        cartajug.j2.GetComponent<Jugador>().mano.Add(cartaBuscada);
                        cartajug.j2.GetComponent<Jugador>().barajaPartida.Remove(cartaBuscada);
                    }
                    else if (accion == 1) // poner en cima de la biblioteca
                    {
                        cartajug.j2.GetComponent<Jugador>().barajaPartida.Insert(0, cartaBuscada);
                    }
                    devueltas++;
                }
            }
        }
    }
}