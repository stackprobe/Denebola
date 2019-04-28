namespace Charlotte
{
	partial class MainWin
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
			this.MainTimer = new System.Windows.Forms.Timer(this.components);
			this.MapPicture = new System.Windows.Forms.PictureBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.Status = new System.Windows.Forms.ToolStripStatusLabel();
			this.ErrorStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.MapPanelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.ChangeMeterPerLatLonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.MapPicture)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.MapPanelMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTimer
			// 
			this.MainTimer.Enabled = true;
			this.MainTimer.Interval = 50;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// MapPicture
			// 
			this.MapPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MapPicture.ContextMenuStrip = this.MapPanelMenu;
			this.MapPicture.Location = new System.Drawing.Point(12, 12);
			this.MapPicture.Name = "MapPicture";
			this.MapPicture.Size = new System.Drawing.Size(760, 524);
			this.MapPicture.TabIndex = 0;
			this.MapPicture.TabStop = false;
			this.MapPicture.Click += new System.EventHandler(this.MapPicture_Click);
			this.MapPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseDown);
			this.MapPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseMove);
			this.MapPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPicture_MouseUp);
			this.MapPicture.Resize += new System.EventHandler(this.MapPicture_Resize);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Status,
            this.ErrorStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 538);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(784, 23);
			this.statusStrip1.TabIndex = 1;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// Status
			// 
			this.Status.Name = "Status";
			this.Status.Size = new System.Drawing.Size(694, 18);
			this.Status.Spring = true;
			this.Status.Text = "Status";
			this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Status.Click += new System.EventHandler(this.Status_Click);
			// 
			// ErrorStatus
			// 
			this.ErrorStatus.ForeColor = System.Drawing.Color.Red;
			this.ErrorStatus.Name = "ErrorStatus";
			this.ErrorStatus.Size = new System.Drawing.Size(75, 18);
			this.ErrorStatus.Text = "ErrorStatus";
			this.ErrorStatus.Click += new System.EventHandler(this.ErrorStatus_Click);
			// 
			// MapPanelMenu
			// 
			this.MapPanelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChangeMeterPerLatLonMenuItem});
			this.MapPanelMenu.Name = "MapPanelMenu";
			this.MapPanelMenu.Size = new System.Drawing.Size(213, 26);
			this.MapPanelMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MapPanelMenu_Opening);
			// 
			// ChangeMeterPerLatLonMenuItem
			// 
			this.ChangeMeterPerLatLonMenuItem.Name = "ChangeMeterPerLatLonMenuItem";
			this.ChangeMeterPerLatLonMenuItem.Size = new System.Drawing.Size(212, 22);
			this.ChangeMeterPerLatLonMenuItem.Text = "ChangeMeterPerLatLon";
			this.ChangeMeterPerLatLonMenuItem.Click += new System.EventHandler(this.ChangeMeterPerLatLonMenuItem_Click);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 561);
			this.Controls.Add(this.MapPicture);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "MainWin";
			this.Text = "全国地図";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			((System.ComponentModel.ISupportInitialize)(this.MapPicture)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.MapPanelMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.PictureBox MapPicture;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel Status;
		private System.Windows.Forms.ToolStripStatusLabel ErrorStatus;
		private System.Windows.Forms.ContextMenuStrip MapPanelMenu;
		private System.Windows.Forms.ToolStripMenuItem ChangeMeterPerLatLonMenuItem;
	}
}

