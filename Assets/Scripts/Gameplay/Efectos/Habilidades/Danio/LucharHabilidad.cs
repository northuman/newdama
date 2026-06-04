public class LucharHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente) //se debe llamar fuera de la fase de combate
    {
        carta.Atacar(oponente);
    }
}