public class Defensor : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        // No puede atacar. Solo bloquea ataques.
        carta.defensor = true;
    }
}