namespace ParkingManager
{
    partial class ParkingManager
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCaminho = new System.Windows.Forms.Button();
            this.btnSentido = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCaminho
            // 
            this.btnCaminho.Location = new System.Drawing.Point(160, 12);
            this.btnCaminho.Name = "btnCaminho";
            this.btnCaminho.Size = new System.Drawing.Size(75, 23);
            this.btnCaminho.TabIndex = 1;
            this.btnCaminho.Text = "*";
            this.btnCaminho.UseVisualStyleBackColor = true;
            // 
            // btnSentido
            // 
            this.btnSentido.Location = new System.Drawing.Point(308, 12);
            this.btnSentido.Name = "btnSentido";
            this.btnSentido.Size = new System.Drawing.Size(75, 23);
            this.btnSentido.TabIndex = 2;
            this.btnSentido.Text = "->";
            this.btnSentido.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "=";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ParkingManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 341);
            this.Controls.Add(this.btnSentido);
            this.Controls.Add(this.btnCaminho);
            this.Controls.Add(this.button1);
            this.Name = "ParkingManager";
            this.Text = "Parking Manager";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnCaminho;
        private System.Windows.Forms.Button btnSentido;
    }
}

