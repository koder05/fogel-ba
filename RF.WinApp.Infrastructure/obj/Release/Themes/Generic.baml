   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp  W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   ArrowContinue � Points  ArrowEnd  MouseOverListBrush �� 
StartPoint �� EndPoint � Offset � Color  SelectedItemListBrush �� Color �� Opacity  HalfOpacityBrush ( $DataGridColumnHeader_MainBorderStyle	 �� 
TargetType
 �� Property �� Value 4 0DataGridColumnHeader_PART_LeftHeaderGripperColor 5 1DataGridColumnHeader_PART_RightHeaderGripperColor & "DataGridColumnHeader_SortPathStyle 	 ColumnHeader=   7System.Windows.Controls.Primitives.DataGridColumnHeader �� 
TargetType � 	Resources �� ImageSource �� ContentStringFormat �� ContentStringFormat E� 
StartPoint �� Point �� Triggers P� Property P� Value �� 
TargetName 
 	RowHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader  Key  SelectPathStyle :� RoutedEvent �� TargetProperty g� Duration	 W� To W� From g� 	BeginTime �� Value �� Binding �� RelativeSource  �� AncestorType)  #System.Windows.Controls.DataGridRow! �� Path  	CellStyle*  $System.Windows.Controls.DataGridCell" J� SnapsToDevicePixels# �� Triggers  	ItemStyle # gridSplitterDotsBackgroundBrush  gridSplitterStyle
$ �� Top% �� Left  Validation_ErrorTemplate& ��Name' � ToolTip( �� ElementName/   )RF.WinApp.ShadedAdornedElementPlaceholder)  Name* ��Name+ �� RadiusX, �� RadiusY- J� IsHitTestVisible. P� 
SourceName   RF.WinApp.ActionBlock/ �� TypeInTargetAssembly0 �� 
ResourceId1 � ColumnDefinitions2 ��Name3 F�Name  DisabledBackgroundComboBox  DisabledBorderBrushComboBox  DisabledForegroundComboBox  DropDownPopupBorder4 ��Name  TextBoxWithAdornedPopup5 �� 	ClickMode6 d� 	IsChecked7 �� Mode  Control_ExtensionButton8 � LayoutTransform9 �� Angle   RF.WinApp.ActionTextBox: d�Name;   Action  TextBoxWithAdornedPopup2   RF.WinApp.AdornedPopup<   IsKeeperElement= �� BasedOn> $� IsVirtualizing? �� SelectionBoxItem@ �� SelectionBoxItemTemplateA ��Name  ComboBoxTextBoxB �� 
IsReadOnlyC �� 
IsReadOnlyD �� IsDropDownOpenE � AllowsTransparencyF � VerticalOffsetG � ActualWidthH �� MaxDropDownHeightI K� IsItemsHost-  'System.Windows.Input.KeyboardNavigationJ   DirectionalNavigation  ToolBarButtonBaseStyleK �� PaddingL J� Opacity  TabItem_CloseButtonM A�Name�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     !localclr-namespace:RF.WinApp    �� $: 4/RF.WinApp.Infrastructure;component/Themes/Base.xaml?��� $; 5/RF.WinApp.Infrastructure;component/Themes/Menus.xaml?��� $B </RF.WinApp.Infrastructure;component/Themes/CrudCCLookup.xaml?�%�+  &	       &	 .    &	 �     &	     &	 �    &	 �    &	 �    &	     &	     &		 �    7  7  &	
 �    7  &	 �	    &	 �    &	 �    &	 "    7  7  7  7  7  &	 �    (�� �    "/ d�$0 ControlBoxToggleButton��)7  7  7  7  (�� =    "/ d�$0 ControlBoxToggleButton2��)7  7  7  7  &	 �    '�� �    7  'G� �    &	 �    &	 ,    7  '  �    7  &	 )    (�� �    "/ ��$0 InThemeButtonBaseTemplate��)'�� �    '�� ;     7��'�� J     7  7  7  &	 �%    &	 �'    �  �   ��[  	=  z 
���   �� G �	   ��[  	= ��[ ��j ��j  z ��[  z ��[  	=  z 
���   ��� $	 0,0&�$	 0,1&���� $	 0.0Q�$ #FAE9C4��� $	 0.5Q�$ #F9E5BA��� $
 0.51Q�$ #F7DA9F��� $	 1.0Q�$ #F4D9A3���� $	 0,0&�$	 0,1&���� $	 0.0Q�$ #F3C667��� $	 0.5Q�$ #F0BA4C��� $
 0.51Q�$ #EAA008��� $	 1.0Q�$ #E39E11���� $ 	#FF688CAF��$ .3Q��� "	 ��.��� 
 � �� �#EBFFE7�� $ #F7FEF7���� $ #C6D7C5���� "	 F�.��� 
 � ��
 � � ��� 
 � ��$ Right=��� 
 � ��$ Center=��� 
 � ��$ 0,2,7,0q��� 
 � y�$ Hidden=��� "	   .��� 
 � ��$ Center=��� 
 � ��$ 35���� 
 � �� �� "    ���� 'n�       "	 n�.��� 
 � ��$ 1���� 
 �@�Cursor$ SizeWE���� 
 � �� �� " n�.R��� #��z��.R�� .+��� $��
MainBorder������ $F @/RF.WinApp;component/Img/modules_view_grid_header_background.png��._�� .+��� $��3q�$��Center=�$��Center=�#��z��# z #��z��F� $��UpArrow��8��  ��B� .��E� $	 0,0&�.���� $	 7,0&��� $ 3.5,6&��� $	 0,0&�F� $��	DownArrow��8�� ��B� .��E� $	 0,6&�.���� $	 7,6&��� $ 3.5,0&��� $	 0,6&�n� $��PART_LeftHeaderGripper��$
��Left=����#F7FEF7n� $��PART_RightHeaderGripper��$��Right=����#C6D7C5 P�  �@  SortDirection$ 
DescendingX�. ��� $ UpArrow��
 � y�$ Hidden=��� $ 	DownArrow��
 � y�$ Visible=�P�  �@  SortDirection$ 	AscendingX�. ��� $ 	DownArrow��
 � y�$ Hidden=��� $ UpArrow��
 � y�$ Visible=��� "	  .��� 
 � ��$ 20���� 
 � �� �� "   ���� 'n�       "	 n�.��� 
 � ��$ 1���� 
 � �� �� " n�.R��� #��z���� SelectPathStyle "	 F�.��� 
 � ��
 � � ��� 
 � ��$ Center=��� 
 � ��$ Center=��� 
 � y�$ Hidden=�.R�� .+��� $��
MainBorder������ $F @/RF.WinApp;component/Img/modules_view_grid_header_background.png��F� $��Arrow��8��  ��B� .��E� $	 0,0&�.���� $	 6,4&��� $	 0,8&��� $	 0,0&� P�  �@ IsRowSelected . . ��� $ Arrow��
 � y�$ Visible=�:� $ Binding.SourceUpdated��.[��� .���� .��W� $ Width�$ 0:0:0D�$ 50X�W� $ Width�$ 0:0:2D�$ 50X�$ 20X�$ 0:0:0X��� $
 True�� ��  �� *FindAncestor+"   $! Model.IsEditing�.a��� $ Arrow��
 � ��
 �  ���� "	  .��� 
 � ��$ Arial)��� 
 � ��$ 11(��� 
 � ��$ CharacterEllipsis=��� 
 � ��
 �   ��� 
 � ��$ 0q��� 
 � ��$ 20���� 
 � �� �� "  .R��� #��z��#��z��#��z��" . ._��� $��Center=�#��z��$��1,0,0,0q�#��z��# z #" z" # P�  �@ 
IsSelected . . ��� 
 � ��# �  �� 
 � ��
 �   ��� 
 � ��
 ���� P�  �@J�	IsFocused . . ��� 
 � ��# �  ��  �� $! ID� 0.a��� 
 � ��
 ������� 
 � ��
 ������� "	  .��� 
 � ��$ 0,0,0,0q�# P�  �@ AlternationIndex$ 0��. ��� 
 � ��
 �����P�  �@ AlternationIndex$ 1��. ��� 
 � �� �#EFF3EFP�  �@J�IsMouseOver . . ��� 
 � ��# �  �� $	 0,0&�$	 0,1&���� $ 	#FFE3EFFF��$ 0Q�� $ 	#FFAFD2FF��$ 1Q��� "	 ��.��� 
 � ��$ 8���� 
 �@J�SnapsToDevicePixels . �� 
 � |� .  �� 
 � ��$ 7,7,7,7q��� 
 �@�Cursor$ SizeWE���� 
 � �� �	#FF204D89�� 
 � �� �� .R��� ._��� $��3��$��19��.+��� 8��  $��2��$��2��$$ 1��$% 0���� 8�� $��2��$��2��$$ 5��$% 0���� 8�� $��2��$��2��$$ 9��$% 0���� 8�� $��2��$��2��$$ 13��$% 0���� 8�� $��2��$��2��$$ 17��$% 0���� #��z��$��2��$��2��$$ 0��$% 0���� #��z��$��2��$��2��$$ 4��$% 0���� #��z��$��2��$��2��$$ 8��$% 0���� #��z��$��2��$��2��$$ 12��$% 0���� #��z��$��2��$��2��$$ 16��$% 0���� " ��.R�]� ��.  .+��� $& 	ErrorSign��$��Right=�$��2,0,0,0q�$(��"/RF.WinApp;component/Img/error.png��$��14��$��14��' �� $( 	MyAdorner��$8! 2AdornedElement.(Validation.Errors)[0].ErrorContent�� .+�  $) 	MyAdorner���� $* 	RedBorder��
������ 
���  ��$��1��$+ 6��$, 6��- .  ���� $( 	MyAdorner��$! AdornedElement.Width����� $( 	MyAdorner��$! AdornedElement.Height� P� $. 	MyAdorner�� �@ IsShaded$
 True��. ��� 
 � y�$ 	Collapsed=��� " d�.R�� 1 �� #��z���� $��24 �.+��� $	* Btn��$��Right=�$��30��
�������$��1��
�������$��2��#��z��$+ 6��$, 6���� $
��Left=�$��30��
�������$��1��
�������#��z��$+ 6��$, 6���� $2 Border��
�������
�������$��10,0,0,0q�$��0,1,1,1q�F� $3 Arrow��$��1��
�������$��Center=�$��Center=�"�����{��    р�  	= р�[     	 P�  �@J�IsMouseOver . . ��� $	 Btn��
 � ��
 ������� $ Arrow��
 � ��
 �����P�  � {� .  . ��� $ Border��8   
 � ���� $ Border��8  
 � ���� 8  
 � ���� $ Arrow��8  
 � ���� " d�.R�� 1 �� #��z���� $��20 �.+��� $	* Btn��$��Right=�$��30��
�������$��1��
�������$��2��#��z���� $
��Left=�$��30��
�������$��1��
�������#��z��$+ 6��$, 6���� $2 Border��
�������
�������$��10,0,0,0q�$��0,1,1,1q�F� $3 Arrow��$��1��
�������$��Center=�$��Center=�"�����{��    р�  	= р�[     	 P�  �@J�IsMouseOver . . ��� $	 Btn��
 � ��
 ������� $ Arrow��
 � ��
 �����P�  � {� .  . ��� $ Border��8   
 � ���� $ Border��8  
 � ���� 8  
 � ���� $ Arrow��8  
 � ���� "	 ��.��� 
 � ��
 ������� 
 � ��
 ������� 
 � ��$ 1,0,1,1q��� "	 ��.��� 
 �@J�SnapsToDevicePixels . �� 
 � ��$ 18���� 
 � �� �� " ��.R�� .+��� 
�������
�������$��1��$+ 6��$, 6��#��z��#��z���� $4 PART_ContentHost��$��3,1,3,0q��� 
 �@=�ErrorTemplate8   �� "	 G�.��� 
 �@J�SnapsToDevicePixels . �� 
 � ��$ 18���� 
 � �� �� " G�.R�� .+��� 
�������
�������$��1��$+ 6��$, 6��#��z��#��z���� $4 PART_ContentHost��$��3,1,3,0q��� "	 ��.��� 
 � ��$ 18���� 
 �@J�SnapsToDevicePixels . �� 
 � �� �� " ��.R�� .+�d� $��ToggleButton��|�.  #��z��$5 Press=���C� *�� "/ d�$0 ControlBoxToggleButton��+6 �� $!! (local:AdornedPopup.IsShow)�$7 TwoWay=� �� *TemplatedParent+�� $4 PART_ContentHost��$��3,1,25,0q��� " d�.R�� .+��� $	* Btn��#��z��
�������$��1��
�������#��z��$+ 6��$, 6��F� $3 Arrow��
�������$��Center=�$��Center=�$��5,0,0,1q�"�����{��    р�  	= р�[     	8 �� $	9 -90Q� P�  �@J�IsMouseOver . . ��� $	 Btn��
 � ��
 ������� $ Arrow��
 � ��
 �����P�  � {� .  . ��� $ Arrow��8   
 � ���� "	  .��� 
 � ��$ 18���� 
 �@J�SnapsToDevicePixels . �� 
 � �� �� "  .R�� #��z��1 �� �� $��24 �.+�d� $: ExtensionButton��|�.  $5 Press=�8��  $��30��$��2��$��Right=�#��z; �� $
��Left=�#��z��
�������$��1��
�������$+ 6��$, 6���� $��Right=�$��1q�
�������$��1���� $4 PART_ContentHost��$��3,1,3,0q��� "	  .��� 
 � ��$ 18���� 
 �@J�SnapsToDevicePixels . �� 
 � �� �� "  .R�� #��z��1 �� �� $��20 �.+�d� $: ExtensionButton��|�.  $5 Press=�#���  $��26��$��2��$��Right=�#��z; d� $: PopupButton��|�.  < . $5 Press=���C� *�� "/ d�$0 ControlBoxToggleButton2��+6 �� $!! (local:AdornedPopup.IsShow)�$7 TwoWay=� �� *TemplatedParent+�� $4 PART_ContentHost��< . $��3,1,20,0q��� " ��.R��� $2 Border��$��1q����#F4D9A3
�������#��z��#��z��._��� #��z��$��Center=�$��Center=���.  P�  �@��IsDefaulted . . ��� $ Border��
 � ��$ 2q��� "	 ��.��� 
 �@J�SnapsToDevicePixels . �� 
 � �� C� *�� "/ ��$0 InThemeButtonBaseTemplate��+�� "	 ��8=   �� "	 ��.��� 
 � ��$
 Auto���� 
 � ��$ 18���� 
 �@J�SnapsToDevicePixels . �� %
 �@��IsSynchronizedWithCurrentItem$ FalseY��� 
 � ��$
 Left=��� 
 � ��$ Center=��� 
 �@�OverridesDefaultStyle . �� 
 � ��$
 Auto=��� 
 � ��$
 Auto=��� 
 � �� . �� 
 �@=�ErrorTemplate# �  �� 
 � �� �� .R�$� > . �� 
 � �� �� " ��.R�� .+�d� $��ToggleButton��|�.  $5 Press=���C� *�� "/ d�$0 ControlBoxToggleButton��+6 �� $! IsDropDownOpen�$7 TwoWay=� �� *TemplatedParent+�� $��ContentSite��- .  #��z? #��z@ #��z��$��6,1,23,1q�$��Center=�$
��Left=��� $A PART_EditableTextBox��8��  $
��Left=�$��Center=�$��6,3,23,3q�|�. 
������ $y�Hidden=�#C zB ��Z� � $��Popup��$��Bottom=�#��zD E . |�.  $��Slide=�$F 1��.��� $��DropDown��" . #��zG #��zH .+��� $2 DropDownBorder��#���  �� " . .���� I . $J 	Contained=� P�  �@��HasItems .  . ��� $ DropDownBorder��
 � ��$ 95��P�  � {� .  . ��� 8  
 � ���� 8  
 � ���� 
 �@J�Opacity$
 0.72Q�P�  �@��
IsGrouping . . ��� 
 � �� .  P� $. Popup�� �@�AllowsTransparency . P�  �@��
IsEditable . . ��� 
 � �� .  �� $ PART_EditableTextBox��
 � y�$ Visible=��� $ ContentSite��
 � y�$ Hidden=�# �� "	 ��.��� 
 �@J�SnapsToDevicePixels . �� 
 � ��
 ���� �� 
 � �� �� " ��.R��� $2 Border��$��0q�#��z��
������ $K 1q�$��1q�._�� .+��� $��0,0,0,0q�$��Center=�$��Stretch=���. �� $2 DisableCouple�����#777777$	L 0.5Q�$y�Hidden=� P�  �@J�IsMouseOver . . ��� $ Border��
 � �� �#E39E11P�  � {� .  . ��� $ DisableCouple��
 � y�$ Visible=�P�  �@ IsOpened$
 True��. ��� 
 � �� �#E39E11�� "	 ��.��� 
 �@J�SnapsToDevicePixels . �� 
 � ��
 ���� �� 
 � �� �� " ��.R�� $��3,0,0,0q�.+�A� $M BackgroundRing��$��16��$��16��
�������$��1��
�������$y�Hidden=��� $* Cross1��$��10��
�������$��2��8 �� $9 45Q��� $* Cross2��$��2��
�������$��10��8 �� $9 45Q� P�  �@J�IsMouseOver . . ��� $ BackgroundRing��
 � y�$ Visible=��� $ Cross1��
 � ��
 ������� $ Cross2��
 � ��
 �����