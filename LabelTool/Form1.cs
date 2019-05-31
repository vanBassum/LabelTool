using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Datasave;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Forms;
using System.Reflection;
using Silver.UI;
using System.IO.Compression;

namespace LabelTool
{
    
    public partial class Form1 : Form
    {
        string openedFile = "";
        LabelPage pageSettings;
        SaveableBindingList<LabelItem> labelItems = new SaveableBindingList<LabelItem>() { };
        public Form1()
        {
            InitializeComponent();
            toolBox1.BackColor = System.Drawing.SystemColors.Control;
            toolBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            toolBox1.TabHeight = 18;
            toolBox1.ItemHeight = 20;
            toolBox1.ItemSpacing = 1;
            toolBox1.ItemHoverColor = System.Drawing.Color.BurlyWood;
            toolBox1.ItemNormalColor = System.Drawing.SystemColors.Control;
            toolBox1.ItemSelectedColor = System.Drawing.Color.Linen;
            //toolBox1.Name = "toolBox1";
            //toolBox1.Size = new System.Drawing.Size(208, 405);
            //toolBox1.Location = new System.Drawing.Point(0, 0);
            toolBox1.AddTab("New Item");
            toolBox1.ItemMouseDown += ToolBox1_ItemMouseDown;
            //toolBox1.AddTab("Items");

            Type iface = typeof(LabelItem);
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && iface.IsAssignableFrom(t)
                    select t;

            foreach (Type t in q)
                toolBox1[0].AddItem(t.Name, -1, true, t);

            comboBox1.DataSource = labelItems;
        }

        private void ToolBox1_ItemMouseDown(ToolBoxItem sender, MouseEventArgs e)
        {
            labelItems.Add(Activator.CreateInstance(sender.Object as Type) as LabelItem);
            labelItems[labelItems.Count - 1].Name += labelItems.Count.ToString();
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
            propertyGrid2.SelectedObject = labelItems[labelItems.Count - 1];
            pagePreview1.Refresh(); 
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            //PdfDocument document = new PdfDocument();
            //document.Info.Title = "test";
            //PdfPage page = document.AddPage();
            //XGraphics gfx = XGraphics.FromPdfPage(page);
            pageSettings = new LabelPage();
            LoadDefaultSettings();
            pageSettings.PropertyChanged += PageSettings_PropertyChanged;
            pagePreview1.SetRenderFunction((gfx) => Render(gfx));
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            propertyGrid1.SelectedObject = pageSettings;
        }


        void LoadDefaultSettings()
        {
            pageSettings.PageSize = PageSize.A4;
            pageSettings.LabelSize = new SizeF(63.5f, 29.6f);
            pageSettings.FirstLabel = new PointF(7.11200f, 15.24f);
            pageSettings.Corner = new SizeF(7f, 7f);
            pageSettings.Spacing = new SizeF(2.54f, 0);
            pageSettings.LabelsX = 3;
            pageSettings.LabelsY = 27 / 3;
        }
        private void PageSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            pagePreview1.Refresh();
        }

        

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    pagePreview1.PageSize =PageSizeConverter.ToSize( pageSettings.PageSize);
                    propertyGrid1.SelectedObject = null;
                    RefreshTable();
                    break;
                case 1:
                    pagePreview1.PageSize = new XSize(XUnit.FromMillimeter(pageSettings.LabelSize.Width), XUnit.FromMillimeter(pageSettings.LabelSize.Height));
                    break;
                case 2:
                    pagePreview1.PageSize = PageSizeConverter.ToSize(pageSettings.PageSize);
                    break;
            }
            pagePreview1.Refresh();
        }

        void RefreshTable()
        {
            int i;

            for (i = 0; i < labelItems.Count; i++)
            {
                if (!dataGridView1.Columns.Contains(labelItems[i].VarName))
                    dataGridView1.Columns.Add(labelItems[i].VarName, labelItems[i].VarName);
            }

            for (i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (labelItems.Where(l => l.VarName == dataGridView1.Columns[i].Name).Count() == 0)
                {
                    dataGridView1.Columns.RemoveAt(i);
                    i--;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid2.SelectedObject = comboBox1.SelectedItem;
        }

        

        void Save(string filePath)
        {
            using (FileStream zipToOpen = new FileStream(filePath, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    pageSettings.Save(archive.CreateEntry("pageSettings.json").Open());
                    labelItems.Save(archive.CreateEntry("labelItems.json").Open());
                }
            }

            openedFile = filePath;
            this.Text = filePath;
        }

        void Open(string filePath)
        {
            using (FileStream zipToOpen = new FileStream(filePath, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    pageSettings.Load<LabelPage>(archive.GetEntry("pageSettings.json").Open());
                    labelItems.Load(archive.GetEntry("labelItems.json").Open());
                }
            }

            
            openedFile = filePath;
            this.Text = filePath;
            RefreshTable();
            pagePreview1.Refresh();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.InitialDirectory = Directory.GetCurrentDirectory();
            diag.OverwritePrompt = true;
            diag.Filter = "Label files (*.lbl)|*.lbl";
            if (openedFile == "")
                diag.FileName = "New label";
            else
                diag.FileName = openedFile;

            if (diag.ShowDialog() == DialogResult.OK)
            {
                Save(diag.FileName);
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openedFile != "")
            {
                Save(openedFile);
            }
            else
            {
                SaveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog diag = new OpenFileDialog();
            diag.InitialDirectory = Directory.GetCurrentDirectory();
            diag.Filter = "Label files (*.lbl)|*.lbl";
            diag.Multiselect = false;
            if (diag.ShowDialog() == DialogResult.OK)
            {
                Open(diag.FileName);
            }

            

        }

        private void PropertyGrid2_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            pagePreview1.Refresh();
        }

        private void Render(XGraphics gfx)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    RenderGrid(gfx);
                    RenderTotal(gfx);
                    break;
                case 1:
                    RenderLabel(gfx);
                    break;
                case 2:
                    RenderGrid(gfx);
                    break;
            }
        }


        private void RenderLabel(XGraphics gfx)
        {
            XSize lSize = new XSize(XUnit.FromMillimeter(pageSettings.LabelSize.Width), XUnit.FromMillimeter(pageSettings.LabelSize.Height));
            XPoint lPt = new XPoint(0, 0);
            //Just draw each side for all labels, dont care if they overlap...
            XRect rect = new XRect(lPt, lSize);
            gfx.DrawRoundedRectangle(XPens.Black, rect, new XSize(XUnit.FromMillimeter(pageSettings.Corner.Width), XUnit.FromMillimeter(pageSettings.Corner.Height)));

            foreach (LabelItem item in labelItems)
                item.Render(gfx, lPt);
        }

        private void RenderGrid(XGraphics gfx)
        {
            int x, y;

            //Create the grid
            for (x = 0; x < pageSettings.LabelsX; x++)
            {
                for (y = 0; y < pageSettings.LabelsY; y++)
                {
                    XSize sSize = new XSize(XUnit.FromMillimeter(pageSettings.Spacing.Width), XUnit.FromMillimeter(pageSettings.Spacing.Height));
                    XSize lSize = new XSize(XUnit.FromMillimeter(pageSettings.LabelSize.Width), XUnit.FromMillimeter(pageSettings.LabelSize.Height));
                    XPoint lPt = new XPoint(XUnit.FromMillimeter(pageSettings.FirstLabel.X), XUnit.FromMillimeter(pageSettings.FirstLabel.Y));
                    XPoint pt = new XPoint(x * (lSize.Width + sSize.Width) + lPt.X, y * (lSize.Height + sSize.Height) + lPt.Y);

                    //Just draw each side for all labels, dont care if they overlap...
                    XRect rect = new XRect(pt, lSize);
                    gfx.DrawRoundedRectangle(XPens.Black, rect, new XSize(XUnit.FromMillimeter(pageSettings.Corner.Width), XUnit.FromMillimeter(pageSettings.Corner.Height)));

                    //foreach (LabelItem item in labelItems)
                    //    item.Render(gfx, pt);
                }
            }
        }

        private void RenderTotal(XGraphics gfx)
        {
            int x, y;

            //Create the grid
            int i;

            for (i = 0; i < dataGridView1.RowCount; i++)
            {
                x = i % pageSettings.LabelsX;
                y = i / pageSettings.LabelsX;

                foreach (LabelItem item in labelItems)
                {
                    try
                    {
                        XSize sSize = new XSize(pageSettings.Spacing.Width,     pageSettings.Spacing.Height);
                        XSize lSize = new XSize(pageSettings.LabelSize.Width,   pageSettings.LabelSize.Height);
                        XPoint lPt = new XPoint(pageSettings.FirstLabel.X,      pageSettings.FirstLabel.Y);
                        XPoint pt = new XPoint(x * (lSize.Width + sSize.Width) + lPt.X, y * (lSize.Height + sSize.Height) + lPt.Y);
                        item.Render(gfx, pt, dataGridView1.Rows[i].Cells[item.VarName].Value as string);
                    }
                    catch
                    {

                    }
                }

            }

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            pagePreview1.Refresh();
        }
    }


    public class LabelPage : SettingsExtention
    {
        public PageSize PageSize { get => GetPar<PageSize>(); set => SetPar(value); }
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PointF FirstLabel { get => GetPar<PointF>(); set => SetPar(value); }
        public SizeF LabelSize { get => GetPar<SizeF>(); set => SetPar(value); }
        public SizeF Corner { get => GetPar<SizeF>(); set => SetPar(value); }
        public SizeF Spacing { get => GetPar<SizeF>(); set => SetPar(value); }
        public int LabelsX { get => GetPar<int>(); set => SetPar(value); }
        public int LabelsY { get => GetPar<int>(); set => SetPar(value); }
    }

}
