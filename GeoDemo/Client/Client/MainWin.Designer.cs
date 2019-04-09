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
			this.MainSplit = new System.Windows.Forms.SplitContainer();
			this.MapPanel = new System.Windows.Forms.Panel();
			this.MapPanelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.NoopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.MapPanelLayerMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.レイヤプリセットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.フルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.軽量ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.無しToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.中心からタイリングするToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.道路を表示するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.住所を表示するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.住所DlgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ルートの色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ルートの色MenuItem_Aqua = new System.Windows.Forms.ToolStripMenuItem();
			this.ルートの色MenuItem_OrangeRed = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客を表示するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客の色ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客の色MenuItem_DarkRed = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客の色MenuItem_Magenta = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客の色MenuItem_YellowGreen = new System.Windows.Forms.ToolStripMenuItem();
			this.顧客の色ヒットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ヒットした顧客の色MenuItem_DarkRed = new System.Windows.Forms.ToolStripMenuItem();
			this.ヒットした顧客の色MenuItem_Magenta = new System.Windows.Forms.ToolStripMenuItem();
			this.ヒットした顧客の色MenuItem_YellowGreen = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.リフレッシュToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.リフレッシュクイックToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.プロットクリアToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.プロットクリアOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.プロットを顧客情報として抽出するToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.プロットを顧客情報として抽出する_抽出MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ルート検索ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ルートクリアToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aroundヒットした顧客にメールを送るToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.メール送信ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aroundヒットした顧客をプロットするToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Aroundヒット_プロットToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aroundクリアToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Status = new System.Windows.Forms.TextBox();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.googleMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GoogleMapUrlMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GoogleMap表示MenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.MainSplit)).BeginInit();
			this.MainSplit.Panel1.SuspendLayout();
			this.MainSplit.Panel2.SuspendLayout();
			this.MainSplit.SuspendLayout();
			this.MapPanelMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainTimer
			// 
			this.MainTimer.Enabled = true;
			this.MainTimer.Interval = 50;
			this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// MainSplit
			// 
			this.MainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.MainSplit.Location = new System.Drawing.Point(0, 0);
			this.MainSplit.Name = "MainSplit";
			// 
			// MainSplit.Panel1
			// 
			this.MainSplit.Panel1.Controls.Add(this.MapPanel);
			// 
			// MainSplit.Panel2
			// 
			this.MainSplit.Panel2.Controls.Add(this.Status);
			this.MainSplit.Size = new System.Drawing.Size(994, 568);
			this.MainSplit.SplitterDistance = 764;
			this.MainSplit.TabIndex = 0;
			// 
			// MapPanel
			// 
			this.MapPanel.ContextMenuStrip = this.MapPanelMenu;
			this.MapPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MapPanel.Location = new System.Drawing.Point(0, 0);
			this.MapPanel.Name = "MapPanel";
			this.MapPanel.Size = new System.Drawing.Size(764, 568);
			this.MapPanel.TabIndex = 0;
			this.MapPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MapPanel_Paint);
			this.MapPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseDown);
			this.MapPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseMove);
			this.MapPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapPanel_MouseUp);
			// 
			// MapPanelMenu
			// 
			this.MapPanelMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NoopMenuItem,
            this.toolStripMenuItem2,
            this.MapPanelLayerMenu,
            this.レイヤプリセットToolStripMenuItem,
            this.タイルデータをローカルディスク経由で取得するToolStripMenuItem,
            this.中心からタイリングするToolStripMenuItem,
            this.道路を表示するToolStripMenuItem,
            this.住所を表示するToolStripMenuItem,
            this.住所DlgToolStripMenuItem,
            this.ルートの色ToolStripMenuItem,
            this.顧客を表示するToolStripMenuItem,
            this.顧客の色ToolStripMenuItem,
            this.顧客の色ヒットToolStripMenuItem,
            this.toolStripMenuItem1,
            this.リフレッシュToolStripMenuItem,
            this.リフレッシュクイックToolStripMenuItem,
            this.toolStripMenuItem3,
            this.プロットクリアToolStripMenuItem,
            this.プロットクリアOneToolStripMenuItem,
            this.プロットを顧客情報として抽出するToolStripMenuItem,
            this.ルート検索ToolStripMenuItem,
            this.ルートクリアToolStripMenuItem,
            this.aroundToolStripMenuItem,
            this.aroundヒットした顧客にメールを送るToolStripMenuItem,
            this.aroundヒットした顧客をプロットするToolStripMenuItem,
            this.aroundクリアToolStripMenuItem,
            this.toolStripMenuItem4,
            this.googleMapToolStripMenuItem});
			this.MapPanelMenu.Name = "MapPanelMenu";
			this.MapPanelMenu.Size = new System.Drawing.Size(341, 556);
			this.MapPanelMenu.Opening += new System.ComponentModel.CancelEventHandler(this.MapPanelMenu_Opening);
			// 
			// NoopMenuItem
			// 
			this.NoopMenuItem.Name = "NoopMenuItem";
			this.NoopMenuItem.Size = new System.Drawing.Size(340, 22);
			this.NoopMenuItem.Text = "nop";
			this.NoopMenuItem.Click += new System.EventHandler(this.NoopMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(337, 6);
			// 
			// MapPanelLayerMenu
			// 
			this.MapPanelLayerMenu.Name = "MapPanelLayerMenu";
			this.MapPanelLayerMenu.Size = new System.Drawing.Size(340, 22);
			this.MapPanelLayerMenu.Text = "レイヤ";
			// 
			// レイヤプリセットToolStripMenuItem
			// 
			this.レイヤプリセットToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.フルToolStripMenuItem,
            this.軽量ToolStripMenuItem,
            this.無しToolStripMenuItem});
			this.レイヤプリセットToolStripMenuItem.Name = "レイヤプリセットToolStripMenuItem";
			this.レイヤプリセットToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.レイヤプリセットToolStripMenuItem.Text = "レイヤ・プリセット";
			// 
			// フルToolStripMenuItem
			// 
			this.フルToolStripMenuItem.Name = "フルToolStripMenuItem";
			this.フルToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.フルToolStripMenuItem.Text = "フル";
			this.フルToolStripMenuItem.Click += new System.EventHandler(this.フルToolStripMenuItem_Click);
			// 
			// 軽量ToolStripMenuItem
			// 
			this.軽量ToolStripMenuItem.Name = "軽量ToolStripMenuItem";
			this.軽量ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.軽量ToolStripMenuItem.Text = "軽量";
			this.軽量ToolStripMenuItem.Click += new System.EventHandler(this.軽量ToolStripMenuItem_Click);
			// 
			// 無しToolStripMenuItem
			// 
			this.無しToolStripMenuItem.Name = "無しToolStripMenuItem";
			this.無しToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.無しToolStripMenuItem.Text = "無し";
			this.無しToolStripMenuItem.Click += new System.EventHandler(this.無しToolStripMenuItem_Click);
			// 
			// タイルデータをローカルディスク経由で取得するToolStripMenuItem
			// 
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem.Name = "タイルデータをローカルディスク経由で取得するToolStripMenuItem";
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem.Text = "タイルデータをローカルディスク経由で取得する";
			this.タイルデータをローカルディスク経由で取得するToolStripMenuItem.Click += new System.EventHandler(this.タイルデータをローカルディスク経由で取得するToolStripMenuItem_Click);
			// 
			// 中心からタイリングするToolStripMenuItem
			// 
			this.中心からタイリングするToolStripMenuItem.Name = "中心からタイリングするToolStripMenuItem";
			this.中心からタイリングするToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.中心からタイリングするToolStripMenuItem.Text = "中心からタイリングする";
			this.中心からタイリングするToolStripMenuItem.Click += new System.EventHandler(this.中心からタイリングするToolStripMenuItem_Click);
			// 
			// 道路を表示するToolStripMenuItem
			// 
			this.道路を表示するToolStripMenuItem.Name = "道路を表示するToolStripMenuItem";
			this.道路を表示するToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.道路を表示するToolStripMenuItem.Text = "道路を表示する";
			this.道路を表示するToolStripMenuItem.Click += new System.EventHandler(this.道路を表示するToolStripMenuItem_Click);
			// 
			// 住所を表示するToolStripMenuItem
			// 
			this.住所を表示するToolStripMenuItem.Name = "住所を表示するToolStripMenuItem";
			this.住所を表示するToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.住所を表示するToolStripMenuItem.Text = "住所を表示する";
			this.住所を表示するToolStripMenuItem.Click += new System.EventHandler(this.住所を表示するToolStripMenuItem_Click);
			// 
			// 住所DlgToolStripMenuItem
			// 
			this.住所DlgToolStripMenuItem.Name = "住所DlgToolStripMenuItem";
			this.住所DlgToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.住所DlgToolStripMenuItem.Text = "住所Dlg";
			this.住所DlgToolStripMenuItem.Click += new System.EventHandler(this.住所DlgToolStripMenuItem_Click);
			// 
			// ルートの色ToolStripMenuItem
			// 
			this.ルートの色ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ルートの色MenuItem_Aqua,
            this.ルートの色MenuItem_OrangeRed});
			this.ルートの色ToolStripMenuItem.Name = "ルートの色ToolStripMenuItem";
			this.ルートの色ToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.ルートの色ToolStripMenuItem.Text = "ルートの色";
			// 
			// ルートの色MenuItem_Aqua
			// 
			this.ルートの色MenuItem_Aqua.Name = "ルートの色MenuItem_Aqua";
			this.ルートの色MenuItem_Aqua.Size = new System.Drawing.Size(140, 22);
			this.ルートの色MenuItem_Aqua.Text = "Aqua";
			this.ルートの色MenuItem_Aqua.Click += new System.EventHandler(this.ルートの色MenuItem_Aqua_Click);
			// 
			// ルートの色MenuItem_OrangeRed
			// 
			this.ルートの色MenuItem_OrangeRed.Name = "ルートの色MenuItem_OrangeRed";
			this.ルートの色MenuItem_OrangeRed.Size = new System.Drawing.Size(140, 22);
			this.ルートの色MenuItem_OrangeRed.Text = "OrangeRed";
			this.ルートの色MenuItem_OrangeRed.Click += new System.EventHandler(this.ルートの色MenuItem_OrangeRed_Click);
			// 
			// 顧客を表示するToolStripMenuItem
			// 
			this.顧客を表示するToolStripMenuItem.Name = "顧客を表示するToolStripMenuItem";
			this.顧客を表示するToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.顧客を表示するToolStripMenuItem.Text = "顧客を表示する";
			this.顧客を表示するToolStripMenuItem.Click += new System.EventHandler(this.顧客を表示するToolStripMenuItem_Click);
			// 
			// 顧客の色ToolStripMenuItem
			// 
			this.顧客の色ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.顧客の色MenuItem_DarkRed,
            this.顧客の色MenuItem_Magenta,
            this.顧客の色MenuItem_YellowGreen});
			this.顧客の色ToolStripMenuItem.Name = "顧客の色ToolStripMenuItem";
			this.顧客の色ToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.顧客の色ToolStripMenuItem.Text = "顧客の色";
			// 
			// 顧客の色MenuItem_DarkRed
			// 
			this.顧客の色MenuItem_DarkRed.Name = "顧客の色MenuItem_DarkRed";
			this.顧客の色MenuItem_DarkRed.Size = new System.Drawing.Size(148, 22);
			this.顧客の色MenuItem_DarkRed.Text = "DarkRed";
			this.顧客の色MenuItem_DarkRed.Click += new System.EventHandler(this.顧客の色MenuItem_DarkRed_Click);
			// 
			// 顧客の色MenuItem_Magenta
			// 
			this.顧客の色MenuItem_Magenta.Name = "顧客の色MenuItem_Magenta";
			this.顧客の色MenuItem_Magenta.Size = new System.Drawing.Size(148, 22);
			this.顧客の色MenuItem_Magenta.Text = "Magenta";
			this.顧客の色MenuItem_Magenta.Click += new System.EventHandler(this.顧客の色MenuItem_Magenta_Click);
			// 
			// 顧客の色MenuItem_YellowGreen
			// 
			this.顧客の色MenuItem_YellowGreen.Name = "顧客の色MenuItem_YellowGreen";
			this.顧客の色MenuItem_YellowGreen.Size = new System.Drawing.Size(148, 22);
			this.顧客の色MenuItem_YellowGreen.Text = "YellowGreen";
			this.顧客の色MenuItem_YellowGreen.Click += new System.EventHandler(this.顧客の色MenuItem_YellowGreen_Click);
			// 
			// 顧客の色ヒットToolStripMenuItem
			// 
			this.顧客の色ヒットToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ヒットした顧客の色MenuItem_DarkRed,
            this.ヒットした顧客の色MenuItem_Magenta,
            this.ヒットした顧客の色MenuItem_YellowGreen});
			this.顧客の色ヒットToolStripMenuItem.Name = "顧客の色ヒットToolStripMenuItem";
			this.顧客の色ヒットToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.顧客の色ヒットToolStripMenuItem.Text = "顧客の色(ヒット)";
			// 
			// ヒットした顧客の色MenuItem_DarkRed
			// 
			this.ヒットした顧客の色MenuItem_DarkRed.Name = "ヒットした顧客の色MenuItem_DarkRed";
			this.ヒットした顧客の色MenuItem_DarkRed.Size = new System.Drawing.Size(148, 22);
			this.ヒットした顧客の色MenuItem_DarkRed.Text = "DarkRed";
			this.ヒットした顧客の色MenuItem_DarkRed.Click += new System.EventHandler(this.ヒットした顧客の色MenuItem_DarkRed_Click);
			// 
			// ヒットした顧客の色MenuItem_Magenta
			// 
			this.ヒットした顧客の色MenuItem_Magenta.Name = "ヒットした顧客の色MenuItem_Magenta";
			this.ヒットした顧客の色MenuItem_Magenta.Size = new System.Drawing.Size(148, 22);
			this.ヒットした顧客の色MenuItem_Magenta.Text = "Magenta";
			this.ヒットした顧客の色MenuItem_Magenta.Click += new System.EventHandler(this.ヒットした顧客の色MenuItem_Magenta_Click);
			// 
			// ヒットした顧客の色MenuItem_YellowGreen
			// 
			this.ヒットした顧客の色MenuItem_YellowGreen.Name = "ヒットした顧客の色MenuItem_YellowGreen";
			this.ヒットした顧客の色MenuItem_YellowGreen.Size = new System.Drawing.Size(148, 22);
			this.ヒットした顧客の色MenuItem_YellowGreen.Text = "YellowGreen";
			this.ヒットした顧客の色MenuItem_YellowGreen.Click += new System.EventHandler(this.ヒットした顧客の色MenuItem_YellowGreen_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(337, 6);
			// 
			// リフレッシュToolStripMenuItem
			// 
			this.リフレッシュToolStripMenuItem.Name = "リフレッシュToolStripMenuItem";
			this.リフレッシュToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.リフレッシュToolStripMenuItem.Text = "リフレッシュ";
			this.リフレッシュToolStripMenuItem.Click += new System.EventHandler(this.リフレッシュToolStripMenuItem_Click);
			// 
			// リフレッシュクイックToolStripMenuItem
			// 
			this.リフレッシュクイックToolStripMenuItem.Name = "リフレッシュクイックToolStripMenuItem";
			this.リフレッシュクイックToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.リフレッシュクイックToolStripMenuItem.Text = "リフレッシュ・クイック";
			this.リフレッシュクイックToolStripMenuItem.Click += new System.EventHandler(this.リフレッシュクイックToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(337, 6);
			// 
			// プロットクリアToolStripMenuItem
			// 
			this.プロットクリアToolStripMenuItem.Name = "プロットクリアToolStripMenuItem";
			this.プロットクリアToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.プロットクリアToolStripMenuItem.Text = "プロット・クリア";
			this.プロットクリアToolStripMenuItem.Click += new System.EventHandler(this.プロットクリアToolStripMenuItem_Click);
			// 
			// プロットクリアOneToolStripMenuItem
			// 
			this.プロットクリアOneToolStripMenuItem.Name = "プロットクリアOneToolStripMenuItem";
			this.プロットクリアOneToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.プロットクリアOneToolStripMenuItem.Text = "プロット・クリア (1個)";
			this.プロットクリアOneToolStripMenuItem.Click += new System.EventHandler(this.プロットクリアOneToolStripMenuItem_Click);
			// 
			// プロットを顧客情報として抽出するToolStripMenuItem
			// 
			this.プロットを顧客情報として抽出するToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.プロットを顧客情報として抽出する_抽出MenuItem});
			this.プロットを顧客情報として抽出するToolStripMenuItem.Name = "プロットを顧客情報として抽出するToolStripMenuItem";
			this.プロットを顧客情報として抽出するToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.プロットを顧客情報として抽出するToolStripMenuItem.Text = "プロットを顧客情報として抽出する";
			// 
			// プロットを顧客情報として抽出する_抽出MenuItem
			// 
			this.プロットを顧客情報として抽出する_抽出MenuItem.Name = "プロットを顧客情報として抽出する_抽出MenuItem";
			this.プロットを顧客情報として抽出する_抽出MenuItem.Size = new System.Drawing.Size(100, 22);
			this.プロットを顧客情報として抽出する_抽出MenuItem.Text = "抽出";
			this.プロットを顧客情報として抽出する_抽出MenuItem.Click += new System.EventHandler(this.プロットを顧客情報として抽出する_抽出MenuItem_Click);
			// 
			// ルート検索ToolStripMenuItem
			// 
			this.ルート検索ToolStripMenuItem.Name = "ルート検索ToolStripMenuItem";
			this.ルート検索ToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.ルート検索ToolStripMenuItem.Text = "ルート検索";
			this.ルート検索ToolStripMenuItem.Click += new System.EventHandler(this.ルート検索ToolStripMenuItem_Click);
			// 
			// ルートクリアToolStripMenuItem
			// 
			this.ルートクリアToolStripMenuItem.Name = "ルートクリアToolStripMenuItem";
			this.ルートクリアToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.ルートクリアToolStripMenuItem.Text = "ルート・クリア";
			this.ルートクリアToolStripMenuItem.Click += new System.EventHandler(this.ルートクリアToolStripMenuItem_Click);
			// 
			// aroundToolStripMenuItem
			// 
			this.aroundToolStripMenuItem.Name = "aroundToolStripMenuItem";
			this.aroundToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.aroundToolStripMenuItem.Text = "Around";
			this.aroundToolStripMenuItem.Click += new System.EventHandler(this.aroundToolStripMenuItem_Click);
			// 
			// aroundヒットした顧客にメールを送るToolStripMenuItem
			// 
			this.aroundヒットした顧客にメールを送るToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.メール送信ToolStripMenuItem});
			this.aroundヒットした顧客にメールを送るToolStripMenuItem.Name = "aroundヒットした顧客にメールを送るToolStripMenuItem";
			this.aroundヒットした顧客にメールを送るToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.aroundヒットした顧客にメールを送るToolStripMenuItem.Text = "Aroundヒットした顧客にメールを送る";
			// 
			// メール送信ToolStripMenuItem
			// 
			this.メール送信ToolStripMenuItem.Enabled = false;
			this.メール送信ToolStripMenuItem.Name = "メール送信ToolStripMenuItem";
			this.メール送信ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
			this.メール送信ToolStripMenuItem.Text = "メール送信";
			this.メール送信ToolStripMenuItem.Click += new System.EventHandler(this.メール送信ToolStripMenuItem_Click);
			// 
			// aroundヒットした顧客をプロットするToolStripMenuItem
			// 
			this.aroundヒットした顧客をプロットするToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Aroundヒット_プロットToolStripMenuItem});
			this.aroundヒットした顧客をプロットするToolStripMenuItem.Name = "aroundヒットした顧客をプロットするToolStripMenuItem";
			this.aroundヒットした顧客をプロットするToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.aroundヒットした顧客をプロットするToolStripMenuItem.Text = "Aroundヒットした顧客をプロットする";
			// 
			// Aroundヒット_プロットToolStripMenuItem
			// 
			this.Aroundヒット_プロットToolStripMenuItem.Name = "Aroundヒット_プロットToolStripMenuItem";
			this.Aroundヒット_プロットToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.Aroundヒット_プロットToolStripMenuItem.Text = "プロット";
			this.Aroundヒット_プロットToolStripMenuItem.Click += new System.EventHandler(this.Aroundヒット_プロットToolStripMenuItem_Click);
			// 
			// aroundクリアToolStripMenuItem
			// 
			this.aroundクリアToolStripMenuItem.Name = "aroundクリアToolStripMenuItem";
			this.aroundクリアToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.aroundクリアToolStripMenuItem.Text = "Aroundクリア";
			this.aroundクリアToolStripMenuItem.Click += new System.EventHandler(this.aroundクリアToolStripMenuItem_Click);
			// 
			// Status
			// 
			this.Status.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Status.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Status.Location = new System.Drawing.Point(0, 0);
			this.Status.Multiline = true;
			this.Status.Name = "Status";
			this.Status.ReadOnly = true;
			this.Status.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.Status.Size = new System.Drawing.Size(226, 568);
			this.Status.TabIndex = 0;
			this.Status.Text = "Ready...";
			this.Status.WordWrap = false;
			this.Status.TextChanged += new System.EventHandler(this.Status_TextChanged);
			this.Status.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Status_KeyPress);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(337, 6);
			// 
			// googleMapToolStripMenuItem
			// 
			this.googleMapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.GoogleMapUrlMenuItem,
            this.GoogleMap表示MenuItem});
			this.googleMapToolStripMenuItem.Name = "googleMapToolStripMenuItem";
			this.googleMapToolStripMenuItem.Size = new System.Drawing.Size(340, 22);
			this.googleMapToolStripMenuItem.Text = "GoogleMap";
			// 
			// GoogleMapUrlMenuItem
			// 
			this.GoogleMapUrlMenuItem.Name = "GoogleMapUrlMenuItem";
			this.GoogleMapUrlMenuItem.Size = new System.Drawing.Size(160, 22);
			this.GoogleMapUrlMenuItem.Text = "URLを表示する";
			this.GoogleMapUrlMenuItem.Click += new System.EventHandler(this.GoogleMapUrlMenuItem_Click);
			// 
			// GoogleMap表示MenuItem
			// 
			this.GoogleMap表示MenuItem.Name = "GoogleMap表示MenuItem";
			this.GoogleMap表示MenuItem.Size = new System.Drawing.Size(160, 22);
			this.GoogleMap表示MenuItem.Text = "表示";
			this.GoogleMap表示MenuItem.Click += new System.EventHandler(this.GoogleMap表示MenuItem_Click);
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(994, 568);
			this.Controls.Add(this.MainSplit);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MainWin";
			this.Text = "GeoDemo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.MainSplit.Panel1.ResumeLayout(false);
			this.MainSplit.Panel2.ResumeLayout(false);
			this.MainSplit.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MainSplit)).EndInit();
			this.MainSplit.ResumeLayout(false);
			this.MapPanelMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer MainTimer;
		private System.Windows.Forms.SplitContainer MainSplit;
		private System.Windows.Forms.Panel MapPanel;
		private System.Windows.Forms.TextBox Status;
		private System.Windows.Forms.ContextMenuStrip MapPanelMenu;
		private System.Windows.Forms.ToolStripMenuItem MapPanelLayerMenu;
		private System.Windows.Forms.ToolStripMenuItem タイルデータをローカルディスク経由で取得するToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem レイヤプリセットToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem フルToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 軽量ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 中心からタイリングするToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem リフレッシュToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 道路を表示するToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem リフレッシュクイックToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem NoopMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem プロットクリアToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 住所DlgToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ルート検索ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ルートクリアToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 無しToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 住所を表示するToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ルートの色ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ルートの色MenuItem_Aqua;
		private System.Windows.Forms.ToolStripMenuItem ルートの色MenuItem_OrangeRed;
		private System.Windows.Forms.ToolStripMenuItem プロットクリアOneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 顧客を表示するToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem プロットを顧客情報として抽出するToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem プロットを顧客情報として抽出する_抽出MenuItem;
		private System.Windows.Forms.ToolStripMenuItem aroundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aroundクリアToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aroundヒットした顧客にメールを送るToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem メール送信ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aroundヒットした顧客をプロットするToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Aroundヒット_プロットToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 顧客の色ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 顧客の色MenuItem_DarkRed;
		private System.Windows.Forms.ToolStripMenuItem 顧客の色MenuItem_Magenta;
		private System.Windows.Forms.ToolStripMenuItem 顧客の色MenuItem_YellowGreen;
		private System.Windows.Forms.ToolStripMenuItem 顧客の色ヒットToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ヒットした顧客の色MenuItem_DarkRed;
		private System.Windows.Forms.ToolStripMenuItem ヒットした顧客の色MenuItem_Magenta;
		private System.Windows.Forms.ToolStripMenuItem ヒットした顧客の色MenuItem_YellowGreen;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripMenuItem googleMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem GoogleMapUrlMenuItem;
		private System.Windows.Forms.ToolStripMenuItem GoogleMap表示MenuItem;
	}
}

