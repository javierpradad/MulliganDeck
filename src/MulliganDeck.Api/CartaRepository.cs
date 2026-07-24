public class CartaRepository{
    private List<Carta> listaCartas = new List<Carta>{
        new Carta(1, "Ureni de lo no escrito", 7, "Rojo, Verde, Azul", 7, 7),
        new Carta(2, "Engendro de escarcha engañoso", 2, "Azul", 1, 1),
        new Carta(3, "Sirviente de la señora dragon", 2, "Rojo", 1, 3)
    };

    public List<Carta> GetCartas(){
        return listaCartas;
    }

    public Carta GetCartaPorId(int id){
        return listaCartas.FirstOrDefault(c => c.Id == id);
    }

    public Carta AddCarta(Carta carta){
        var maxID = listaCartas.Any() ? listaCartas.Max(c => c.Id) : 0;
        carta = carta with { Id = maxID + 1 };
        listaCartas.Add(carta);
        return carta;
    }

    public bool UpdateCarta(int id, Carta carta){
        var index = listaCartas.FindIndex(c => c.Id == id);
        if (index != -1){
            carta = carta with { Id = id };
            listaCartas[index] = carta;
            return true;
        }
        return false;
    }

    public bool DeleteCarta(int id){
        var carta = listaCartas.FirstOrDefault(c => c.Id == id);
        if (carta != null){
            listaCartas.Remove(carta);
            return true;
        }
        return false;
    }
}