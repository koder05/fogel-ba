   M S B A M L   `   `   `  ���� `  \Microsoft.Practices.Prism, Version=4.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35xRclr-namespace:Microsoft.Practices.Prism.Regions;assembly=Microsoft.Practices.Prism!Microsoft.Practices.Prism.Regions  S ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp mLclr-namespace:RF.WinApp.Infrastructure.UIS;assembly=RF.WinApp.InfrastructureRF.WinApp.Infrastructure.UIS D @RF.WinApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp    RF.WinApp.ShellWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     dprismRclr-namespace:Microsoft.Practices.Prism.Regions;assembly=Microsoft.Practices.Prism     Hvc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure     \uisLclr-namespace:RF.WinApp.Infrastructure.UIS;assembly=RF.WinApp.Infrastructure     'vc2clr-namespace:RF.WinApp     -     �Name$  Shell�� � Title$$ Регионфонд Atlas 2.0��$	��569��$	��954��$	��200�� � Icon$+ %/RF.WinApp;component/Img/birdicon.ico��-  'RF.WinApp.Infrastructure.UIS.UISettings   
ControlUID$ RF.WinApp.ShellWindow��.��  RF.WinApp.ModalSpace  .u�� -    �Name$ 
GlobalGrid�� � RowDefinitions �� $��23 �$��23���� $
��345* ��� $��23 �$��23�� � ColumnDefinitions �� $
��211* ��� $
��499* ��� $
��222* �.+�  RF.WinApp.TabsPlatesCC  -   $��	MainFrame��$��1��$��3��5   /Microsoft.Practices.Prism.Regions.RegionManager   
RegionName$ WorkspaceRegion��o� -   $��menu1��$��23��$��Stretch=�$	��Top=�$��3��$ 
MenuRegion��.@�  RF.WinApp.SplitButton  $��	_Файл��.@�  RF.WinApp.RadioMenuHeader  -   $��	Themes...��   
DefaultKey$ RF.WinApp.Infrastructure��$. (RF.WinApp.Infrastructure.RadioMenuHeader��$ MenuThemesRegion��.@�  RF.WinApp.RadioMenuItem  $��Default��
	   Key$	 RF.WinApp.Infrastructure��m� $��
Выход��
 m� Command
 �� *ExitCommand+�� -   $��
statusBar1��$��23��$��Stretch=�$��0,0,0,0q�$��bottom=�$��3��$��2��.@��� ���� *Status+  RF.WinApp.ActionBlock  -     Name$ 
ModalPlace��$	��320��$	��300��$��3��$��2��$��1��   BlockCaption$ 
ModalPlace��   ApplyCaption$ OK��   ClearCaption$ Отмена��$ ModalRegion��   ModalScopeElement ��  �� ElementName$ 
GlobalGrid������  �� Color$ 	#FF688CAF�� �� Opacity$ .3Q�