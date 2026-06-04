public class Indestructible : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        // La criatura con indestructible no puede ser destruida por daño o efectos que digan "destruye".
        // Esto significa que no se le puede asignar daño que la destruya, y tampoco se puede destruir por efectos.
        
        carta.indestructible = true; // Marca la carta como indestructible
    }
}