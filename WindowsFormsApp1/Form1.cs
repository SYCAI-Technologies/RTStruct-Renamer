using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FellowOakDicom;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        String fileName;
        DicomFile file;

        public Form1()
        {
            InitializeComponent();
        }

        private void toggleGuiVisibility(Boolean status)
        {
            label1.Visible = status;
            textBox1.Visible = status;
            label2.Visible = status;
            textBox2.Visible = status;
            label4.Visible = status;
            textBox3.Visible = status;
            button1.Enabled = status;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toggleGuiVisibility(false);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Choose RTStruct File",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "dcm",
                Filter = "dcm files (*.dcm)|*.dcm | All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                fileName = openFileDialog1.FileName;

                file = DicomFile.Open(fileName);
                Console.WriteLine(fileName);
                
                try
                {
                    var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);
                    label3.Text = file.Dataset.GetString(DicomTag.PatientID);
                    
                    var texts = new List<TextBox> { textBox1, textBox2, textBox3 };
                    int c = 0;
                    foreach (var sequence in roiSequence)
                    {
                        
                        var roiName = sequence.GetString(DicomTag.ROIName);
                        Console.WriteLine(roiName);
                        texts.ElementAt(c).Text = roiName;
                        c++;
                    }

                    toggleGuiVisibility(true);
                    Console.WriteLine("Sequence Length: " + roiSequence.Count());
                    if (roiSequence.Count() == 2)
                    {
                        label4.Visible = false;
                        textBox3.Visible = false;
                    }
                    if (roiSequence.Count() == 1)
                    {
                        label2.Visible = false;
                        textBox2.Visible = false;
                    }
                }
                catch (FellowOakDicom.DicomDataException)
                {
                    MessageBox.Show("Please make sure to select a valid RTStruct file.", "File is not valid",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //Save button
        private void button1_Click(object sender, EventArgs e)
        {
            var roiSequence = file.Dataset.GetSequence(DicomTag.StructureSetROISequence);

            var texts = new List<TextBox> { textBox1, textBox2, textBox3 };
            int c = 0;
            foreach (var sequence in roiSequence)
            {
                sequence.AddOrUpdate(DicomTag.ROIName, texts.ElementAt(c).Text);
                c++;
            }

            var roiObservation = file.Dataset.GetSequence(DicomTag.RTROIObservationsSequence);
            c = 0;
            foreach (var sequence in roiObservation)
            {
                sequence.AddOrUpdate(DicomTag.ROIObservationLabel, texts.ElementAt(c).Text);
                c++;
            }


            file.Save(fileName);
            MessageBox.Show("File saved successfully.", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
