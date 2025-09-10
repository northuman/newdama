public class ContadoresHabilidad : IHabilidad
{
    private int cantidad;
    private int fuerza;
    private int resistencia;

    public ContadoresHabilidad(int cantidad, int fuerza, int resistencia)
    {
        this.cantidad = cantidad;
        this.fuerza = fuerza;
        this.resistencia = resistencia;
    }

    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas objetivo) //revisar cuando este hecho el objeto fisico como tal
    {
        if(objetivo != null)
        {
            if (objetivo.carta.tipo == 1) // si es criatura
            {
                for (int i = 0; i < cantidad; i++)
                {
                    var contador = new Contador(fuerza, resistencia);
                    objetivo.carta.fuerza += fuerza;
                    objetivo.carta.resistencia += resistencia;
                }
            }
        }
        else
        {
            for (int i = 0; i < cantidad; i++)
            {
                var contador = new Contador(fuerza, resistencia);
                //hace falta saber la posicion de la carta para ponerselo encima
                cartajug.carta.fuerza += fuerza;
                cartajug.carta.resistencia += resistencia;
            }
        }

    }
}