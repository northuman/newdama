public class CementerioHabilidad : IHabilidad
{
    private int cantidad;
    public bool misma;
    public bool mano;

    public CementerioHabilidad(int cantidad, bool misma, bool mano)
    {
        this.cantidad = cantidad;
        this.misma = misma;
        this.mano = mano; //si es falso es a la biblioteca del jugador

    }
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        int devueltas = 0;

        if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
        {
            for (int i = 0; i < cartajug.j1.GetComponent<Jugador>().cementerio.Count && devueltas < cantidad; i++)
            {
                CartasJugadas cartaCementerio = cartajug.j1.GetComponent<Jugador>().cementerio[i];

                if ((misma && cartajug.carta.nombreCarta.Equals(cartaCementerio.carta.nombreCarta)) || !misma) 
                {
                    if (mano)
                    {
                        cartajug.j1.GetComponent<Jugador>().mano.Add(cartaCementerio.carta);
                    }
                    else
                    {
                        cartajug.j1.GetComponent<Jugador>().barajaPartida.Add(cartaCementerio.carta);
                    }
                    cartajug.j1.GetComponent<Jugador>().cementerio.Remove(cartaCementerio);
                    cartaCementerio = null;
                    devueltas++;
                }
            }
        }
        else
        {
            for (int i = 0; i < cartajug.j2.GetComponent<Jugador>().cementerio.Count; i++)
            {
                CartasJugadas cartaCementerio = cartajug.j2.GetComponent<Jugador>().cementerio[i];

                if ((misma && cartajug.carta.nombreCarta.Equals(cartaCementerio.carta.nombreCarta)) || !misma)
                {
                    if (mano)
                    {
                        cartajug.j2.GetComponent<Jugador>().mano.Add(cartaCementerio.carta);
                    }
                    else
                    {
                        cartajug.j2.GetComponent<Jugador>().barajaPartida.Add(cartaCementerio.carta);
                    }
                    cartajug.j2.GetComponent<Jugador>().cementerio.Remove(cartaCementerio);
                    cartaCementerio = null;
                    devueltas++;
                }
            }
        }

    }
}