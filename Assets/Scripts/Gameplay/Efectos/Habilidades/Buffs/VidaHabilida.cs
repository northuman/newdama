public class VidaHabilidad : IHabilidad
{
    private int vida; // si el numero es negativo se le hace al oponente
    public VidaHabilidad(int vida)
    {
        this.vida = vida;
    }
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if(vida > 0)
        {
            cartajug.TratarVida(0,vida);
        }
        else
        {
            cartajug.TratarVida(1, vida);
        }
    }
}