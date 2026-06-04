public class CuandoQuitaContador : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoQuitaContador()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}