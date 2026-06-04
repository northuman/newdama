public class Amenaza : IAtributo
{
    public void aplicarAtributo(CartasJugadas carta)
    {
        // tienen que bloquearla 2 o mas criaturas
        //NO TENEMOS LOGICA DE BLOQUEO IMPLEMENTADA, ASI QUE POR AHORA NO HACE NADA
        carta.amenaza = true; // Marca la carta como amenaza
    }
}