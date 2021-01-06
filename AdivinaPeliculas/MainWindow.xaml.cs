using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

using Newtonsoft.Json;
using Microsoft.Win32;

namespace AdivinaPeliculas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Pelicula> listaPeliculas;
        List<string> listaGenerosComboBox;
        String archivoJson;
        public MainWindow()
        {
            InitializeComponent();
            listaGenerosComboBox = new List<string> { "Comedia", "Drama", "Accion", "Terror", "Sci-fi" };

            listaPeliculas = new ObservableCollection<Pelicula>();

            PeliculasListBox.DataContext = listaPeliculas;
            generoPeliculaComboBox.ItemsSource = listaGenerosComboBox;
        }

        private void guardarButton_Click(object sender, RoutedEventArgs e)
        {
            string peliculasJson = JsonConvert.SerializeObject(listaPeliculas);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json;)|*.json;|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, peliculasJson);
        }

        private void cargarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json;)|*.json;|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                archivoJson = openFileDialog.FileName;

            listaPeliculas.Clear();
            using (StreamReader jsonStream = File.OpenText(archivoJson))
            {
                var json = jsonStream.ReadToEnd();
                List<Pelicula> peliculas = JsonConvert.DeserializeObject<List<Pelicula>>(json);

                foreach (Pelicula p in peliculas)
                {
                    listaPeliculas.Add(p);
                }
            }
        }

        private void eliminarButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Remove((Pelicula)PeliculasListBox.SelectedItem);
        }

        private void añadirButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Add(new Pelicula());
        }

        private void examinarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                imagenTextBox.Text = openFileDialog.FileName;
        }
    }
}
