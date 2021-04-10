using System;
using jra.ListaPuntos;
using jra.objListaOrdenada;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jra
{
    public partial class Form1 : Form
    {
        bool reproduce = false;
        OpenFileDialog buscar = new OpenFileDialog();
        ListaOrdenada add = new ListaOrdenada();

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            buscar.Multiselect = true; //Esto va a permitir seleccionar varios archivos al mismo tiempo

            //Este if se encarga de abrir la ventana
            if (buscar.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //ArchivosMP3 = CajaDeBusquedaDeArchivos.SafeFileNames;//Aqui se van a almacenar todos los archivos

                //Para recorrer dependiendo de la cantidad de canciones que haya ingresado 
                for (int i = 0; i < buscar.FileNames.Length; i++)
                //FileNames para devolver la extensión de la ruta de acceso del archivo
                {
                    add.insertaOrden(buscar.FileNames[i]); //Para insertarlas a la lista

                    //Para insertarlas al listbox
                    listBox1.Items.Add(buscar.SafeFileNames[i]);
                    //SafeFileNames para obtener el nombre y la extensión del archivo seleccionado
                }


                Reproductor.URL = buscar.FileNames[0];
                listBox1.SelectedIndex = 0; //SelectedIndex para obtener el índice del elemento seleccionado
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Insertar las canciones al listbox
            //Va a ejecutarse mientras selectedIndex sea diferente a -1
            if (listBox1.SelectedIndex != -1)
            {
                //Esto va a hacer que cuando el usuario seleccione de la lista de reproduccion automaticamente se reproduzca en windows media player
                Reproductor.URL = buscar.FileNames[listBox1.SelectedIndex];

            }
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            int eliminar = listBox1.SelectedIndex; //Para tomar la posicion a eliminar

            if (listBox1.SelectedIndex != -1)
            {
                add.eliminar(eliminar);
                listBox1.Items.RemoveAt(eliminar); //Para eliminar lo que este en la posicion 
                Reproductor.Ctlcontrols.stop();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            switch (reproduce)
            {
                case true:
                    Reproductor.Ctlcontrols.pause();
                    pictureBox2.Image = Properties.Resources.play;
                    reproduce = false;
                    break;
                case false:
                    Reproductor.Ctlcontrols.play();
                    pictureBox2.Image = Properties.Resources.pausa;
                    reproduce = true;
                    break;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
            }
        }


        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Reproductor.Ctlcontrols.stop();
            pictureBox2.Image = Properties.Resources.play;
            reproduce = false;
        }

        public void Actualizar()
        {
            if (Reproductor.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                Status.Maximum = (int)Reproductor.Ctlcontrols.currentItem.duration;
                timer1.Start();
            }
            else if (Reproductor.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer1.Stop();
            }
            else if (Reproductor.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer1.Stop();
                Status.Value = 0;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Actualizar();
            Status.Value = (int)Reproductor.Ctlcontrols.currentPosition;
            volumen.Value = Reproductor.settings.volume;
        }

        private void Reproductor_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            Actualizar();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
            }
        }
    }
}
