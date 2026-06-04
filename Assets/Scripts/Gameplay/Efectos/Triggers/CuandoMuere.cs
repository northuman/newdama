public class CuandoMuere : ITrigger
{
    private IHabilidad habilidad;
    
    public CuandoMuere(int cantidad, string nombreFicha, int tipo, string color, int fuerza, int resistencia) //CrearFicha
    {
        habilidad = new CrearFichaHabilidad(cantidad, nombreFicha, tipo, color, fuerza, resistencia);
    }

    public CuandoMuere(int danio) //DanioHabilidad
    {
        habilidad = new DanioHabilidad(danio);
    }

    public CuandoMuere(bool esta, int fuerza, int resistencia) //HincharDebilitarHabilidad (debilitar la del contrario)
    {
        habilidad = new HincharDebilitarHabilidad(esta, fuerza, resistencia);
    }

    public CuandoMuere(int cantidad, bool extra) //RobarHabilidad
    {
        habilidad = new RobarHabilidad(cantidad);
    }

    public CuandoMuere(int cantidad, int fuerza, int resistencia) //ContadoresHabilidad
    {
        habilidad = new ContadoresHabilidad(cantidad, fuerza, resistencia);
    }

    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente = null)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}