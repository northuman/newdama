public class DanioHabilidad : IHabilidad
{
	private int danio;

	public DanioHabilidad(int danio)
	{
		this.danio = danio;
	}

	public void ActivarHabilidad(CartasJugadas carta, CartasJugadas oponente) //hace daño a oponente o al jugador
	{
		if(oponente != null)
		{
			oponente.RecibirDanio(danio);
		}
		else
		{
			carta.TratarVida(1, danio);
		}
	}
}