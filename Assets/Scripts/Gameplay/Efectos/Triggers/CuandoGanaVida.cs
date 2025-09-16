public class CuandoGanaVida : ITrigger
{
    private IHabilidad habilidad;

    public CuandoGanaVida(int cantidad, int fuerza, int resistencia) //ContadoresHabilidad
    {
        habilidad = new ContadoresHabilidad(cantidad, fuerza, resistencia);
    }

    public CuandoGanaVida(bool esta, int fuerza, int resistencia, string color) //HincharDebilitarHabilidad
    {
        habilidad = new HincharDebilitarHabilidad(esta, fuerza, resistencia, color);
    }

    public CuandoGanaVida(int vida, bool sumar) //VidaHabilidad (si es true se le suma la vida al jugador, si es false se le resta al oponente)
    {
        habilidad = new VidaHabilidad(vida, sumar);
    }


    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}