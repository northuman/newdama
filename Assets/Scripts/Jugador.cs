using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{

    public int id;
    public string nombre;
    public int vida;
    public int[] mana; 
    public List<Carta> barajaOriginal = new List<Carta>();
    public List<Carta> barajaPartida;
    public List<Carta> mano;
    public List<CartasJugadas> cementerio;
    public List<CartasJugadas> pila;
    public List<CartasJugadas> tierras;
    public List<CartasJugadas> batalla;
    public GameObject CartaJugada;
    public static int tamanyoBaraja;   
    public int x;
    public GameObject Mano;

    bool tierraJugada = false;

    public Jugador( List<Carta> baraja)
    {
        
        barajaOriginal = baraja;
        barajaPartida = new List<Carta>();
        mano = new List<Carta>();
        cementerio = new List<CartasJugadas>();
        pila    = new List<CartasJugadas>();
        tierras = new List<CartasJugadas>();
        batalla = new List<CartasJugadas>();
    }

    public void crearBarajaPartida()
    {
        barajaPartida = barajaOriginal.ToList();
        barajaPartida.Randomizar();
    }

    //hay que crear la barajaOriginal para poder probar

    public void rellenarBaraja()
    {
        //Debug.Log("Relleno baraja para " + nombre);
        x = 0;
        tamanyoBaraja = 40;
        for(int i = 0; i < tamanyoBaraja; i++)
        {        
            x = Random.Range(0,CartaDatabase.listaCartas.Count -1);
            barajaOriginal.Add(new Carta(CartaDatabase.listaCartas[x]));
        }
    }

    /* obsoleto
    public void enderezoInicial()
    {
        for(int i = 0; i<batalla.Count; i++){
            batalla[i].girada=false;
            batalla[i].mareo=false;
        }

        for(int i = 0; i<tierras.Count; i++){
            tierras[i].girada=false;
        }

        tierraJugada=false;
    }*/

    public void girarTierra(GameObject tierraSeleccionada)
    {
        GameObject panelTierras = GameObject.Find("AreaTierras");
        if(tierraSeleccionada.transform.parent.gameObject == panelTierras){
            tierraSeleccionada.GetComponent<Reverso>().GirarCarta();
            if(tierraSeleccionada.GetComponent<CartasJugadas>().girada == true){
                SumarMana(tierraSeleccionada);
            }else{
                RestarMana(tierraSeleccionada);
            }
        }
    }

    public void SumarMana(GameObject cartaSeleccionada)
    {
        mana[0] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[1];
        mana[1] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[2];
        mana[2] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[3];
        mana[3] += cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[4];
        
    }
    public void RestarMana(GameObject cartaSeleccionada)
    {
        mana[0] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[1];
        mana[1] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[2];
        mana[2] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[3];
        mana[3] -= cartaSeleccionada.GetComponent<MostrarCarta>().GetCarta().costeMana[4];
    }


    /**Robar carta
    num: cantidad de cartas a robar.
    Se añade una carta a la mano por iteración
    Se llama a InstanciarPrefab para que haga una copia de la plantilla con la carta que se ha robado
    Se quita la carta de la baraja
    **/
    public bool robarCarta(int num){
        bool robar = false;
        if(barajaPartida.Count>=num){
            for(int i = 0; i<num;i++){
                //Debug.Log("robo de carta para " + nombre); 
                mano.Add(barajaPartida[barajaPartida.Count-1]);    
                //CartaJugada.GetComponent<MostrarCarta>().id = barajaPartida[barajaPartida.Count-1].id;  
                
                StartCoroutine(InstanciarPrefab(barajaPartida[barajaPartida.Count-1]));
                barajaPartida.RemoveAt(barajaPartida.Count-1);
            }
            robar = true;
        }
        return robar;
    }

    //num : Carta jugada, indice de mano
    //tonum : Carta sobre la que es jugada (encantamientos a cartas), indice de batalla
    public bool jugarCarta(int num, int tonum = -1){
        bool jugable = false;
        if(num<mano.Count){
            int tipocar = mano[num].tipo;
            //Jugar tierra
            if(tipocar == (int)Carta.Tipos.TIERRA && !tierraJugada){
                tierras.Add(gameObject.AddComponent<CartasJugadas>());
                mano.RemoveAt(num);
                tierraJugada = true;
            }
            //Jugar resto
            if( tipocar == (int)Carta.Tipos.ARTEFACTO      ||
                tipocar == (int)Carta.Tipos.CONJURO        ||
                tipocar == (int)Carta.Tipos.CRIATURA       ||
                tipocar == (int)Carta.Tipos.ENCANTAMIENTO  ||
                tipocar == (int)Carta.Tipos.INSTANTANEO){

                //Comprobar si hay mana suficiente

                bool auto = true; //Usar tierras automaticamente
                

                //Comprobar teniendo en cuenta tierras no giradas o no
                if(auto)
                    if(comprobarManaAuto(mano[num].costeMana)) jugable = true;
                else
                    if(comprobarManaNormal(mano[num].costeMana)) jugable = true;

                if(jugable){
                    //Restar coste a mana activo, en auto girar si es necesario
                    if(auto) aplicarCosteManaAuto(mano[num].costeMana);
                    else     aplicarCosteManaNormal(mano[num].costeMana);

                    //Artefactos se consideran como criaturas
                    if( tipocar == (int)Carta.Tipos.ARTEFACTO ||
                        tipocar == (int)Carta.Tipos.CRIATURA){

                        batalla.Add(gameObject.AddComponent<CartasJugadas>());

                    }else if(tipocar == (int)Carta.Tipos.CONJURO ||
                        tipocar == (int)Carta.Tipos.INSTANTANEO){
                        
                        pila.Add(gameObject.AddComponent<CartasJugadas>());

                    }else if(tipocar == (int)Carta.Tipos.ENCANTAMIENTO){
                        
                        batalla[tonum].encantamientos.Add(mano[num]);
                        
                    }

                    mano.RemoveAt(num);
                }
            }
        }
        return jugable;
    }

    //Activar efecto carta tierra
    //num : Indice de carta de tierra a aplicar
    public void aplicarCartaTierra(int num){
        if(num<tierras.Count){
            if(!tierras[num].girada){
                tierras[num].girada = true;
                mana[0]+=tierras[num].carta.costeMana[1];
                mana[1]+=tierras[num].carta.costeMana[2];
                mana[2]+=tierras[num].carta.costeMana[3];
                mana[3]+=tierras[num].carta.costeMana[4];
            }
        }
    }

    //Activar tierra en mana virtual
    //num : Indice de carta de tierra a aplicar
    public int[] aplicarCartaTierra(int num, int[] auxmana){
        if(num<tierras.Count){
            if(!tierras[num].girada){
                tierras[num].girada = true;
                auxmana[0]+=tierras[num].carta.costeMana[1];
                auxmana[1]+=tierras[num].carta.costeMana[2];
                auxmana[2]+=tierras[num].carta.costeMana[3];
                auxmana[3]+=tierras[num].carta.costeMana[4];
            }
        }
        
        return auxmana;
    }

    //Comprueba si una carta se puede jugar con el mana activo
    //coste : Array de costes de mana [INCOLORO, BLANCO, NEGRO, ROJO, VERDE]
    public bool comprobarManaNormal(int[] coste){
        bool puedeJugar = false;
        int[] auxmana = mana;
        //Si los valores de colores especificos los cumple
        if( auxmana[0]>=coste[1] ||
            auxmana[1]>=coste[2] ||
            auxmana[2]>=coste[3] ||
            auxmana[3]>=coste[4]){
            int manaEnIncoloro = 0;
            
            auxmana[0]-=coste[1];
            auxmana[1]-=coste[2];
            auxmana[2]-=coste[3];
            auxmana[3]-=coste[4];

            manaEnIncoloro += auxmana[0] + auxmana[1] + auxmana[2] + auxmana[3];
            //Si con el resto de mana se puede pagar el valor incoloro
            if(manaEnIncoloro>=coste[0]) puedeJugar = true;
        }
        return puedeJugar;
    }

    //Comprueba si una carta se puede jugar con el mana activo y tierras jugables
    //coste : Array de costes de mana [INCOLORO, BLANCO, NEGRO, ROJO, VERDE]
    public bool comprobarManaAuto(int[] coste){
        bool puedeJugar = false;
        int[] auxmana = mana;

        //Sumar mana a disposicion en cartas no giradas
        for(int i = 0; i<tierras.Count;i++){
            if(!tierras[i].girada){
                int auxcount = 0;
                for(int j = 1; j<tierras[i].carta.costeMana.Length; j++){
                    auxmana[auxcount] += tierras[i].carta.costeMana[j];
                    auxcount++;
                }
            }
        }

        //Si los valores de colores especificos los cumple
        if( auxmana[0]>=coste[1] ||
            auxmana[1]>=coste[2] ||
            auxmana[2]>=coste[3] ||
            auxmana[3]>=coste[4]){
            int manaEnIncoloro = 0;
            
            auxmana[0]-=coste[1];
            auxmana[1]-=coste[2];
            auxmana[2]-=coste[3];
            auxmana[3]-=coste[4];

            manaEnIncoloro += auxmana[0] + auxmana[1] + auxmana[2] + auxmana[3];
            //Si con el resto de mana se puede pagar el valor incoloro
            if(manaEnIncoloro>=coste[0]) puedeJugar = true;
        }
        return puedeJugar;
    }

    //Aplica coste de la carta al mana activo, asume que se ha comprobado que se pueda
    //coste : Array de costes de mana [INCOLORO, BLANCO, NEGRO, ROJO, VERDE]
    public void aplicarCosteManaNormal(int[] coste){
        //Aplica coste a manas especificos
        mana[0]-=coste[1];
        mana[1]-=coste[2];
        mana[2]-=coste[3];
        mana[3]-=coste[4];

        //Reparte coste incoloro automaticamente
        int aPagar = coste[0];
        for(int i = 0; i < mana.Length && aPagar > 0; i++){
            if(mana[i]>=aPagar){
                mana[i] -= aPagar;
                aPagar = 0;
            }else{
                aPagar -= mana[i];
                mana[i] = 0;
            }
        }
    }

    //Aplica coste de la carta al mana activo y gira tierras en caso de ser necesario
    //, asume que se ha comprobado que se pueda
    //coste : Array de costes de mana [INCOLORO, BLANCO, NEGRO, ROJO, VERDE]
    public void aplicarCosteManaAuto(int[] coste){
        int[] auxmana = mana;

        //Aplica coste a manas especificos
        auxmana[0]-=coste[1];
        auxmana[1]-=coste[2];
        auxmana[2]-=coste[3];
        auxmana[3]-=coste[4];

        //Paga lo que falte girando tierras, primero pagar mana especifico
        //Se comprueba cada tierra que pueda pagar la deuda
        for(int i = 0; i < tierras.Count && auxmana[0]<0 && auxmana[1]<0
        && auxmana[2]<0 && auxmana[3]<0; i++){
            //Se comprueba cada color de deuda en cada carta
            for(int j = 0; j < auxmana.Length &&
            j < tierras[i].carta.costeMana.Length; j++){
                if(!tierras[i].girada && auxmana[j]<0 && tierras[i].carta.costeMana[j+1] > 0){
                    auxmana = aplicarCartaTierra(i,auxmana);
                }
            }
        }

        //Reparte coste incoloro automaticamente a mana activo
        int aPagar = coste[0];
        for(int i = 0; i < auxmana.Length && aPagar > 0; i++){
            if(auxmana[i]>=aPagar){
                auxmana[i] -= aPagar;
                aPagar = 0;
            }else{
                aPagar -= auxmana[i];
                auxmana[i] = 0;
            }
        }

        //Pagar coste incoloro que falte con tierras no jugadas
        for(int i = 0; i < tierras.Count && aPagar > 0; i++){
            if(!tierras[i].girada){
                auxmana = aplicarCartaTierra(i,auxmana);

                //Recomprobar cada mana y saldar deudas
                for(int j = 0; j < auxmana.Length; j++){
                    if(auxmana[i]>=aPagar){
                        auxmana[i] -= aPagar;
                        aPagar = 0;
                    }else{
                        aPagar -= auxmana[i];
                        auxmana[i] = 0;
                    }
                }
            }
        }

        mana = auxmana;
    }

    //Elimina carta en mano
    //num : indice de carta de mano
    public void eliminarCartaMano(int num){
        //Eliminar en caso de tener id
        /*
        var cartaAEliminar = mano.SingleOrDefault(r => r.Id == num);
        if (cartaAEliminar != null)
            mano.Remove(cartaAEliminar);
            */

        //Eliminar en caso de posicion
        mano.RemoveAt(num);
    }


    public void reiniciarEstadisticasBatalla(){
        //Reiniciar a cada carta en campo de batalla fuerza y resistencia por defecto
        for(int i = 0; i<batalla.Count; i++){
            batalla[i].fuerzaActual = batalla[i].carta.fuerza;
            batalla[i].resistenciaActual = batalla[i].carta.resistencia;
        }
    }


    //
    void Start()
    {    
        vida = 20;
        mana = new int[]{0,0,0,0};
    }

    // 
    void Update()
    {
        //CartaJugada.GetComponent<CartasJugadas>().perteneceAJugador = id;
    }

    //Rutina para instanciar prefabs
    //Instancia una plantilla de carta vacía. Se le pasa una carta y le pone su id a la instancia.
    IEnumerator InstanciarPrefab(Carta carta)
    {
        //yield return new WaitForSeconds(0.5f);
        Instantiate(CartaJugada, transform.position, transform.rotation);
        CartaJugada.GetComponent<MostrarCarta>().id = carta.id;
        CartaJugada.GetComponent<CartasJugadas>().perteneceAJugador = id;
        
        //Debug.Log("instancio carta " + CartaJugada.GetComponent<MostrarCarta>().id);
        yield return null;

    }

    //Encontrar carta por ID
    //Se le pasa un id, busca en BarajaOriginal y devuelve la carta
    public Carta findById(int id){
        foreach (Carta c in barajaOriginal){
            if (c.id == id){
                return c;
            }
        }
        return null;
    }
}
