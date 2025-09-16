public class CuandoHaceDanio : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoHaceDanio()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}