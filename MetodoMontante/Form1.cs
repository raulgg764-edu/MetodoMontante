using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetodoMontante
{
    public partial class Form1 : Form
    {
        /*Author: raulgg764*/
        public Form1()
        {
            InitializeComponent();
        }

        private void btnNumEcuaciones_Click(object sender, EventArgs e)
        {
            int numEcuaciones = (int)(nupNumEcuaciones.Value);
            CreateTable(numEcuaciones);
            btnResults.Enabled = true;
        }

        private void CreateTable(int num)
        {
            DataGridView table = dataGridView1;

            dataGridView1.Columns.Clear();

            for (int i = 0; i < num; i++)
            {
                table.Columns.Add("Column" + (i + 1), "X"+(i+1));
                if(i==num-1) 
                table.Columns.Add("ColumnB", "B");
            }
            for (int i = 0; i < num; i++)
            {
                table.Rows.Add();
            } 
            
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            int rows = dataGridView1.Rows.Count;
            int cols = dataGridView1.Columns.Count;

            int[,] numbers = new int[rows,cols-1];
            int[] results = new int[rows];

            //get matrix and results
            try
            { //matrix
                Clear(false);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        if (j < cols - 1)
                        {
                            numbers[i, j] = int.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                            //Console.WriteLine(numbers[i, j]);
                        }
                        else if (j == (cols - 1))
                        {
                            results[i] = int.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                            //Console.WriteLine(results[i] + "res");
                        }
                    }
                }
                MontanteAlgorithm(numbers, results);
            }
            catch
            {
               MessageBox.Show("La matriz tiene un espacio vacio o tiene un carácter no valido","¡Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
            }
           
        }

        private void MontanteAlgorithm(int[,] numbers, int[] results)
        {
            int rows = dataGridView1.Rows.Count;
            int cols = dataGridView1.Columns.Count-1;

            double[] finalResults = new double[rows];
            int pivAnt = 1;

            int[,] adjunctNumbers = numbers;

            int[,] adjunct = IdentityMatrix();

            int[,] identity = IdentityMatrix();

            for(int piv = 0; piv < rows; piv++)
            {
                for (int i=0; i < rows; i++)
                {
             
                    for (int j=0; j < cols; j++)
                    {

                        if (i!=piv)
                        {
                            if (piv == 0)
                            {
                                adjunct[i, j] = (adjunctNumbers[piv, piv] * adjunct[i, j] - adjunctNumbers[i, piv] * adjunct[piv, j]) / pivAnt;
                            }
                            else
                            {
                                adjunct[i, j] = (adjunctNumbers[piv, piv] * adjunct[i, j] - adjunctNumbers[i, piv] * adjunct[piv, j]) / pivAnt;
                            }      
                        }
                                
                        if (i != piv && j != piv)
                        {
                            numbers[i, j] = (numbers[piv, piv] * numbers[i, j] - numbers[i, piv] * numbers[piv, j]) / pivAnt;
                            
                        }
                        
                    }
                    if (i != piv){
                        numbers[i, piv] = 0;
                    }
                    
                }
                adjunctNumbers = numbers;
                pivAnt = numbers[piv, piv];
            }

            for(int i = 0; i < rows; i++)
            {
                
                for(int j = 0; j < cols; j++)
                {
                    finalResults[i] = finalResults[i] + (results[j] * ((double)adjunct[i, j] / (double)pivAnt));
                    Console.Write(adjunct[i, j]+" ");
                }
                Console.WriteLine();
                Console.WriteLine("x"+i+"="+finalResults[i]);
                Console.WriteLine("piv:" + pivAnt);
            }

            double[,] inverse = Inverse(adjunct, pivAnt);
            ShowResults(finalResults, inverse,adjunct, pivAnt);
        }
        
        private int[,] IdentityMatrix()
        {

            int[,] identity = new int[dataGridView1.Rows.Count, dataGridView1.Columns.Count - 1];

            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for(int j=0; j<dataGridView1.Columns.Count - 1; j++)
                {
                    identity[i, j] = i == j ? 1 : 0;
                    //Console.WriteLine(identity[i, j]);
                }
            }

            return identity;
        }

        private double[,] Inverse(int [,] matrix,int piv)
        {
            int rows = dataGridView1.Rows.Count;
            int cols = dataGridView1.Columns.Count - 1;

            double[,] inverse = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    inverse[i,j] = (double)matrix[i,j] / (double)piv;
                }
            }
            return inverse;
        }

        private void ShowResults(double[] results, double[,] inverse, int [,]adjunct, int piv)
        {
            DataGridView inverseTable = dataGridViewInverse;
            DataGridView resultsTable = dataGridViewValues;
            DataGridView adjunctTable = dataGridViewAdjunct;
            int inverseRows = dataGridView1.Rows.Count, inverseCols = dataGridView1.Rows.Count, resultsRows = dataGridView1.Rows.Count; 
            int adjunctRows=dataGridView1.Rows.Count, adjunctCols = dataGridView1.Rows.Count;



            //inversa
            inverseTable.ColumnCount = inverseCols;
            for (int i = 0; i<inverseRows;i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(inverseTable);

                for (int j = 0; j < inverseCols; j++)
                {
                    row.Cells[j].Value = inverse[i, j];
                }

                inverseTable.Rows.Add(row);
            }

            //adjunta
            adjunctTable.ColumnCount = adjunctCols;
            for (int i = 0; i < adjunctRows; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(adjunctTable);

                for (int j = 0; j < adjunctCols; j++)
                {
                    row.Cells[j].Value = adjunct[i, j];
                }

                adjunctTable.Rows.Add(row);
            }

            //Determinante
            lblDeterm.Text = piv.ToString();

            //resultados
            resultsTable.Columns.Add("Variable", "Var");
            resultsTable.Columns.Add("Results", "Resultado");

            for (int i=0;i<resultsRows;i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(resultsTable);
                row.Cells[0].Value = "X" + (i+1);
                row.Cells[1].Value = results[i];

                resultsTable.Rows.Add(row);
            }

            adjunctTable.ClearSelection();
            inverseTable.ClearSelection();
            resultsTable.ClearSelection();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtNumEcuaciones_KeyPress(object sender, KeyPressEventArgs e)
        {

          
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int cols = dataGridView1.Columns.Count;
            for (int i=0;i<cols;i++)
            {
                if (dataGridView1.CurrentCell.ColumnIndex == i)
                {
                    e.Control.KeyPress += new KeyPressEventHandler(dataGridView1_KeyPress);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear(true);
        }

        private void Clear(bool cleanMatrix)
        {
            int cols = dataGridView1.Columns.Count;
            int rows = dataGridView1.Rows.Count;

            if (cleanMatrix == true)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        dataGridView1.Rows[i].Cells[i].Value = "";
                    }
                }
            }
            dataGridViewAdjunct.Columns.Clear();
            dataGridViewAdjunct.Rows.Clear();

            dataGridViewInverse.Columns.Clear();
            dataGridViewInverse.Rows.Clear();

            dataGridViewValues.Columns.Clear();
            dataGridViewValues.Rows.Clear();

            lblDeterm.Text = "";
        }
    }
}
