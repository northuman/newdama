public class CrearFichaHabilidad : IHabilidad
{
    public int cantidad;
    public int tipo;
    public string nombreCarta;
    public string color;
    public int fuerza;
    public int resistencia;

    public CrearFichaHabilidad(int cantidad, int tipo = 1, string nombreCarta, string color = "cualquiera", int fuerza = 1, int resistencia = 1)
    {
        this.cantidad = cantidad;
        this.tipo = tipo;
        this.nombreCarta = nombreCarta;
        this.color = color;
        this.fuerza = fuerza;
        this.resistencia = resistencia;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente = null)
    {
        
        for (int i = 0; i < cantidad; i++)
        {
            cartaFicha = new Carta(nombreCarta, tipo, "", null, color, fuerza, resistencia, "", "");
            GameObject go = new GameObject("Ficha");
            CartasJugadas ficha= go.AddComponent<CartasJugadas>();
            ficha.Inicializar(cartaFicha);

            if (cartajug.perteneceAJugador == cartajug.j1.GetComponent<Jugador>().id)
            {
                cartajug.j1.GetComponent<Jugador>().batalla.Add(ficha);
            }
            else
            {
                cartajug.j2.GetComponent<Jugador>().batalla.Add(ficha);
            }
        }
    }
}