public class CuandoOtraEntra : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoOtraEntra()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}