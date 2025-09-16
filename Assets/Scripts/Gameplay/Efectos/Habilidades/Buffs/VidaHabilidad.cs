public class VidaHabilidad : IHabilidad
{
    private int vida; // si el numero es negativo se le hace al oponente
    private bool sumar; // si es true se suma vida, si es false se resta vida al oponente
    public VidaHabilidad(int vida, bool sumar)
    {
        this.vida = vida;
        this.sumar = sumar;
    }
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente)
    {
        if(sumar)
        {
            cartajug.TratarVida(0,vida);
        }
        else
        {
            cartajug.TratarVida(1, vida);
        }
    }
}