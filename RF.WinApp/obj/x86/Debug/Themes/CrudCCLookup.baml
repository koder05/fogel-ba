   M S B A M L   `   `   `  ����S  ORF.WinApp.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure	RF.WinApp  5      D @RF.WinApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RF.WinApp	RF.WinApp 5         RF.WinApp.CrudCCLookupW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089% RF.WinApp.Object2ModelConverter   Object2ModelConverter  ValidationErrorTemplate  �� 
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
SourceName   5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      @vc9clr-namespace:RF.WinApp;assembly=RF.WinApp.Infrastructure  5      vc2clr-namespace:RF.WinApp  5      %�%  &	        &	      &	 �    &	 8    7  7  &	 .    &	 �    &	 X    &		 �    7  7  7  7  7    5      6    �� 5	      "  ��66   .R�5
   
   ]� ��.  .+�5      �� $��Right=�6   $��2,0,0,0q�6+   $(��"/RF.WinApp;component/Img/error.png��6<   $��14��6h   $��14��6s    6   �� $ 	MyAdorner��$8 2AdornedElement.(Validation.Errors)[0].ErrorContent��� 5      
���  ��$��1q�6'   ._�5      �� $��	MyAdorner��6,   5      5      5      �� 5      "  ��63   .R�5   
   �� $ Border��6   $��1,0,0,0q�6!   
�������
�������$��24��6a   ._�5      F� $ Arrow��6   
�������$��Center=�6.   $��Center=�6K   "�����{��    р�  	= р�[     	6f   5       5   
   P� 5       �@J�IsMouseOver . . �5      �� $	 Border��6   
 � ��
 �����6C   �� 5      $	 Arrow��6   
 � ��
 �����6<   5      P� 5       � {� .  . �5      �� $	 Border��6   
 � �� �#c2c2c26C   �� 5      $	 Border��6   
 � �� �	#FF9191916D   �� 5      
 � �� �#aaaaaa6/   �� 5      $	 Arrow��6   
 � �� �	#FF9191916<   5       5"      �� 5$      "  6)   .�5%   
   �� 
 � ��$ Center=�65   �� 5&   
   
 � �� �#656A646'   �� 5'   
   
 � ��$ 18��6#   �� 5(   
   
 � ��$ Arial)�6'   �� 5)   
   
 � ��$ 10(�6%   �� 5*   
   
 � �� 5+      �� 5,      "   6"    5-      ���� 5.      'n�       " n�6    .�5/      �� 
 � ��$ 1��6G   �� 50      
 �@�Cursor$ SizeWE��6H   �� 51      
 � �� 52   "   �� 53   &   "  n�66   .R�54   *   �� #��z��61   55   '   57      58      �� 59      SortPathStyle " F�66   .�5:      �� 
 � ��
 � � �65   �� 5;      
 � ��$ Right=�6D   �� 5<      
 � ��$ Center=�6B   �� 5=      
 � ��$ 0,2,7,0q�67   �� 5>      
 � y�$ Hidden=�6;   5?      5@      .R�5A      � .+�5B      �� $��
MainBorder��6!   ���#EBFFE7._�5C      � .+�5D   "   �� $��3q�63   $��Center=�6>   $��Center=�6[   #��z��6v   # z 5E   7   #��z��6s   5F      5G      F� 5H      $��UpArrow��6   8��  ��5I      B� 5J   "   .��5K   &   E� $	 0,0&�61   .��5L   *   �� $	 7,0&�66   �� 5M   *   $ 3.5,6&�66   �� 5N   *   $	 0,0&�66   5O   '   5P   #   5R      F� 5S      $��	DownArrow��6   8�� ��5T      B� 5U   "   .��5V   &   E� $	 0,6&�61   .��5W   *   �� $	 7,6&�66   �� 5X   *   $ 3.5,0&�66   �� 5Y   *   $	 0,6&�66   5Z   '   5[   #   5]      n� 5^      $��PART_LeftHeaderGripper��6    $
��Left=�6>   ���#F7FEF76Y   n� 5_      $��PART_RightHeaderGripper��6    $��Right=�6?   ���#C6D7C56[   5`       5a      P� 5b       �@ SortDirection$ 
DescendingX�6;   . �5c      �� $	 UpArrow��6%   
 � y�$ Hidden=�6P   �� 5d      $	 	DownArrow��6%   
 � y�$ Visible=�6R   5e      P� 5f       �@ SortDirection$ 	AscendingX�6;   . �5g      �� $	 	DownArrow��6%   
 � y�$ Hidden=�6R   �� 5h      $	 UpArrow��6%   
 � y�$ Visible=�6P   5i      5k      5m      5n      �� 5p      "  6   .�5q   
   �� 
 � ��$ 0��6"   �� 5r   
   
 � �� 6%   Z� 5s      �� 5u      "  6!   .�5v   
   �� 
 � ��$ Arial)�61   �� 5w   
   
 � ��$ 10(�6/   �� 5x   
   
 � ��$ CharacterEllipsis=�63   �� 5y   
   
 � ��
 �   �61   �� 5z   
   
 � ��$ 0q�6,   �� 5{   
   
 � ��$ 18��6#   �� 5|   
   
 � �� 5}      �� 5~      "   6"   .R�5      �� #��z��6   #��z��5�      #��z��5�       . ._�5�      �� $��Center=�6+   #��z��5�   #   $��1,0,0,0q�5�   #   #��z��5�   #   # z 5�   #   # z 5�   #   5�      5�      5�       5�   
   P� 5�       �@ 
IsSelected . . �5�      �� 
 � �� �#E39E116/   �� 5�      
 � ��
 �   �6/   �� 5�      
 � ��
 ���� 60   5�      P� 5�       �@J�	IsFocused . . �5�      �� 
 � �� �#FEECC86/   5�      �� 5�       6$   �� $ ID� 06   .a�5�      �� 
 � ��
 �����6/   �� 5�      
 � ��
 �����6/   5�      5�      �� 5�      "  6!   .�5�   
   �� 
 � ��$ 0,0,0,0q�6#    5�   
   P� 5�       �@ AlternationIndex$ 0��62   . �5�      �� 
 � ��
 �����6/   5�      P� 5�       �@ AlternationIndex$ 1��62   . �5�      �� 
 � �� �#EFF3EF6/   5�      P� 5�       �@J�IsMouseOver . . �5�      �� 
 � �� �#FAE9C46/   5�      5�      �� 5�      "  6   .�5�   
   �� 
 �@ AutoGenerateColumns .  60   �� 5�   
   
 � ��
 �����6'   �� 5�   
   
 � �� �	#FF688CAF6(   �� 5�   
   
 � ��$ 0,0,0,1q�6,   �� 5�   
   
 � ��$
 Left=�60   �� 5�   
   
 � ��$	 Top=�6.   �� 5�   
   
 �@ 
IsReadOnly . 6'   �� 5�   
   
 �@ HeadersVisibility$ Column=�6.   �� 5�   
   
 �@ RowHeaderStyle8   6+   �� 5�   
   
 �@ ColumnHeaderStyle8  6.   �� 5�   
   
 �@ 	CellStyle8  6&   �� 5�   
   
 � ��8  6/   �� 5�   
   
 �@��AlternationCount$ 2��6-   �� 5�   
   
 �@ GridLinesVisibility$ Vertical=�60   �� 5�   
   
 �@ SelectionMode$ Extended=�6*   �� 5�   
   
 �@ ClipboardCopyMode$ ExcludeHeader=�6.   �� 5�   
   
 �@ VerticalGridLinesBrush �#C6D7C563   �� 5�   
   
 �@ SelectionUnit$ FullRow=�6*   �� 5�   
   
 �@$�IsVirtualizing . 6B   �� 5�   
   "
 �@ EnableColumnVirtualization .  67   �� 5�   
   
 �@ EnableRowVirtualization . 64   �� 5�   
   
 � �� . 6:   �� 5�   
   
 �@=�ErrorTemplate 65   Z� �� 5�   
   
 � �� 5�      �� 5�      "   6"   Crud�� 5�      ���� 5�      '��       " ��6    .�5�      �� 
 �@J�SnapsToDevicePixels . 6D   �� 5�      
 �@�OverridesDefaultStyle .  6F   �� 5�      
 � ��$ 0,0,0,0q�67   �� 5�      
 � ��$ 18��67   �� 5�      
 � ��$	 110��66   �� 5�      
 � �� 5�   "   �� 5�   &   "  ��66   .R�5�   *   �� $ PART_ContentHost��6B   $��0q�67   6B   5�   '   5�      5�      5�      .R�5�      �� .+�5�      �  5�      �� 5�   "   ��63   �� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+�� 5�   "   $��26 �63   �� 5�   "   $��16 �63   .+�5�      �� $ Border��6%   $��1q�65   
�������
�������$��0��6o   $��2��6    . ._�5�   "   �� $ 
Horizontal=�6-   .+�5�   &   �� $ ContentHost��6.   -   5        
���  ����5�   .   �� $! (vc:CrudLookup.LookupWidth)�$ OneWay=� �� *TemplatedParent+ 5�   .   ��  �� *TemplatedParent+$ TwoWay=�$ SelectedModel��� 5�   &   $ PART_Button��6-   -   5        8�� $��18��5�   s   |�.  6�   5�   #   5�      �� 5�      $ 
WarningImg��6$   $��2��68   $��2,0,0,0q�6H   $(��"/RF.WinApp;component/Img/error.png��6Y   $��14��6�   $��14��6�   $y�	Collapsed=�6�   5�        5�      $ aPopup��6*   .��5�      �� $ calDatePopupBorder��6%   $��1q�6A   
�������
�������$��0,1,0,0q�6{   $
��Auto��5�   -   #��z��6:   $
��Auto��6]   #��z��6k   ._�5�   "   �� $ DG_ScrollViewer��6/   |�.  ��5�   &   �� 5�   *   "  ��6:   .R�5�   .   �  5�   2   �� 5�   6   $
��Auto �6G   �� 5�   6   $��* �6G   �� 5�   6   $
��Auto �6G     5�   2   �� 5�   6   $
��Auto �6D   �� 5�   6   $��* �6D   �� 5�   6   $
��Auto �6D   .+�5�   2   �� #��Z! 69   |�.  ��6z   C� *�� $"" DataGridSelectAllButtonStyle��"#  +y�6�   �� *HeadersVisibility+#% Z$ #' Z&  �� "(  ��6�  �� *CellsPanelHorizontalOffset+ �� "(  
  5�   2   $!) PART_ColumnHeadersPresenter��6Q   $��1��6v   y�6�   �� *HeadersVisibility+#% Z* #' Z&  �� "(  �� 5�   2   $!+ PART_ScrollContentPresenter��6I   #, z��6n   $��2��6�   $��1��6�   �� 5�   2   $- PART_VerticalScrollBar��6<   $��2��6\   #/ z. 6l   $0 Vertical=�6�   $��1��6�   #y�z1 6�   #3 z2 6_  4 6   �� *VerticalOffset+$ OneWay=� �� *TemplatedParent+6_  � 5�   2   $��1��67   $��2��6G    5�   6   �� 5�   :   ��6K   �� **(NonFrozenColumnsViewportHorizontalOffset+ �� "(  �� 5�   :   $��* �6K   .+�5  6   �� $- PART_HorizontalScrollBar��6@   $��1��6b   #/ z5 6r   $0 
Horizontal=�6�   #y�z6 6�   #3 z7 6]  4 6�   �� *HorizontalOffset+$ OneWay=� �� *TemplatedParent+6]  5  3   5  /   5  +   .��5  &   �� # z 65   5  #   5     5
     �� 5     $8 FPopup��6!   .+�5       $9 PART_FilterBlockCC��5  2   ���	#FF688CAF#��z: 5  2   #��z; 5  2   $y�Hidden=�6_   $< Фильтр��5  2   $= Применить��6H   $> Очистить��6a    5  2   ��  �� $? FindAncestor=�"(  $ SelectedItem�.��5  "   �� $��Filter��63   5     5     5      5     P� 5     $@ aPopup��6B    �@ IsShow .  . �5     �� 
 �@ 
IsBindable .  6E   5     P� 5      �@=�HasError . . �5     �� $	 Border��6%   
 � ��
 �  ��6P   �� 5     $	 
WarningImg��6%   
 � y�$ Visible=�6S   �� 5      $	 
WarningImg��6%    6P   ��  �� *TemplatedParent+$) #(Validation.Errors)[0].ErrorContent�
 �@�ToolTip5!     5#     5%     5&     5'     