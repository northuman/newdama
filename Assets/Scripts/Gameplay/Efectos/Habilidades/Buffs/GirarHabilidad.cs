public class GirarHabilidad : IHabilidad
{
    public void ActivarHabilidad(CartasJugadas cartajug, CartasJugadas oponente = null)
    {
        if (!cartajug.girada)
        {
            cartajug.girada = true;
        }
        else
        {
            cartajug.girada = false;
        }
    }
}