public class HincharDebilitarHabilidad : IHabilidad
{
    private int fuerza;
    private int resistencia;
    private bool esta; // es a esta misma carta a la que se le aplica
    private string color;

    public HincharDebilitarHabilidad(bool esta, int fuerza, int resistencia, string color = "")
    {
        this.esta = esta;
        this.fuerza = fuerza;
        this.resistencia = resistencia;
        this.color = color;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if (esta)
        {
            cartajug.carta.fuerza += fuerza;
            cartajug.carta.resistencia += resistencia;
        }
        else if (oponente == null) 
        {
        
            if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
            {
                for (int i = 0; i < cartajug.j1.GetComponent<Jugador>().batalla.Count; i++)
                {
                    CartasJugadas cartaEditable = cartajug.j1.GetComponent<Jugador>().batalla[i];
                    if ((cartaEditable.carta.tipo == 0 && color == "") || (cartaEditable.carta.tipo == 0 && cartaEditable.carta.color.Equals(color)))
                    {
                        cartaEditable.carta.fuerza += fuerza;
                        cartaEditable.carta.resistencia += resistencia;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cartajug.j2.GetComponent<Jugador>().batalla.Count; i++)
                {
                    CartasJugadas cartaEditable = cartajug.j2.GetComponent<Jugador>().batalla[i];
                    if ((cartaEditable.carta.tipo == 0 && color == "") || (cartaEditable.carta.tipo == 0 && cartaEditable.carta.color.Equals(color)))
                    {
                        cartaEditable.carta.fuerza += fuerza;
                        cartaEditable.carta.resistencia += resistencia;
                    }
                }
            }
        }
        else // se da por hecho que se pasa un numero negativo por lo que afecta al contrario
        {
            oponente.carta.fuerza += fuerza;
            oponente.carta.resistencia += resistencia;
        }
    }
}