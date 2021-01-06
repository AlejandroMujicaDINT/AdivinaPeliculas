using System.ComponentModel;

namespace AdivinaPeliculas
{
    class Pelicula : INotifyPropertyChanged
    {
        private string nombre;
        private string pista;
        private string imagen;
        private bool facil;
        private bool normal;
        private bool dificil;
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
        public bool Facil
        {
            get => facil;
            set
            {
                if (facil != value)
                {
                    facil = value;
                    NotifyPropetyChanged("Facil");
                }
            }
        }
        public bool Normal
        {
            get => normal;
            set
            {
                if (normal != value)
                {
                    normal = value;
                    NotifyPropetyChanged("Normal");
                }
            }
        }
        public bool Dificil
        {
            get => dificil;
            set
            {
                if (dificil != value)
                {
                    dificil = value;
                    NotifyPropetyChanged("Dificil");
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
            Facil = true;
            Genero = "Comedia";
        }

        public Pelicula(string nombre, string pista, string imagen, 
            bool facil, bool normal, bool dificil, string genero)
        {
            Nombre = nombre;
            Pista = pista;
            Imagen = imagen;
            Facil = facil;
            Normal = normal;
            Dificil = dificil;
            Genero = genero;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropetyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
