﻿using System;
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
        List<string> listaGenerosComboBox;
        List<string> listaDificultadComboBox;
        List<Pelicula> peliculasAleatorias;
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
                MessageBox.Show("No has selecccionado archivo para guardar.");
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
                MessageBox.Show("No has selecccionado archivo para cargar.");
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
                MessageBox.Show("Debes elegir un archivo Json.");
            }
        }

        private void eliminarButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Remove((Pelicula)PeliculasListBox.SelectedItem);
            MessageBox.Show("Película eliminada.");
        }

        private void añadirButton_Click(object sender, RoutedEventArgs e)
        {
            listaPeliculas.Add(new Pelicula());
            MessageBox.Show("Nueva película lista para editar.");
        }

        private void examinarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                imagenTextBox.Text = rutaImagenes+openFileDialog.SafeFileName;
        }

        private void nuevaPartidaButton_Click(object sender, RoutedEventArgs e)
        {
            Random rng = new Random();
            Pelicula peliculaRandom;
            peliculasAleatorias = new List<Pelicula>();
            do
            {
                peliculaRandom = listaPeliculas[rng.Next(0, listaPeliculas.Count)];
                if(!peliculasAleatorias.Contains(peliculaRandom))
                {
                    peliculasAleatorias.Add(peliculaRandom);
                }
            } while (peliculasAleatorias.Count != 5);
            Indice = 0;
            PeliculaActual();
            HabilitarBotones();
        }

        private void cambiarPeliculaButton_Click(object sender, RoutedEventArgs e)
        {
            string flecha = ((Button)sender).Tag.ToString();

            if (flecha == "-1" && jugarDockPanel.DataContext != peliculasAleatorias[0])
            {
                Indice--;
                PeliculaActual();
                LimpiarDatos();
            }
            else if (flecha == "1" && jugarDockPanel.DataContext != peliculasAleatorias[peliculasAleatorias.Count - 1])
            {
                Indice++;
                PeliculaActual();
                LimpiarDatos();
            }
        }

        private void validarButton_Click(object sender, RoutedEventArgs e)
        {
            if (respuestaTextBox.Text.Equals(peliculasAleatorias[Indice].Nombre, StringComparison.CurrentCultureIgnoreCase))
            {
                respuestaTextBox.Background = Brushes.Green;
                MessageBox.Show("Respuesta Correcta!");
                SumarPuntos();
            }
            else
            {
                respuestaTextBox.Background = Brushes.Red;
                MessageBox.Show("Respuesta Incorrecta!");
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
        }

        private void LimpiarDatos()
        {
            respuestaTextBox.Text = "";
            respuestaTextBox.Background = Brushes.White;
        }
    }
}
