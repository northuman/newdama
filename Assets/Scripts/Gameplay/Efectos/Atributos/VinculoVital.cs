public class VinculoVital : IAtributo
{
    public void AplicarAlHacerDanio(CartasJugadas atacante, int cantidad)
    {
        //// el dano que haga la carta se le suma a la vida del jugador
        //if (atacante != null && cantidad > 0)
        //{
        //    Debug.Log($"[Vínculo Vital] {atacante.carta.nombreCarta} hace {cantidad} danioo y su controlador gana esa vida.");

        //    // Busca al jugador dueño de la carta
        //    Jugador propietario = null;
        //    if (atacante.perteneceAJugador == 1)
        //        propietario = GameObject.Find("Jugador").GetComponent<Jugador>();
        //    else if (atacante.perteneceAJugador == 2)
        //        propietario = GameObject.Find("Oponente").GetComponent<Jugador>();

        //    if (propietario != null)
        //    {
        //        propietario.vida += cantidad;
        //        Debug.Log($"[Vínculo Vital] {propietario.nombre} gana {cantidad} de vida.");
        //    }
        //}
    }

    public void aplicarAtributo(CartasJugadas carta) { }
}