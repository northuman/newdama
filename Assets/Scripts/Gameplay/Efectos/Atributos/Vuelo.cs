public class Vuelo : IAtributo
{
       public void aplicarAtributo(CartasJugadas carta)
    {
        // La criatura con vuelo no puede ser bloqueada excepto por criaturas con vuelo.
        // En este caso, no se implementa la lógica de bloqueo, ya que es parte del sistema de combate.
        // Este atributo simplemente indica que la criatura tiene vuelo.
        
        carta.vuelo = true;
    }

    public void AplicarAlHacerDanio(CartasJugadas atacante, int cantidad) { }

}