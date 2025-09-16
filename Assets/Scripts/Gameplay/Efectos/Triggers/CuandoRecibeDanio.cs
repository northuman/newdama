public class CuandoRecibeDanio : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoRecibeDanio()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}