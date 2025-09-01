public class Vigilancia : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        //no se gira al atacar
        carta.vigilancia = true;
        carta.girada = false; // La carta no se gira al atacar
    }

    public void AplicarAlHacerDanio(CartasJugadas atacante, int cantidad) { }

}