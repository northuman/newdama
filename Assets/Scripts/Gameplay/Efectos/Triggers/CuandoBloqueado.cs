public class CuandoBloqueado : ITrigger {     
    
    private IHabilidad habilidad;
    
    //public CuandoBloqueado()
    //{
    //    habilidad = new ;
    //}
    public void ActivarTrigger(CartasJugadas estaCarta, CartasJugadas oponente)
    {
        habilidad.ActivarHabilidad(estaCarta, oponente);
    }
}