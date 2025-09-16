public class CuandoBloquea : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoBloquea()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}