namespace OCR
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inserirePaginaSinistraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inserirePaginaDestraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlloPagineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rilevaAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eseguiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continuaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verificaLeNuovePagineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.riprendiControlloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.picOriginalSx = new System.Windows.Forms.PictureBox();
            this.picOriginalDx = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalSx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalDx)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.controlloPagineToolStripMenuItem,
            this.continuaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inserirePaginaSinistraToolStripMenuItem,
            this.inserirePaginaDestraToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // inserirePaginaSinistraToolStripMenuItem
            // 
            this.inserirePaginaSinistraToolStripMenuItem.Name = "inserirePaginaSinistraToolStripMenuItem";
            this.inserirePaginaSinistraToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.inserirePaginaSinistraToolStripMenuItem.Text = "Inserire Pagina Sinistra";
            this.inserirePaginaSinistraToolStripMenuItem.Click += new System.EventHandler(this.inserirePaginaSinistraToolStripMenuItem_Click);
            // 
            // inserirePaginaDestraToolStripMenuItem
            // 
            this.inserirePaginaDestraToolStripMenuItem.Name = "inserirePaginaDestraToolStripMenuItem";
            this.inserirePaginaDestraToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.inserirePaginaDestraToolStripMenuItem.Text = "Inserire Pagina Destra";
            this.inserirePaginaDestraToolStripMenuItem.Click += new System.EventHandler(this.inserirePaginaDestraToolStripMenuItem_Click);
            // 
            // controlloPagineToolStripMenuItem
            // 
            this.controlloPagineToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rilevaAreaToolStripMenuItem,
            this.eseguiToolStripMenuItem});
            this.controlloPagineToolStripMenuItem.Name = "controlloPagineToolStripMenuItem";
            this.controlloPagineToolStripMenuItem.Size = new System.Drawing.Size(108, 20);
            this.controlloPagineToolStripMenuItem.Text = "Controllo Pagine";
            // 
            // rilevaAreaToolStripMenuItem
            // 
            this.rilevaAreaToolStripMenuItem.Name = "rilevaAreaToolStripMenuItem";
            this.rilevaAreaToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.rilevaAreaToolStripMenuItem.Text = "Rileva Area ";
            this.rilevaAreaToolStripMenuItem.Click += new System.EventHandler(this.rilevaAreaToolStripMenuItem_Click);
            // 
            // eseguiToolStripMenuItem
            // 
            this.eseguiToolStripMenuItem.Name = "eseguiToolStripMenuItem";
            this.eseguiToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.eseguiToolStripMenuItem.Text = "Esegui";
            this.eseguiToolStripMenuItem.Click += new System.EventHandler(this.eseguiToolStripMenuItem_Click);
            // 
            // continuaToolStripMenuItem
            // 
            this.continuaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verificaLeNuovePagineToolStripMenuItem,
            this.riprendiControlloToolStripMenuItem});
            this.continuaToolStripMenuItem.Name = "continuaToolStripMenuItem";
            this.continuaToolStripMenuItem.Size = new System.Drawing.Size(159, 20);
            this.continuaToolStripMenuItem.Text = "Continua/Modifica Pagina";
            // 
            // verificaLeNuovePagineToolStripMenuItem
            // 
            this.verificaLeNuovePagineToolStripMenuItem.Name = "verificaLeNuovePagineToolStripMenuItem";
            this.verificaLeNuovePagineToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.verificaLeNuovePagineToolStripMenuItem.Text = "Verifica le nuove pagine";
            this.verificaLeNuovePagineToolStripMenuItem.Click += new System.EventHandler(this.verificaLeNuovePagineToolStripMenuItem_Click);
            // 
            // riprendiControlloToolStripMenuItem
            // 
            this.riprendiControlloToolStripMenuItem.Name = "riprendiControlloToolStripMenuItem";
            this.riprendiControlloToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.riprendiControlloToolStripMenuItem.Text = "Riprendi controllo";
            this.riprendiControlloToolStripMenuItem.Click += new System.EventHandler(this.riprendiControlloToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.picOriginalSx, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.picOriginalDx, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 426);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // picOriginalSx
            // 
            this.picOriginalSx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picOriginalSx.Location = new System.Drawing.Point(3, 3);
            this.picOriginalSx.Name = "picOriginalSx";
            this.picOriginalSx.Size = new System.Drawing.Size(394, 420);
            this.picOriginalSx.TabIndex = 0;
            this.picOriginalSx.TabStop = false;
            this.picOriginalSx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picOriginalSx_MouseDown);
            this.picOriginalSx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picOriginalSx_MouseMove);
            this.picOriginalSx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picOriginalSx_MouseUp);
            // 
            // picOriginalDx
            // 
            this.picOriginalDx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picOriginalDx.Location = new System.Drawing.Point(403, 3);
            this.picOriginalDx.Name = "picOriginalDx";
            this.picOriginalDx.Size = new System.Drawing.Size(394, 420);
            this.picOriginalDx.TabIndex = 1;
            this.picOriginalDx.TabStop = false;
            this.picOriginalDx.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picOriginalDx_MouseDown);
            this.picOriginalDx.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picOriginalDx_MouseMove);
            this.picOriginalDx.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picOriginalDx_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalSx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalDx)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inserirePaginaSinistraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inserirePaginaDestraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlloPagineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eseguiToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox picOriginalSx;
        private System.Windows.Forms.PictureBox picOriginalDx;
        private System.Windows.Forms.ToolStripMenuItem continuaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rilevaAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verificaLeNuovePagineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem riprendiControlloToolStripMenuItem;
    }
}

