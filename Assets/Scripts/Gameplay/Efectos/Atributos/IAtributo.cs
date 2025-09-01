public interface IAtributo
{
	// Precondicion: Se da por hecho que se han hecho las comprobaciones necesarias antes de aplicar el atributo.
	public void aplicarAtributo(CartasJugadas carta);

	public void AplicarAlHacerDanio(CartasJugadas atacante, int cantidad);
}