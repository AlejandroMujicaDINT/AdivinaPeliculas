using System.ComponentModel;

namespace AdivinaPeliculas
{
    class Pelicula : INotifyPropertyChanged
    {
        private string nombre;
        private string pista;
        private string imagen;
        private string dificultad;
        private string genero;

        public string Nombre
        {
            get => nombre;
            set
            {
                if (nombre != value)
                {
                    nombre = value;
                    NotifyPropetyChanged("Nombre");
                }
            }
        }
        public string Pista
        {
            get => pista;
            set
            {
                if (pista != value)
                {
                    pista = value;
                    NotifyPropetyChanged("Pista");
                }
            }
        }
        public string Imagen
        {
            get => imagen;
            set
            {
                if (imagen != value)
                {
                    imagen = value;
                    NotifyPropetyChanged("Imagen");
                }
            }
        }
        public string Dificultad
        {
            get => dificultad;
            set
            {
                if (dificultad != value)
                {
                    dificultad = value;
                    NotifyPropetyChanged("Dificultad");
                }
            }
        }
        public string Genero
        {
            get => genero;
            set
            {
                if (genero != value)
                {
                    genero = value;
                    NotifyPropetyChanged("Genero");
                }
            }
        }

        public Pelicula()
        {
            Nombre = "Nueva Pelicula";
            Dificultad = "Facil";
            Genero = "Comedia";
        }

        public Pelicula(string nombre, string pista, string imagen, 
            string dificultad, string genero)
        {
            Nombre = nombre;
            Pista = pista;
            Imagen = imagen;
            Dificultad = dificultad;
            Genero = genero;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
