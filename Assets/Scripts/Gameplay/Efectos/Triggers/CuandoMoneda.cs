public class CuandoMoneda : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoMoneda()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}