   M S B A M L   `   `   `  ���� S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  D @RF.WinApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp    RF.WinApp.CrudCCLookupW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089% RF.WinApp.Object2ModelConverter   Object2ModelConverter  ValidationErrorTemplate  �� 
TargetType � ToolTip �� ElementName �� Path  ComboBoxToggleButton ��Name F�Name �� Triggers P� Property P� Value	 �� 
TargetName
 �� Property �� Value  ColumnHeaderInLookup �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader � 	Resources  Key �� ContentStringFormat �� ContentStringFormat  SortPathStyle E� 
StartPoint �� Point  	RowHeader:  4System.Windows.Controls.Primitives.DataGridRowHeader  CellInLookup*  $System.Windows.Controls.DataGridCell J� SnapsToDevicePixels �� Triggers �� Binding �� Value  ItemInLookup)  #System.Windows.Controls.DataGridRow   RF.WinApp.CrudCC 
	 Lookup&   System.Windows.Controls.DataGrid ��Name � ColumnDefinitions �� Mode �� RelativeSource �� Orientation ��Name � DataContext ��Name ��Name   RF.WinApp.AdornedPopup  Name  � RowDefinitions!   SelectAllCommand" �� 
ResourceId# �� TypeInTargetAssembly7	  1System.Windows.Controls.DataGridHeadersVisibility
$ 	  All% �� ConverterParameter!&   HeadersVisibilityConverter' �� 	Converter( �� AncestorTypeG
  ASystem.Windows.Controls.Primitives.DataGridColumnHeadersPresenter) 
 Name* 	  Column+ ��Name, �� CanContentScroll- ��Name. �� ScrollableHeight/ � Maximum0 �� Orientation*1 �� #ComputedVerticalScrollBarVisibility2 �� ViewportHeight3 �� ViewportSize4 � Value5 �� ScrollableWidth,6 �� %ComputedHorizontalScrollBarVisibility7 �� ViewportWidth8 ��Name   RF.WinApp.FilterBlockCC9  Name:   FilterBlockWidth;   FilterBlockHeight   RF.WinApp.ActionBlock<   BlockCaption=   ApplyCaption>   ClearCaption? �� Mode@ P� 
SourceName   D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     @vc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure  vc2clr-namespace:RF.WinApp  %  &	        &	      &	 �     &	 �    7  7  &	 L    &	 �    &	 �	    &		 �
    7  7  7  7  7    �� "  ��.R�]� ��.  .+��� $��Right=�$��2,0,0,0q�$(��"/RF.WinApp;component/Img/error.png��$��14��$��14�� �� $ 	MyAdorner��$8 2AdornedElement.(Validation.Errors)[0].ErrorContent��� 
���  ��$��1q�._��� $��	MyAdorner���� "  ��.R��� $ Border��$��1,0,0,0q�
�������
�������$��24��._�F� $ Arrow��
�������$��Center=�$��Center=�"�����{��    р�  	= р�[     	 P�  �@J�IsMouseOver . . ��� $	 Border��
 � ��
 ������� $	 Arrow��
 � ��
 �����P�  � {� .  . ��� $	 Border��
 � �� �#c2c2c2�� $	 Border��
 � �� �	#FF919191�� 
 � �� �#aaaaaa�� $	 Arrow��
 � �� �	#FF919191�� "  .��� 
 � ��$ Center=��� 
 � �� �#656A64�� 
 � ��$ 18���� 
 � ��$ Arial)��� 
 � ��$ 10(��� 
 � �� �� "    ���� 'n�       " n�.��� 
 � ��$ 1���� 
 �@�Cursor$ SizeWE���� 
 � �� �� "  n�.R��� #��z���� SortPathStyle " F�.��� 
 � ��
 � � ��� 
 � ��$ Right=��� 
 � ��$ Center=��� 
 � ��$ 0,2,7,0q��� 
 � y�$ Hidden=�.R�� .+��� $��
MainBorder�����#EBFFE7._�� .+��� $��3q�$��Center=�$��Center=�#��z��# z #��z��F� $��UpArrow��8��  ��B� .��E� $	 0,0&�.���� $	 7,0&��� $ 3.5,6&��� $	 0,0&�F� $��	DownArrow��8�� ��B� .��E� $	 0,6&�.���� $	 7,6&��� $ 3.5,0&��� $	 0,6&�n� $��PART_LeftHeaderGripper��$
��Left=����#F7FEF7n� $��PART_RightHeaderGripper��$��Right=����#C6D7C5 P�  �@ SortDirection$ 
DescendingX�. ��� $	 UpArrow��
 � y�$ Hidden=��� $	 	DownArrow��
 � y�$ Visible=�P�  �@ SortDirection$ 	AscendingX�. ��� $	 	DownArrow��
 � y�$ Hidden=��� $	 UpArrow��
 � y�$ Visible=��� "  .��� 
 � ��$ 0���� 
 � �� Z� �� "  .��� 
 � ��$ Arial)��� 
 � ��$ 10(��� 
 � ��$ CharacterEllipsis=��� 
 � ��
 �   ��� 
 � ��$ 0q��� 
 � ��$ 18���� 
 � �� �� "   .R��� #��z��#��z��#��z�� . ._��� $��Center=�#��z��$��1,0,0,0q�#��z��# z # z  P�  �@ 
IsSelected . . ��� 
 � �� �#E39E11�� 
 � ��
 �   ��� 
 � ��
 ���� P�  �@J�	IsFocused . . ��� 
 � �� �#FEECC8��  �� $ ID� 0.a��� 
 � ��
 ������� 
 � ��
 ������� "  .��� 
 � ��$ 0,0,0,0q� P�  �@ AlternationIndex$ 0��. ��� 
 � ��
 �����P�  �@ AlternationIndex$ 1��. ��� 
 � �� �#EFF3EFP�  �@J�IsMouseOver . . ��� 
 � �� �#FAE9C4�� "  .��� 
 �@ AutoGenerateColumns .  �� 
 � ��
 ������� 
 � �� �	#FF688CAF�� 
 � ��$ 0,0,0,1q��� 
 � ��$
 Left=��� 
 � ��$	 Top=��� 
 �@ 
IsReadOnly . �� 
 �@ HeadersVisibility$ Column=��� 
 �@ RowHeaderStyle8   �� 
 �@ ColumnHeaderStyle8  �� 
 �@ 	CellStyle8  �� 
 � ��8  �� 
 �@��AlternationCount$ 2���� 
 �@ GridLinesVisibility$ Vertical=��� 
 �@ SelectionMode$ Extended=��� 
 �@ ClipboardCopyMode$ ExcludeHeader=��� 
 �@ VerticalGridLinesBrush �#C6D7C5�� 
 �@ SelectionUnit$ FullRow=��� 
 �@$�IsVirtualizing . �� "
 �@ EnableColumnVirtualization .  �� 
 �@ EnableRowVirtualization . �� 
 � �� . �� 
 �@=�ErrorTemplate Z� �� 
 � �� �� "   Crud�� ���� '��       " ��.��� 
 �@J�SnapsToDevicePixels . �� 
 �@�OverridesDefaultStyle .  �� 
 � ��$ 0,0,0,0q��� 
 � ��$ 18���� 
 � ��$	 110���� 
 � �� �� "  ��.R��� $ PART_ContentHost��$��0q�.R��� .+��  �� ���� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+�� $��26 ��� $��16 �.+��� $ Border��$��1q�
�������
�������$��0��$��2�� . ._��� $ 
Horizontal=�.+��� $ ContentHost��-   
���  ������ $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ ��  �� *TemplatedParent+$ TwoWay=�$ SelectedModel��� $ PART_Button��-   8�� $��18��|�.  �� $ 
WarningImg��$��2��$��2,0,0,0q�$(��"/RF.WinApp;component/Img/error.png��$��14��$��14��$y�	Collapsed=�  $ aPopup��.���� $ calDatePopupBorder��$��1q�
�������
�������$��0,1,0,0q�$
��Auto��#��z��$
��Auto��#��z��._��� $ DG_ScrollViewer��|�.  ���� "  ��.R��  �� $
��Auto ��� $��* ��� $
��Auto �  �� $
��Auto ��� $��* ��� $
��Auto �.+��� #��Z! |�.  ��C� *�� $"" DataGridSelectAllButtonStyle��"#  +y��� *HeadersVisibility+#% Z$ #' Z&  �� "(  ���� *CellsPanelHorizontalOffset+ �� "(  
  $!) PART_ColumnHeadersPresenter��$��1��y��� *HeadersVisibility+#% Z* #' Z&  �� "(  �� $!+ PART_ScrollContentPresenter��#, z��$��2��$��1���� $- PART_VerticalScrollBar��$��2��#/ z. $0 Vertical=�$��1��#y�z1 #3 z2 4 �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+� $��1��$��2�� �� ���� **(NonFrozenColumnsViewportHorizontalOffset+ �� "(  �� $��* �.+��� $- PART_HorizontalScrollBar��$��1��#/ z5 $0 
Horizontal=�#y�z6 #3 z7 4 �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+.���� # z �� $8 FPopup��.+�  $9 PART_FilterBlockCC�����	#FF688CAF#��z: #��z; $y�Hidden=�$< Фильтр��$= Применить��$> Очистить�� ��  �� $? FindAncestor=�"(  $ SelectedItem�.���� $��Filter�� P� $@ aPopup�� �@ IsShow .  . ��� 
 �@ 
IsBindable .  P�  �@=�HasError . . ��� $	 Border��
 � ��
 �  ���� $	 
WarningImg��
 � y�$ Visible=��� $	 
WarningImg�� ��  �� *TemplatedParent+$) #(Validation.Errors)[0].ErrorContent�
 �@�ToolTip