public class VinculoVital : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        carta.vinculoVital = true;
    }
}