public class Arrolla : IAtributo
{
       public void aplicarAtributo(CartasJugadas carta)
    {
        // El daño que sobra de un ataque se le resta a la vida del jugador oponente.
        carta.arrolla = true;
    }

}