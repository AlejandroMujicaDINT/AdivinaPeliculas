using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

using Newtonsoft.Json;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdivinaPeliculas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Pelicula> listaPeliculas;
        ObservableCollection<Pelicula> peliculasAleatorias;
        List<string> listaGenerosComboBox;
        List<string> listaDificultadComboBox;
        List<bool> guardarPistas;
        List<string> guardarRespuestas;
        private string archivoJson;
        private string rutaImagenes = "..\\images\\";
        private int indice;

        public int Indice
        {
            get => indice;
            set
            {
                indice = value;
                jugarDockPanel.DataContext = peliculasAleatorias[indice];
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            listaGenerosComboBox = new List<string> { "Comedia", "Drama", "Accion", "Terror", "Sci-fi" };
            listaDificultadComboBox = new List<string> { "Facil", "Normal", "Dificil" };

            listaPeliculas = new ObservableCollection<Pelicula>();

            PeliculasListBox.DataContext = listaPeliculas;
            generoPeliculaComboBox.ItemsSource = listaGenerosComboBox;
            dificultadPeliculaComboBox.ItemsSource = listaDificultadComboBox;

            cargarPeliculas("peliculas.json");
        }

        private void guardarButton_Click(object sender, RoutedEventArgs e)
        {
            string peliculasJson = JsonConvert.SerializeObject(listaPeliculas);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog.Filter = "Json files (*.json;)|*.json;|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, peliculasJson);
            else
                MessageBox.Show("No has selecccionado archivo para guardar.","Información",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void cargarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Json files (*.json;)|*.json;|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                archivoJson = openFileDialog.FileName;
                cargarPeliculas(archivoJson);
            }
            else
                MessageBox.Show("No has selecccionado archivo para cargar.","Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cargarPeliculas(String archivo)
        {
            try
            {
                using (StreamReader jsonStream = File.OpenText(archivo))
                {
                    var json = jsonStream.ReadToEnd();
                    List<Pelicula> peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(json);

                    listaPeliculas.Clear();
                    foreach (Pelicula p in peliculas)
                    {
                        listaPeliculas.Add(p);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Debes elegir un archivo Json.", "Archivo incorrecto", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void eliminarButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Remove((Pelicula)PeliculasListBox.SelectedItem);
            MessageBox.Show("Película eliminada con éxito.","Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void añadirButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Add(new Pelicula());
            MessageBox.Show("Nueva película lista para editar.","Información", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void examinarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string ruta = Directory.GetCurrentDirectory();
            openFileDialog.InitialDirectory = ruta.Replace("\\bin\\Debug",rutaImagenes);
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                imagenTextBox.Text = rutaImagenes+openFileDialog.SafeFileName;
        }

        private void nuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            puntuacionTextBox.Text = "0";
            if (listaPeliculas.Count >= 5)
            {
                Random rng = new Random();
                Pelicula peliculaRandom;
                peliculasAleatorias = new ObservableCollection<Pelicula>();
                guardarPistas = new List<bool>();
                guardarRespuestas = new List<string>();
                do
                {
                    peliculaRandom = listaPeliculas[rng.Next(0, listaPeliculas.Count)];
                    if (!peliculasAleatorias.Contains(peliculaRandom))
                    {
                        peliculasAleatorias.Add(peliculaRandom);
                        guardarPistas.Add(false);
                        guardarRespuestas.Add("");
                    }
                } while (peliculasAleatorias.Count != 5);
                Indice = 0;
                PeliculaActual();
                HabilitarBotones();
                ComprobarDatos();
                MessageBox.Show("Empezando nueva partida.", "Inicio de partida", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Necesitas al menos 5 películas para poder jugar.", "Faltan películas", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cambiarPeliculaButton_Click(object sender, RoutedEventArgs e)
        {
            string flecha = ((Button)sender).Tag.ToString();
            GuardarDatos();

            if (flecha == "-1" && jugarDockPanel.DataContext != peliculasAleatorias[0])
            {
                Indice--;
                PeliculaActual();
                ComprobarDatos();
            }
            else if (flecha == "1" && jugarDockPanel.DataContext != peliculasAleatorias[peliculasAleatorias.Count - 1])
            {
                Indice++;
                PeliculaActual();
                ComprobarDatos();
            }
        }

        private void validarButton_Click(object sender, RoutedEventArgs e)
        {
            if (respuestaTextBox.Text.Equals(peliculasAleatorias[Indice].Nombre, StringComparison.CurrentCultureIgnoreCase))
            {
                GuardarDatos();
                ComprobarDatos();
                MessageBox.Show("Respuesta Correcta!", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                SumarPuntos();
            }
            else if(respuestaTextBox.Text != "")
            {
                respuestaTextBox.Background = Brushes.Red;
                respuestaTextBox.Foreground = Brushes.White;
                MessageBox.Show("Respuesta Incorrecta!", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SumarPuntos()
        {
            string dificultad = peliculasAleatorias[Indice].Dificultad;
            int puntuacion = int.Parse(puntuacionTextBox.Text);
            if (dificultad == "Facil")
            {
                if (pistaCheckBox.IsChecked == true)
                    puntuacionTextBox.Text = (puntuacion + 5).ToString();
                else
                    puntuacionTextBox.Text = (puntuacion + 10).ToString();
            }
            else if(dificultad == "Normal")
            {
                if (pistaCheckBox.IsChecked == true)
                    puntuacionTextBox.Text = (puntuacion + 15).ToString();
                else
                    puntuacionTextBox.Text = (puntuacion + 30).ToString();
            }
            else
            {
                if (pistaCheckBox.IsChecked == true)
                    puntuacionTextBox.Text = (puntuacion + 25).ToString();
                else
                    puntuacionTextBox.Text = (puntuacion + 50).ToString();
            }
        }

        private void PeliculaActual()
        {
            peliculaActualTextBlock.Text = (Indice + 1) + "/" + peliculasAleatorias.Count;
        }

        private void HabilitarBotones()
        {
            retrocederButton.IsEnabled = true;
            avanzarButton.IsEnabled = true;
            validarButton.IsEnabled = true;
            pistaCheckBox.IsEnabled = true;
            respuestaTextBox.IsEnabled = true;
        }

        private void GuardarDatos()
        {
            if (pistaCheckBox.IsChecked==true)
            {
                guardarPistas[Indice] = true;
            }
            guardarRespuestas[Indice] = respuestaTextBox.Text;
        }

        private void ComprobarDatos()
        {
            pistaCheckBox.IsChecked = guardarPistas[Indice];
            respuestaTextBox.Text = guardarRespuestas[Indice];
            if(respuestaTextBox.Text.Equals(peliculasAleatorias[Indice].Nombre, StringComparison.CurrentCultureIgnoreCase))
            {
                respuestaTextBox.Background = Brushes.Green;
                respuestaTextBox.Foreground = Brushes.White;
                respuestaTextBox.IsReadOnly = true;
                validarButton.IsEnabled = false;
                pistaCheckBox.IsEnabled = false;
            }
            else
            {
                respuestaTextBox.Background = Brushes.White;
                respuestaTextBox.Foreground = Brushes.Black;
                respuestaTextBox.IsReadOnly = false;
                validarButton.IsEnabled = true;
                pistaCheckBox.IsEnabled = true;
            }
        }
    }
}
