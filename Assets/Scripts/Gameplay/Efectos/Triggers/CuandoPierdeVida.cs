public class CuandoPierdeVida : ITrigger
{
    //esta pensado en si el oponente pierde vida, no el jugador
    private IHabilidad habilidad;

    public CuandoPierdeVida(int vida, bool sumar) //VidaHabilidad 
    {
        habilidad = new VidaHabilidad(vida, sumar); //hay que sumarle al jugador asi que true
    }

    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}