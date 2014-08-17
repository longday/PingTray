namespace PingTray
{
    partial class PingTrayForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerPing = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.pingAdres = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPingHistory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // timerPing
            // 
            this.timerPing.Enabled = true;
            this.timerPing.Interval = 500;
            this.timerPing.Tick += new System.EventHandler(this.timerPing_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Адрес для пинга(сереет если изменен и не применен):";
            // 
            // pingAdres
            // 
            this.pingAdres.Location = new System.Drawing.Point(301, 10);
            this.pingAdres.Name = "pingAdres";
            this.pingAdres.Size = new System.Drawing.Size(320, 20);
            this.pingAdres.TabIndex = 1;
            this.pingAdres.TextChanged += new System.EventHandler(this.pingAdres_TextChanged);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(627, 7);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(76, 24);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(687, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Параметры командной строки(порядок любой, наличие не обязательно): /ip <адрес или" +
    " url> /color <цвет строкой (blue,lightgreen, etc)>";
            // 
            // tbPingHistory
            // 
            this.tbPingHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPingHistory.Location = new System.Drawing.Point(16, 70);
            this.tbPingHistory.Multiline = true;
            this.tbPingHistory.Name = "tbPingHistory";
            this.tbPingHistory.Size = new System.Drawing.Size(687, 285);
            this.tbPingHistory.TabIndex = 4;
            // 
            // PingTrayForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 367);
            this.Controls.Add(this.tbPingHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.pingAdres);
            this.Controls.Add(this.label1);
            this.Name = "PingTrayForm";
            this.Text = "Ping Tray";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerPing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pingAdres;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPingHistory;
    }
}

