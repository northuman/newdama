public class CuandoOtorgaContador : ITrigger
{

    private IHabilidad habilidad;

    //public CuandoOtorgaContador()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}