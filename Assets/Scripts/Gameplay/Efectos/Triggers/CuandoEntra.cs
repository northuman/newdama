public class CuandoEntra : ITrigger
{
    private IHabilidad habilidad;
    //no se me ocurre otra forma de hacer esto asi que hay varios constructores que llaman a diferentes habilidades


    public CuandoEntra(int cantidad, string nombreFicha, int tipo, string color, int fuerza, int resistencia) //CrearFicha
    {
        habilidad = new CrearFichaHabilidad(cantidad, nombreFicha, tipo, color, fuerza, resistencia);
    }

    public CuandoEntra(int danio) //DanioHabilidad
    {
        habilidad = new DanioHabilidad(danio);
    }

    public CuandoEntra(int cantidad, int tipo, int accion) //BuscarBibliotecaHabilidad
    {
        habilidad = new BuscarBibliotecaHabilidad(cantidad, tipo, accion);
    }

    public CuandoEntra(int cantidad, bool misma, bool mano) //Devolver del CementerioHabilidad
    {
        habilidad = new CementerioHabilidad(cantidad, misma, mano);
    }

    public CuandoEntra(int vida, bool sumar) //VidaHabilidad
    {
        habilidad = new VidaHabilidad(vida, sumar);
    }

    //queda:
    //- otorgar prisa a otra carta
    //- destruir carta
    //- revelar mano oponente
    //- elegir accion
    //- sacrificar
    //- descartar mano
    // todo esto esta explicado en el README


    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente = null)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}