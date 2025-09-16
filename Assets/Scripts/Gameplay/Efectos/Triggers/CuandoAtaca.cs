public class CuandoAtaca : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoAtaca()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}