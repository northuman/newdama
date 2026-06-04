public class ToqueMortal : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        // Destruye a la carta que ataca
        carta.toqueMortal = true;
    }
}